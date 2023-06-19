using System.ComponentModel.DataAnnotations;
using backend.DTOs.Request;
using backend.DTOs.Response;
using backend.Exceptions;
using backend.Models;
using backend.Services;
using backend.Wrappers;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using backend.Repository;
using backend.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.Controllers {

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly ConfirmationTokenService _confirmationTokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtService _jwtService;
        private readonly AuthService _authService;

        public AuthController(ConfirmationTokenService confirmationTokenService, IUnitOfWork unitOfWork, EmailService emailService, AuthService AuthService, IHttpContextAccessor context, UserManager<User> userManager, JwtService jwtService, SignInManager<User> signInManager) {
            _authService = AuthService;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _context = context;
            _emailService = emailService;
            _confirmationTokenService = confirmationTokenService;
        }

        [AllowAnonymous]
        [HttpGet("/Error")]
        public string Error() {
            throw new InvalidTokenException("Invalid token");
            return "d";

        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterRequest request) {

            if (request == null) {
                return BadRequest("Enter a value");
            }


            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = await _authService.RegisterUser(request);

            if (user != null) {

                return Ok(new ApiResponse<List<string>>() {
                    Succeeded = true,
                    Message = "Account created successfully."
                });
            }

            return BadRequest(new ApiResponse<List<string>>() {
                Succeeded = false,
                Message = "Error creating account.",
                Errors = new[] {
                    "Error creating account."
                }
            });

        }

        [AllowAnonymous]
        [Route("ForgotPassword")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([Required] string emailAddress) {
            var response = await _authService.ForgotPassword(emailAddress);

            return Ok(response);
        }


        [AllowAnonymous]
        [Route("ResetPassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request) {
            var success = await _authService.ResetPassword(request);

            if (success) {


                return Ok(new ApiResponse<List<string>>{
                    Succeeded = true,
                    Message = "Password has been changed successfully."
                });
            }

            return BadRequest(new ApiResponse<List<string>>{
                Succeeded = false,
                Message = "There was an error changing your password, please try again later.",
                Errors = new[] {
                    "There was an error changing your password, please try again later."
                }
            });
        }

        [AllowAnonymous]
        [Route("ResetPassword")]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string passwordResetToken, string emailAddress) {
            var request = new ResetPasswordRequest {
                PasswordResetToken = passwordResetToken,
                EmailAddress = emailAddress
            };

            return Ok(new {
                request
            });

        }



        [AllowAnonymous]
        [Route("ConfirmEmail")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string confirmationToken) {

            var status = await _confirmationTokenService.ConfirmToken(userId, confirmationToken);

            if (status) {
                return Ok("Email address confirmed.");
            }
            return BadRequest("Error confirming email, please try again later");
        }

        [AllowAnonymous]
        [Route("ResendConfirmationEmail")]
        [HttpGet]
        public async Task<IActionResult> ResendConfirmationEmail(string oldConfirmationToken) {

            var response = await _confirmationTokenService.GenerateNewConfirmationToken(oldConfirmationToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenRequest request) {

            var response = await _jwtService.ValidateAndGenerateNewTokens(request);

            if(response == null) {
                return BadRequest("Invalid token");
            }

            return Ok(response);


        }


        [HttpPost]
        [Route("RevokeToken")]
        public async Task<IActionResult> RevokeToken(string refreshToken) {

            var response = await _jwtService.RevokeToken(refreshToken);

            if(response) {
                return Ok("Refresh Token revoked successfully.");
            }
            return BadRequest("Invalid token");
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("GetToken")]
        public async Task<IActionResult> test([FromQuery] string confirmationToken) {
            //var user = await _userManager.FindByNameAsync(AuthUtils.getPrincipal(_context));

            //Console.WriteLine(AuthUtils.getPrincipal(_context));

            //if (user == null) {
            //    return BadRequest("Not found");
            //}


            var response = await _confirmationTokenService.GetConfirmationToken(confirmationToken);
     
            //_emailService.sendEmail("axel.nienow@ethereal.email", "Test", "This is test", user.Email);
            return Ok(response);
        }

            [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequest request) {
            var result = await _authService.Login(request);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpGet("GoogleResponse")]
        public async Task<IActionResult> GoogleResponse() {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
        

            if (info == null)
                return RedirectToAction(nameof(Login));


            Console.WriteLine(info.Principal.Claims.ToList());
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            //string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            if (result.IsLockedOut) {
                return BadRequest("Account is locked");
            }

            var user = new User {
                Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
            };


            if (result.Succeeded) {
                return Ok("Existing user.. sign in " + user);
            } else {
                IdentityResult identResult = await _userManager.CreateAsync(user);
                if (identResult.Succeeded) {
                    identResult = await _userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded) {
                        await _signInManager.SignInAsync(user, false);
                        return Ok("New user..Added " + user);
                    }
                }
                return BadRequest("Error ");
            }
        }


        [AllowAnonymous]
        [HttpGet("SignIn/{provider}")]
        public ActionResult<string> SignIn(string provider) {

            //var authProperties = new AuthenticationProperties {
            //    RedirectUri = Url.Action(nameof(GoogleResponse), pr, "google"),
            //};
            //return Challenge(authProperties, provider);

            var redirectUrl = Url.Action(nameof(GoogleResponse));
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);

            //return new ChallengeResult(
            //    GoogleDefaults.AuthenticationScheme, new AuthenticationProperties {
            //        // Maps action name to it's URL value
            //        RedirectUri = Url.Action(nameof(GoogleResponse))
            //    });


        }

        [AllowAnonymous]
        [HttpGet("GoogleLogout")]
        public async Task<ActionResult<string>> logout() {
            await HttpContext.SignOutAsync();
            return Redirect(Url.Action(nameof(Login)));
        }

    }
}
