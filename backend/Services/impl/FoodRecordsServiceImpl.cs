using backend.Data;
using backend.DTOs.Request;
using backend.DTOs.Response;
using backend.Filter;
using backend.Models;
using backend.Repository;
using backend.Repository.Impl;
using backend.Wrappers;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace backend.Services.impl
{
    public class FoodRecordsServiceImpl : Services.FoodRecordsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodRecordsServiceImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FoodRecordResponse> Create(CreateFoodRecordRequest request)
        {
            var foodRecord = new FoodRecord
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Value = request.Value,
                DateTime = DateTime.Now,
                FoodCategory = request.FoodCategory
            };


            await _unitOfWork.FoodRecords.Add(foodRecord);

            var result =  _unitOfWork.Save();

            if (result < 1) {
                return null;
            }
            return ConvertFoodRecordRequestToResponseDTO(foodRecord);

        }

        public async Task<FoodRecordResponse> GetById(string id) {
            var foodRecord = await _unitOfWork.FoodRecords.GetById(id);

            if(foodRecord == null) {
                throw new Exception("Food doesn't exist");
            }

            return ConvertFoodRecordRequestToResponseDTO(foodRecord);

        }




        public async Task<ICollection<FoodRecord>> GetAll(PaginationAndFilterParams filter) {

            var query = filter.SearchQuery;
            var sortDir = filter.SortDir;
            var sortBy = filter.SortBy;
            var name = filter.Name;
            var foodCategory = filter.FoodCategory;

            IEnumerable<FoodRecord> foodRecords = await _unitOfWork.FoodRecords.GetAll();
            // var foodRecords = _context.FoodRecords.AsQueryable();


            // Sorting
            // Defaults to sorting by newly created Food Record
            if(string.IsNullOrEmpty(sortBy)) {
                foodRecords = foodRecords.OrderByDescending(c => c.DateTime);
            }

            switch(sortBy) {
                case "name":
                    foodRecords = sortDir == "asc" ? foodRecords.OrderBy(c => c.Name) : foodRecords.OrderByDescending(c => c.Name);
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
                //.Select(item => ConvertFoodRecordRequestToResponseDTO(item))
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return foodRecordsPaged;

        }

        public async Task<ICollection<FoodRecord>> Delete(string id, PaginationAndFilterParams filter) {
            var foodRecord = await _unitOfWork.FoodRecords.GetById(id);

            if(foodRecord == null) {
                return null;
            }

            _unitOfWork.FoodRecords.Delete(foodRecord);
            var result = _unitOfWork.Save();

            if (result < 1) {
                return null;
            }
            return await GetAll(filter);

        }


        public async Task<FoodRecordResponse> Update(FoodRecord request) {
            var foodRecord = await _unitOfWork.FoodRecords.GetById(request.Id);

            if(foodRecord != null) {
                Console.WriteLine("found record with id:" + request.Id);
                foodRecord.Name = request.Name;
                foodRecord.Value = request.Value;
                foodRecord.FoodCategory = request.FoodCategory;

                _unitOfWork.FoodRecords.Update(foodRecord);

                var result = _unitOfWork.Save();
                Console.WriteLine("result:" + result);
                if(result > 0) {
                    return ConvertFoodRecordRequestToResponseDTO(foodRecord);
                }

            }

            Console.WriteLine("Cant find record with id: " + request.Id);
    

            return null;
        }

        private FoodRecordResponse ConvertFoodRecordRequestToResponseDTO(FoodRecord foodRecord) =>
            new FoodRecordResponse {
                Id = foodRecord.Id,
                Name = foodRecord.Name,
                Value = foodRecord.Value,
                DateTime = foodRecord.DateTime,
                FoodCategory = foodRecord.FoodCategory
            };


    }
}
