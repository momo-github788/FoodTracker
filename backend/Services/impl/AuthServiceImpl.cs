using backend.DTOs.Request;
using backend.DTOs.Response;
using backend.Exceptions;
using backend.Models;
using backend.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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


        public async Task<bool> RegisterUser(UserRegisterRequest request) {

            var user = new User() {
                UserName = request.UserName,
                Email = request.EmailAddress
            };

            var userNameExists = await _userManager.FindByNameAsync(user.UserName);
            var emailExists = await _userManager.FindByEmailAsync(user.Email);

            if (userNameExists != null) {
                throw new BadRequestException("Username already exists.");
            }

            if (emailExists != null) {
                throw new BadRequestException("Email address already exists.");
            }

            var result = await _userManager.CreateAsync(user, request.Password);

            //if (!result.Succeeded) {
            //    throw new BadRequestException("Please enter all fields");
            //}

            var userRole = UserRoles.USER.ToString();
            var adminRole = UserRoles.ADMIN.ToString();

            await _roleService.CreateRoleIfNotExists(userRole);
            await _roleService.CreateRoleIfNotExists(adminRole);


            await _userManager.AddToRolesAsync(user, new string[] { userRole, adminRole });
            return true;
        }


        public async Task<UserLoginResponse> Login(UserLoginRequest request) {
            // Get user from DB
            var user = await _userManager.FindByNameAsync(request.UserName);

            // Valid user
            var success = await _userManager.CheckPasswordAsync(user, request.Password);

            if (success) {
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
