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
        Task<ICollection<FoodRecord>> GetAll(PaginationAndFilterParams paginationFilter);
        Task<FoodRecordResponse> GetById(string id);
        Task<FoodRecordResponse> Create(CreateFoodRecordRequest requests);
        Task<FoodRecordResponse> Update(FoodRecord request, string id);
        Task<ICollection<FoodRecord>> Delete(string id, PaginationAndFilterParams paginationFilter);
    }
}