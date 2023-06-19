using backend.Models;

namespace backend.Services {
    public interface ConfirmationTokenService {

        Task<bool> ConfirmToken(string userId, string confirmationToken);
        Task<ConfirmationToken> GenerateConfirmationToken(string userId);
        Task<ConfirmationToken> GetConfirmationToken(string confirmationToken);
        Task<ConfirmationToken> GenerateNewConfirmationToken(string oldConfirmationToken);
    }
}
