using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository.Impl {
    public class ConfirmationTokenRepositoryImpl : RepositoryImpl<ConfirmationToken>, ConfirmationTokenRepository{
        public ConfirmationTokenRepositoryImpl(ApplicationDbContext context) : base(context) {

        }
    }
}
