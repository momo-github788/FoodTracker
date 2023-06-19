using backend.Models;

namespace backend.Services {
    public interface ConfirmationTokenService {

        Task<bool> ConfirmToken(string userId, string token);
        Task<ConfirmationToken> GenerateConfirmationToken(string userId);
        Task<ConfirmationToken> GetConfirmationToken(string token);
        Task<ConfirmationToken> GenerateNewConfirmationToken(string oldToken);
    }
}
