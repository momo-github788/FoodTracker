using backend.DTOs.Request;
using backend.DTOs.Response;
using backend.Models;


namespace backend.Services {
    public interface JwtService {
        Task<UserLoginResponseDto> GenerateJwtToken(User user);
        Task<UserLoginResponseDto> ValidateAndGenerateNewTokens(TokenRequest request);
        Task<bool> RevokeToken(string username);
    }
}
