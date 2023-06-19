using backend.Exceptions;
using backend.Models;
using backend.Repository;
using Microsoft.AspNetCore.Identity;

namespace backend.Services.impl {
    public class ConfirmationTokenServiceImpl : ConfirmationTokenService {

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmationTokenServiceImpl(SignInManager<User> signInManager, UserManager<User> userManager,
            IUnitOfWork unitOfWork
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<ConfirmationToken> GenerateConfirmationToken(string userId) {
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null) {
                throw new InvalidTokenException("User not found");
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            

            var confirmationToken = new ConfirmationToken {
                EmailConfirmationToken = token,
                UserId = user.Id
            };

            if(confirmationToken != null) {
                await _unitOfWork.ConfirmationTokens.Add(confirmationToken);
                int result = _unitOfWork.Save();

                if(result < 1) {
                    throw new BadRequestException("Error saving confirmation token to Database");
                }
        
                return confirmationToken;
            }
            return null;
       

        }

        public async Task<ConfirmationToken> GenerateNewConfirmationToken(string oldConfirmationToken) {
            if (oldConfirmationToken == null) { 
                throw new BadRequestException("Token not found");

            }

            Console.WriteLine("generastinr new token..");
            var oldToken = await GetConfirmationToken(oldConfirmationToken);
        

            if (oldToken == null) {
                throw new BadRequestException("Token not exist");
            }
            Console.WriteLine("found old token");
            var user = await _userManager.FindByIdAsync(oldToken.UserId);
            Console.WriteLine("USER: " + user.Name);

            Console.WriteLine("IS TOKEN VALID?: " + await ConfirmToken(oldToken.UserId, oldToken.EmailConfirmationToken));

            if (user.EmailConfirmed) {
                throw new BadRequestException("Email has already been confirmed");

            }
         

            //// If date has not passed then we know token is still valid
            //if (oldToken.expiredAt > DateTime.Now) {
            //    throw new BadRequestException("Token has not expired yet! You can still confirm using old token");
            //}

            Console.WriteLine("Old token found: " +oldToken);

            var newToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
           

            var confirmationToken = new ConfirmationToken{
                EmailConfirmationToken = oldConfirmationToken,
                ResendEmailConfirmationToken = newToken
            };
            Console.WriteLine("New token found: " + confirmationToken);

            return confirmationToken;
        }

        public async Task<ConfirmationToken> GetConfirmationToken(string confirmationToken) {
            var token = await _unitOfWork.ConfirmationTokens.GetByConfirmationToken(confirmationToken);

            Console.WriteLine("token: " + token);
            if(token == null) {
                Console.WriteLine("TOKEN NOT FOUND");
                throw new InvalidTokenException("Token not found");
            }

            return token;
        }

        public async Task<bool> ConfirmToken(string userId, string confirmationToken) {
            if (confirmationToken == null) {
                throw new BadRequestException("Invalid Email confirmation Token");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) {
                throw new BadRequestException("Invalid Email parameters");
            }

            if (user.EmailConfirmed) {
                throw new BadRequestException("Your email address has already been verified");
            }

            // Sets EmailVerified = true if valid (Using Identity)
            var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);

            return result.Succeeded;
        }

    }
}
