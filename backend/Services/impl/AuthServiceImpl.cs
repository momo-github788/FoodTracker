using backend.DTOs.Request;
using backend.DTOs.Response;
using backend.Exceptions;
using backend.Models;
using backend.Repository;
using Lombok.NET;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Policy;

namespace backend.Services.impl {

    public class AuthServiceImpl : AuthService{

        private readonly IUrlHelper _urlHelper;
        private readonly ConfirmationTokenService _confirmationTokenService;
        private readonly JwtService _jwtService;
        private readonly RoleService _roleService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthServiceImpl(IUrlHelper urlHelper, ConfirmationTokenService confirmationTokenService, JwtService jwtService, RoleService roleService, SignInManager<User> signInManager, UserManager<User> userManager, IUnitOfWork unitOfWork, EmailService emailService, TokenValidationParameters tokenValidationParameters) {
            _urlHelper = urlHelper;
            _confirmationTokenService = confirmationTokenService;
            _jwtService = jwtService;
            _roleService = roleService;
            _signInManager = signInManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
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
                var confirmationToken = await _confirmationTokenService.GenerateConfirmationToken(user.Id);

                Console.WriteLine("token created: " + confirmationToken);
                var emailBody = $"Please confirm your email address <a href=\"#URL#\"> Click here</a>";

                var callback_url = "https://localhost:7050" + _urlHelper.Action("ConfirmEmail", "Auth",
                 new {
                     userId = confirmationToken.UserId,
                     confirmationToken = confirmationToken.EmailConfirmationToken
                 });

                Console.WriteLine("callback: " + callback_url);


                var body = emailBody.Replace("#URL#",
                    System.Text.Encodings.Web.HtmlEncoder.Default.Encode(callback_url));

                _emailService.sendEmail("axel.nienow@ethereal.email", "Email Verification", body, user.Email);



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
