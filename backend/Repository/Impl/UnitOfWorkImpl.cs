using backend.Data;

namespace backend.Repository.Impl {
    public class UnitOfWorkImpl : IUnitOfWork{ 
        private readonly ApplicationDbContext _context;
        public FoodRecordRepository FoodRecords { get; }
 
        public UnitOfWorkImpl(ApplicationDbContext context, FoodRecordRepository foodRecordsRecordRepository) {
            _context = context;
            FoodRecords = foodRecordsRecordRepository;
        }



        public void Dispose() {
            _context.Dispose();
        }

        public int Save() {
            return _context.SaveChanges();
        }
    }
}
