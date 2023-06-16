using backend.DTOs.Request;
using backend.DTOs.Response;
using backend.Exceptions;
using backend.Services;
using backend.Wrappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers {

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly JwtService _jwtService;
        private readonly UserService _userService;

        public AuthController(UserService userService, JwtService jwtService) {
            _userService = userService;
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
        public async Task<IActionResult> Register(UserRegisterRequestDto request) {


            if(request.Password != request.ConfirmPassword) {
                ModelState.AddModelError(nameof(request.ConfirmPassword), "Passwords do not match.");
            }

            if(!ModelState.IsValid) {
                return UnprocessableEntity(ModelState);
            }

            Boolean result = await _userService.RegisterUser(request);

            if (result) {
                return Ok(new ApiResponse<UserLoginResponseDto>() {
                    Succeeded = true
                });
            }
            return Ok(new ApiResponse<UserLoginResponseDto>() {
                Succeeded = false
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
        public async Task<IActionResult> RevokeToken(string token) {

            var response = await _jwtService.RevokeToken(token);

            if(response) {
                return Ok("Refresh Token revoked successfully.");
            }
            return BadRequest("Invalid token");




        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequestDto request) {
            var result = await _userService.Login(request);

            return Ok(result);

        }
    }
}
