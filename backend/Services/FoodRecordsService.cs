using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services {
    public class FoodRecordsService : IFoodRecordsService {

        private readonly ApplicationDbContext _context;

        public FoodRecordsService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<FoodRecord> Create(FoodRecord model) {
            var foodRecord = new FoodRecord {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Value = model.Value,
                DateTime = model.DateTime
            };

            _context.FoodRecords.Add(foodRecord);
            await _context.SaveChangesAsync();

            return foodRecord;



        }

        public async Task<bool> Delete(string id) {
            var foodRecord = await _context.FoodRecords.FindAsync(id);

            if(foodRecord == null) {
                throw new Exception("Food doesn't exist");
            }
            _context.FoodRecords.Remove(foodRecord);

            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<ICollection<FoodRecord>> GetAll() {
            return await _context.FoodRecords.ToListAsync();
        }

        public async Task<FoodRecord> GetById(string id) {
            var foodRecord = await _context.FoodRecords.FindAsync(id);

            if (foodRecord == null) {
                throw new Exception("Food doesn't exist");
            }

            return foodRecord;

        }

        public async Task<FoodRecord> Update(FoodRecord model, string id) {
            var exists = await _context.FoodRecords.AnyAsync(f => f.Id == id);
            if (!exists) {
                return null;
            }

            _context.FoodRecords.Update(model);

            await _context.SaveChangesAsync();

            return model;
        }
    }
}
