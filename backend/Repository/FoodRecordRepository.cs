using backend.Filter;
using backend.Models;

namespace backend.Repository {
    public interface FoodRecordRepository : IRepository<FoodRecord, string> {
        //Task<IEnumerable<FoodRecord>> GetAll();
    }
}
