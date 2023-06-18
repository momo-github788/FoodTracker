using backend.DTOs.Request;
using backend.DTOs.Response;
using backend.Exceptions;
using backend.Models;
using backend.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Security.Policy;

namespace backend.Services.impl {
    public class AuthServiceImpl : AuthService{
        private readonly JwtService _jwtService;
        private readonly RoleService _roleService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthServiceImpl(JwtService jwtService, SignInManager<User> signInManager, TokenValidationParameters tokenValidationParameters,
            IUnitOfWork unitOfWork, RoleService roleService, UserManager<User> userManager) {
            _roleService = roleService;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _tokenValidationParameters = tokenValidationParameters;

        }


        public async Task<User> RegisterUser(UserRegisterRequest request) {

      

            var userNameExists = await _userManager.FindByNameAsync(request.UserName);
            var emailExists = await _userManager.FindByEmailAsync(request.EmailAddress);

            if (userNameExists != null || emailExists != null) {
                throw new BadRequestException("Username/Email Address already exists.");
            }

            var user = new User() {
                UserName = request.UserName,
                Email = request.EmailAddress,
                //EmailConfirmed = false
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded) {
      
                var userRole = UserRoles.USER.ToString();
                var adminRole = UserRoles.ADMIN.ToString();

                await _roleService.CreateRoleIfNotExists(userRole);
                await _roleService.CreateRoleIfNotExists(adminRole);


                await _userManager.AddToRolesAsync(user, new string[] { userRole, adminRole });
                return user;
            }

            return null;


        }


        public async Task<UserLoginResponse> Login(UserLoginRequest request) {
            // Use username to authenticate
            var user = await _userManager.FindByNameAsync(request.UserName);

            // Valid user
            var success = await _userManager.CheckPasswordAsync(user, request.Password);

            if (success) {
                if (!user.EmailConfirmed) {
                    throw new BadRequestException("Email address is not verified.");
                }
                var response = await _jwtService.GenerateJwtToken(user);
                return response;
            }

            throw new BadRequestException("Invalid credentials");
        }

        public async Task<UserLoginResponse> GoogleLogin(UserLoginRequest request, Url responseUrl) {
            return null;
        }

    }
}
