using backend.Data;
using backend.Models;

namespace backend.Repository.Impl {
    public class UnitOfWorkImpl : IUnitOfWork{ 
        private readonly ApplicationDbContext _context;

        public FoodRecordRepository FoodRecords { get; }
        public ConfirmationTokenRepository ConfirmationTokens { get; }

        public UnitOfWorkImpl(ApplicationDbContext context, FoodRecordRepository FoodRecords, ConfirmationTokenRepository ConfirmationTokens) {
            _context = context;
            this.FoodRecords = FoodRecords;
            this.ConfirmationTokens = ConfirmationTokens;
        }

  

        public void Dispose() {
            _context.Dispose();
        }

        public int Save() {
            return _context.SaveChanges();
        }
    }
}
