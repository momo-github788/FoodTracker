using backend.Models;

namespace backend.Repository {
    public interface ConfirmationTokenRepository : IRepository<ConfirmationToken, int> {

        Task<ConfirmationToken> GetByConfirmationToken(string confirmationToken);
    }
}
