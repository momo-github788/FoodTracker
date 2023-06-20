using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository.Impl {
    public class ConfirmationTokenRepositoryImpl : RepositoryImpl<ConfirmationToken, int>, ConfirmationTokenRepository {

        private readonly ApplicationDbContext _context;

        
        public ConfirmationTokenRepositoryImpl(ApplicationDbContext context) : base(context) {
            _context = context;
        }

        public async Task<ConfirmationToken> GetByConfirmationToken(string confirmationToken) {
            return await _context.ConfirmationToken.
                Where(t => t.Token == confirmationToken || t.ResendToken == confirmationToken).SingleOrDefaultAsync();

            
        }
    }
}
