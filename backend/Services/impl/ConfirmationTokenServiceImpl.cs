using System.Net;
using System.Text;
using System.Web;
using backend.Exceptions;
using backend.Models;
using backend.Repository;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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

            Console.WriteLine(token);
            var encodedToken = HttpUtility.UrlEncode(token);
            Console.WriteLine(encodedToken);

            var emailConfirmationToken = new ConfirmationToken {
                Token = encodedToken,
                UserId = user.Id,
                isUsed = true
            };

            if(emailConfirmationToken != null) {
                // Save token generated to DB
                await _unitOfWork.ConfirmationTokens.Add(emailConfirmationToken);
                int result = _unitOfWork.Save();

                if(result < 1) {
                    throw new BadRequestException("Error saving confirmation token to Database");
                }
        
                return emailConfirmationToken;
            }
            return null;
       

        }

        public async Task<ConfirmationToken> GenerateNewConfirmationToken(string oldTokenRequest) {
            if (oldTokenRequest == null) { 
                throw new BadRequestException("Token not found");
            }

            var oldToken  = await GetConfirmationToken(oldTokenRequest);
            
            if (oldToken.Token == null || oldToken.UserId == null) {
                throw new BadRequestException("Token not exist");
            }

            Console.WriteLine("found old token" + oldToken);

            var user = await _userManager.FindByIdAsync(oldToken.UserId);

            Console.WriteLine("found user : " + user.Id);


            if (user.EmailConfirmed) {
                throw new BadRequestException("Email has already been confirmed");

            }
         

            //// If date has not passed then we know token is still valid
            //if (oldToken.expiredAt > DateTime.Now) {
            //    throw new BadRequestException("Token has not expired yet! You can still confirm using old token");
            //}


        
            var newToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            Console.WriteLine("NEW TOKEN CREATED: " + newToken);
            //var confirmationToken = await _confirmationTokenService.GenerateConfirmationToken(user.Id);

            //var emailBody = $"Please confirm your email address <a href=\"#URL#\"> Click here</a>";

            //var callback_url = "https://localhost:7050" + _urlHelper.Action("ConfirmEmail", "Auth",
            //    new {
            //        userId = confirmationToken.UserId,
            //        confirmationToken = confirmationToken.Token
            //    });


            //var body = emailBody.Replace("#URL#",
            //    callback_url);

            //Console.WriteLine("callback: " + body);

            //var encodedToken = System.Text.Encodings.Web.HtmlEncoder.Default.Encode(newToken);


            // Delete old token
            _unitOfWork.ConfirmationTokens.Delete(oldToken);
      

            // Generate and save new token
            var emailConfirmationToken = new ConfirmationToken{
                Token = oldToken.Token,
                ResendToken = newToken,
                UserId = user.Id
            };


            await _unitOfWork.ConfirmationTokens.Add(emailConfirmationToken);
            Console.WriteLine("emailConfirmationToken: " + emailConfirmationToken);

            var result = _unitOfWork.Save();

            if(result < 1) {
                return null;
            }

            //await ConfirmToken(oldToken.UserId, oldToken.Token);

            Console.WriteLine("New token found: " + emailConfirmationToken);

            return emailConfirmationToken;
        }

        public async Task<ConfirmationToken> GetConfirmationToken(string token) {

            if (token == null) {
                throw new InvalidTokenException("Invalid token parameters");
            }

            var decodedToken = WebUtility.UrlDecode(token);
    
            var confirmationToken = await _unitOfWork.ConfirmationTokens.GetByConfirmationToken(decodedToken);
            if(confirmationToken == null) {
                Console.WriteLine("TOKEN NOT FOUND");
                throw new InvalidTokenException("Token not found");
            }

          

            Console.WriteLine("TOKEN WAS FOUND");
            return confirmationToken;
        }

        public async Task<bool> ConfirmToken(string userId, string token) {
            if (token == null) {
                throw new BadRequestException("Invalid Email confirmation Token");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) {
                throw new BadRequestException("Invalid Email parameters");
            }

            if (user.EmailConfirmed) {
                throw new BadRequestException("Your email address has already been verified");
            }
            
            var decodedToken = HttpUtility.UrlDecode(token);
            Console.WriteLine("ConfirmToken decoded: " + decodedToken);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            return result.Succeeded;
        }

    }
}
