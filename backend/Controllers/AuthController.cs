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

        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtService _jwtService;
        private readonly AuthService _AuthService;

        public AuthController(IUnitOfWork unitOfWork, EmailService emailService, AuthService AuthService, IHttpContextAccessor context, UserManager<User> userManager, JwtService jwtService, SignInManager<User> signInManager) {
            _AuthService = AuthService;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _context = context;
            _emailService = emailService;
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

            var user = await _AuthService.RegisterUser(request);

            if (user != null) {

                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var emailBody = $"Please confirm your email address <a href=\"#URL#\"> Click here</a>";

                await _unitOfWork.ConfirmationTokens.Add(new ConfirmationToken {
                    EmailConfirmationToken = confirmationToken,
                    UserId = user.Id
                });
                var result = _unitOfWork.Save();

                if(result < 1) {
                    Console.WriteLine("Error saving coonfirm token");
                    return null;
                }


                var callback_url = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Auth",
                new { 
                    userId = user.Id,
                    confirmationToken = confirmationToken
                });

                var body = emailBody.Replace("#URL#",
                    System.Text.Encodings.Web.HtmlEncoder.Default.Encode(callback_url));

                Console.WriteLine("callback: " + callback_url);

                _emailService.sendEmail("axel.nienow@ethereal.email", "Email Verification", body, user.Email);

         

                return Ok(new ApiResponse<UserLoginResponse>() {
                    Succeeded = true,
                    Message = "Account created successfully."
                });
            }

            return BadRequest(new ApiResponse<UserLoginResponse>() {
                Succeeded = false,
                Message = "Error creating account."
            });

        }



            [AllowAnonymous]
        [Route("ConfirmEmail")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string confirmationToken) {
            if (userId == null || confirmationToken == null) {
                return BadRequest("Invalid Email confirmation Token");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) {
                return BadRequest("Invalid Email parameters");
            }

            if (user.EmailConfirmed) {
                return BadRequest("Your email address has already been verified");
            }

            var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);

            var status = result.Succeeded
                ? "Email has been verified successfully"
                : "Email was not verified, please check your email for a new confirmation link";

            return Ok(status);
        }

        [AllowAnonymous]
        [Route("ResendConfirmationEmail")]
        [HttpGet]
        public async Task<IActionResult> ResendConfirmationEmail(string userId, string oldConfirmationToken) {
            return Ok();
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


        [Authorize]
        [HttpGet("Welcome")]
        public async Task<IActionResult> Welcome() {
            var user = await _userManager.FindByNameAsync(AuthUtils.getPrincipal(_context));

            Console.WriteLine(AuthUtils.getPrincipal(_context));

            if (user == null) {
                return BadRequest("Not found");
            }


            //_emailService.sendEmail("axel.nienow@ethereal.email", "Test", "This is test", user.Email);
            return Ok(user);
        }

            [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequest request) {
            var result = await _AuthService.Login(request);

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
