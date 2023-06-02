using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs.Request;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services {
    public interface IFoodRecordsService {
        Task<ICollection<FoodRecord>> GetAll();
        Task<FoodRecord> GetById(string id);
        Task<FoodRecord> Create(CreateFoodRecordRequest requests);
        Task<FoodRecord> Update(FoodRecord request, string id);
        Task<ICollection<FoodRecord>> Delete(string id);
    }
}