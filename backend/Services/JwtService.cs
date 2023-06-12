using backend.Models;
using SuperHeroApi.Dtos.Request;
using SuperHeroApi.Dtos.Response;

namespace backend.Services {
    public interface JwtService {
        Task<UserLoginResponseDto> GenerateJwtToken(User user);
        Task<UserLoginResponseDto> ValidateAndGenerateNewTokens(TokenRequest request);
        Task<bool> RevokeToken(string username);
    }
}
