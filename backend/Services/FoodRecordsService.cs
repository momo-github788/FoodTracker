using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs.Request;
using backend.DTOs.Response;
using backend.Filter;
using backend.Models;
using backend.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services
{
    public interface FoodRecordsService {


        Task<ICollection<FoodRecord>> GetAll(string userId, PaginationAndFilterParams paginationFilter);
        Task<FoodRecordResponse> GetById(string userName, string id);
        Task<FoodRecordResponse> Create(string userName, CreateFoodRecordRequest requests);
        Task<FoodRecordResponse> Update(string userName, FoodRecord request);
        Task<ICollection<FoodRecord>> Delete(string userName, string id, PaginationAndFilterParams paginationFilter);
    }
}