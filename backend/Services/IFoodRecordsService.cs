using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services {
    public interface IFoodRecordsService {
        Task<ICollection<FoodRecord>> GetAll();
        Task<FoodRecord> GetById(string id);
        Task<FoodRecord> Create(FoodRecord foodRecord);
        Task<FoodRecord> Update(FoodRecord model, string id);
        Task<bool> Delete(string id);
    }
}