using backend.Data;
using backend.DTOs.Request;
using backend.DTOs.Response;
using backend.Exceptions;
using backend.Filter;
using backend.Models;
using backend.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.impl
{
    public class FoodRecordsServiceImpl : FoodRecordsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public FoodRecordsServiceImpl(IUnitOfWork unitOfWork, UserManager<User> userManager) {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<FoodRecordResponse> Create(string userName, CreateFoodRecordRequest request) {

            var user = await _userManager.FindByNameAsync(userName);
            var foodRecord = new FoodRecord {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Value = request.Value,
                DateTime = DateTime.Now,
                FoodCategory = request.FoodCategory,
                UserId = user.Id,
                User = user
            };

            await _unitOfWork.FoodRecords.Add(foodRecord);

            var result = _unitOfWork.Save();

            if (result < 1) {
                return null;
            }
            return ConvertFoodRecordRequestToResponseDTO(foodRecord);
        }

        public async Task<FoodRecordResponse> GetById(string userName, string id) {
            var user = await _userManager.FindByNameAsync(userName);

            var foodRecord = await _unitOfWork.FoodRecords.Get(id);

            if(foodRecord == null) {
                throw new Exception("Food doesn't exist");
            }

            return ConvertFoodRecordRequestToResponseDTO(foodRecord);

        }


        public async Task<ICollection<FoodRecordResponse>> GetAll(string userId, PaginationAndFilterParams filter) {

            var query = filter.SearchQuery;
            var sortDir = filter.SortDir;
            var sortBy = filter.SortBy;
            var name = filter.Name;
            var foodCategory = filter.FoodCategory;

            IEnumerable<FoodRecord> foodRecords = await _unitOfWork.FoodRecords.GetAll();
  

            // Sorting
            // Defaults to sorting by newly created Food Record
            if(string.IsNullOrEmpty(sortBy)) {
                foodRecords = foodRecords.OrderByDescending(c => c.DateTime);
            }

            switch(sortBy) {
                case "name":
                    foodRecords = sortDir == "asc" ? foodRecords.OrderBy(c => c.Name) 
                        : foodRecords.OrderByDescending(c => c.Name);
                    break;
                case "foodcategory":
                    foodRecords = sortDir == "asc" ? foodRecords.OrderBy(c => c.FoodCategory) : foodRecords.OrderByDescending(c => c.FoodCategory);
                    break;
            }


            // Filtering
            if(!string.IsNullOrEmpty(foodCategory)) {
                foodRecords = foodRecords.Where(item => item.FoodCategory == filter.FoodCategory);
            }

            if(!string.IsNullOrEmpty(name)) {
                foodRecords = foodRecords.Where(item => item.Name == name);
            }

            // Searching
            if(!string.IsNullOrEmpty(query)) {
                foodRecords = foodRecords.Where(item => item.Name.Contains(query) || item.FoodCategory.Contains(query));
            }

            // Pagination
            var foodRecordsPaged = foodRecords
                .Where(item => item.UserId == userId)
                .Select(item => ConvertFoodRecordRequestToResponseDTO(item))
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return foodRecordsPaged;

        }

        public async Task<ICollection<FoodRecordResponse>> Delete(string userName, string id, PaginationAndFilterParams filter) {
            var foodRecord = await _unitOfWork.FoodRecords.Get(id);

            if(foodRecord == null) {
                return null;
            }

            _unitOfWork.FoodRecords.Delete(foodRecord);
            var result = _unitOfWork.Save();

            if (result < 1) {
                return null;
            }
            return await GetAll(userName, filter);

        }


        public async Task<FoodRecordResponse> Update(UpdateFoodRecordRequest request) {
            var foodRecord = await _unitOfWork.FoodRecords.Get(request.Id);

            if(foodRecord == null) {
                throw new BadRequestException("Food Record not found");

            }

            Console.WriteLine("found record with id:" + request.Id);
            foodRecord.Name = request.Name;
            foodRecord.Value = request.Value;
            foodRecord.FoodCategory = request.FoodCategory;

            _unitOfWork.FoodRecords.Update(foodRecord);

            var result = _unitOfWork.Save();
            Console.WriteLine("result:" + result);
            if (result > 0) {
                return ConvertFoodRecordRequestToResponseDTO(foodRecord);
            }
            return null;
        }

        private FoodRecordResponse ConvertFoodRecordRequestToResponseDTO(FoodRecord foodRecord) =>
            new FoodRecordResponse {
                Id = foodRecord.Id,
                Name = foodRecord.Name,
                Value = foodRecord.Value,
                DateTime = foodRecord.DateTime,
                FoodCategory = foodRecord.FoodCategory,
                UserId = foodRecord.UserId,
                
            };


    }
}
