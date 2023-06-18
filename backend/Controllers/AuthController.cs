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
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

namespace backend.Controllers {

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtService _jwtService;
        private readonly AuthService _AuthService;

        public AuthController(AuthService AuthService, UserManager<User> userManager, JwtService jwtService, SignInManager<User> signInManager) {
            _AuthService = AuthService;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
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

   
            Boolean result = await _AuthService.RegisterUser(request);

            if (result) {
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
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequest request) {
            var result = await _AuthService.Login(request);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpGet("GoogleResponse")]
        public async Task<IActionResult> GoogleResponse() {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            Console.Write("info: " + info.LoginProvider);
            //return Ok("wooo");
            if (info == null)
                return RedirectToAction(nameof(Login));

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            Console.WriteLine(userInfo);

            if (result.Succeeded)
                return Ok("Login suces");
            else {
                var user = new User {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };

                IdentityResult identResult = await _userManager.CreateAsync(user);
                if (identResult.Succeeded) {
                    identResult = await _userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded) {
                        await _signInManager.SignInAsync(user, false);
                        return Ok("Successly added login to db");
                    }
                }
                return BadRequest("Error ");
            }
        }


        //[AllowAnonymous]
        //[Route("SignIn/{provider}")]
        //public IActionResult SignIn(string provider) {
        //    return Challenge(new AuthenticationProperties(), provider);
        //}

        [AllowAnonymous]
        [HttpGet("SignIn/{provider}")]
        public ActionResult<string> SignIn(string provider) {

            var authProperties = new AuthenticationProperties {
                RedirectUri = Url.Action(nameof(GoogleResponse)),
                Items = { new KeyValuePair<string, string>("LoginProvider", provider),
                 new KeyValuePair<string, string>("NameIdentifier", provider)}
            };
            return Challenge(authProperties, provider);

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
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            result.Principal.Identities.FirstOrDefault().Claims.ToList()
            .ForEach(s => Console.WriteLine(s));
            return Ok("Logged out");
        }

    }
}
