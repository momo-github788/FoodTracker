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

        private readonly SignInManager<User> _signInManager;
        private readonly JwtService _jwtService;
        private readonly AuthService _AuthService;

        public AuthController(AuthService AuthService, JwtService jwtService, SignInManager<User> signInManager) {
            _AuthService = AuthService;
            _signInManager = signInManager;
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

            Console.Write("Auth sucess");
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });
            return Ok("wooo");
        }

        [AllowAnonymous]
        [HttpGet("signin-google")]
        public ActionResult<string> home() {

            return new ChallengeResult(
                GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties {
                    RedirectUri = Url.Action("GoogleResponse") // Where google responds back
                });

            
        }


    }
}
