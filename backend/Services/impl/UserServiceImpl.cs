using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Models;
using backend.Repository;
using backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using SuperHeroApi.Auth;
using SuperHeroApi.Dtos.Request;
using SuperHeroApi.Dtos.Response;
using SuperHeroApi.Exceptions;
using SuperHeroApi.Models;
using SuperHeroApi.Utils;

namespace SuperHeroApi.Services
{
    public class UserServiceImpl : UserService
    {

        private readonly JwtService _jwtService;
        private readonly RoleService _roleService;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public UserServiceImpl(JwtService jwtService, TokenValidationParameters tokenValidationParameters,
            IUnitOfWork unitOfWork, RoleService roleService, UserManager<User> userManager) {
            _roleService = roleService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _tokenValidationParameters = tokenValidationParameters;

        }


        public async Task<bool> RegisterUser(UserRegisterRequestDto request) {

            var user = new User() {
                UserName = request.UserName,
                Email = request.EmailAddress
            };

            var userNameExists = await _userManager.FindByNameAsync(user.UserName);
            var emailExists = await _userManager.FindByEmailAsync(user.Email);

            if(userNameExists!=null) {
                throw new BadRequestException("Username already exists.");
            }

            if (emailExists != null) {
                throw new BadRequestException("Email address already exists.");
            }

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded) {
                var userRole = UserRoles.User.ToString();
                var adminRole = UserRoles.Admin.ToString();

                await _roleService.CreateRoleIfNotExists(userRole);
                await _roleService.CreateRoleIfNotExists(adminRole);


                await _userManager.AddToRolesAsync(applicationUser, new string[]{userRole, adminRole});
                return true;
            }

            return false;
        }


        public async Task<UserLoginResponseDto> Login(UserLoginRequestDto request) {
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
   
    }
}
