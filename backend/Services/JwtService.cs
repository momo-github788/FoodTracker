using backend.DTOs.Request;
using backend.DTOs.Response;
using backend.Models;


namespace backend.Services {
    public interface JwtService {
        Task<UserLoginResponse> GenerateJwtToken(User user);
        Task<UserLoginResponse> ValidateAndGenerateNewTokens(TokenRequest request);
        Task<bool> RevokeToken(string username);
    }
}
