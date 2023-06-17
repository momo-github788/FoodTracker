using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Data;
using backend.Exceptions;
using backend.Models;
using backend.Utils;
using Microsoft.EntityFrameworkCore;
using backend.DTOs.Response;
using backend.DTOs.Request;

namespace backend.Services.impl {
    public class JwtServiceImpl : JwtService{

        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;


        public JwtServiceImpl(ApplicationDbContext context, IConfiguration configuration, UserManager<User> userManager, TokenValidationParameters tokenValidationParameters) {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
        }


        public async Task<UserLoginResponse> GenerateJwtToken(User user) {

            IEnumerable<Claim> claims = await GetAllValidClaims(user);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));

            // Generate an Access Token that expires in 30 minutes
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration.GetSection("Jwt:TokenValidityInMinutes").Value)),
                Issuer = _configuration.GetSection("Jwt:Issuer").Value,
                Audience = _configuration.GetSection("Jwt:Audience").Value,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var accessToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(tokenDescriptor);



            // Generate a Refresh Token
            var refreshToken = new RefreshToken {
                // jti GUID
                JwtId = accessToken.Id,
                Token = TokenUtils.GenerateRandomRefreshToken(),
                CreatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddDays(Convert.ToInt32(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Value)),
                IsRevoked = false,
                IsUsed = false,
                UserId = user.Id
            };

            await _context.RefreshToken.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new UserLoginResponse {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken.Token,
                Success = true

            };
        }




        private async Task<List<Claim>> GetAllValidClaims(User user) {

            // Adding details to claims of Access Token
            List<Claim> claims = new List<Claim> {
                // Add the users Id to claim, used to validate and generate new Access + Refresh Token
                new Claim("UserId", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // CLAIMS
            // Getting the users claims if they have any additional claims
            var userClaims = await _userManager.GetClaimsAsync(user);

            // Add list of user claims to claims list
            claims.AddRange(userClaims);

            // ROLES
            // Get the users roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Add list of user roles to claims list
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }


        public async Task<bool> RevokeToken(string token) {

            var refreshToken = await _context.RefreshToken.FirstOrDefaultAsync(item => item.Token == token);

            if (refreshToken == null) {
                Console.WriteLine("token " + token + " not found");
                return false;
            }
             

            Console.Write("Found refresh token: " + token);

            refreshToken.IsRevoked = true;

            _context.RefreshToken.Update(refreshToken);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserLoginResponse> ValidateAndGenerateNewTokens(TokenRequest request) {

            // Accepts Access token to validate claims (Check security algorithm match etc)
            var claimsPrincipal = GetAndValidateClaimsFromExpiredAccessToken(request.AccessToken);

            //  VALIDATION 1 : Check if token matches security algorithm is was created with
            if(claimsPrincipal == null) {
                throw new BadRequestException("Invalid access token or refresh token");
            }

            // Find Refresh Token that exists in DB
            var storedRefreshToken = await _context.RefreshToken.FirstOrDefaultAsync(token => token.Token == request.RefreshToken);

            //  VALIDATION 2 : Check is Refresh Token from database still exists  and matches the request
            if(storedRefreshToken == null) {
                throw new BadRequestException("Invalid access token or refresh token");
            }

            var user = await _userManager.FindByIdAsync(storedRefreshToken.UserId);

            //  VALIDATION 3 : Validate that user isn't OR Refresh token isn't expired OR Refresh token isn't revoked
            if(user == null || storedRefreshToken.ExpiredAt <= DateTime.Now || storedRefreshToken.IsRevoked || storedRefreshToken.IsUsed) {
                throw new BadRequestException("Invalid access token or refresh token");
            }


            var jti = claimsPrincipal.Claims.FirstOrDefault(token => token.Type == JwtRegisteredClaimNames.Jti).Value;

            //  VALIDATION 4 : Validate that the Refresh Tokens JTI matches the JTI from the users claims (Random Guid generated when Access Token is created)
            if(storedRefreshToken.JwtId != jti) {
                throw new BadRequestException("Invalid Token");
            }


            // If validations pass, generate new Access token and Refresh token to allow user to access authorized parts of system
            var tokens = await GenerateJwtToken(user);

            // If stored token exists, set IsUsed to True so the same tokens cannot be used to refresh again
            storedRefreshToken.IsUsed = true;

            // Save modified changes made to Refresh Token
            _context.RefreshToken.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            // Return response with new Access Token and Refresh Token
            return new UserLoginResponse() {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                Success = true
            };

        }


        private ClaimsPrincipal GetAndValidateClaimsFromExpiredAccessToken(string accessToken) {


            var tokenValidationParameters = new TokenValidationParameters {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value)),
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var tokenClaims = jwtTokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var validatedToken);

            if(validatedToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid security algorithm");

            return tokenClaims;

        }

    }

}
