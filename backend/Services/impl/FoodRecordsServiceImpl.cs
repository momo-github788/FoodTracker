using backend.Data;
using backend.DTOs.Request;
using backend.DTOs.Response;
using backend.Filter;
using backend.Models;
using backend.Wrappers;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace backend.Services.impl
{
    public class FoodRecordsServiceImpl : Services.FoodRecordsService
    {

        private readonly ApplicationDbContext _context;

        public FoodRecordsServiceImpl(ApplicationDbContext context)
        {
            _context = context;
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

            _context.FoodRecords.Add(foodRecord);
            await _context.SaveChangesAsync();

            return ConvertFoodRecordRequestToResponseDTO(foodRecord);



        }

        public async Task<ICollection<FoodRecord>> Delete(string id, PaginationAndFilterParams filter)
        {
            var foodRecord = await _context.FoodRecords.FindAsync(id);

            if (foodRecord == null)
            {
                throw new Exception("Food doesn't exist");
            }

            _context.FoodRecords.Remove(foodRecord);

            await _context.SaveChangesAsync();
            return await GetAll(filter);

        }

        public async Task<ICollection<FoodRecord>> GetAll(PaginationAndFilterParams filter)
        {

            var query = filter.SearchQuery;
            var sortDir = filter.SortDir;
            var sortBy = filter.SortBy;
            var name = filter.Name;
            var foodCategory = filter.FoodCategory;

            IQueryable<FoodRecord> foodRecords = _context.FoodRecords;
            // var foodRecords = _context.FoodRecords.AsQueryable();


            // Sorting
            // Defaults to sorting by newly created Food Record
            if (string.IsNullOrEmpty(sortBy)) {
                foodRecords = foodRecords.OrderByDescending(c => c.DateTime);
            }

            switch (sortBy) {
                case "name":
                    foodRecords = sortDir == "asc" ? foodRecords.OrderBy(c => c.Name) : foodRecords.OrderByDescending(c => c.Name);
                    break;
                case "foodcategory":
                    foodRecords = sortDir == "asc" ? foodRecords.OrderBy(c => c.FoodCategory) : foodRecords.OrderByDescending(c => c.FoodCategory);
                    break;
            }
            

            // Filtering
            if (!string.IsNullOrEmpty(foodCategory)) {
                foodRecords = foodRecords.Where(item => item.FoodCategory == filter.FoodCategory);
            }

            if (!string.IsNullOrEmpty(name)) {
                foodRecords = foodRecords.Where(item => item.Name == name);
            } 

            // Searching
            if (!string.IsNullOrEmpty(query)) {
                foodRecords = foodRecords.Where(item => item.Name.Contains(query) || item.FoodCategory.Contains(query));
            }

            // Pagination
            var foodRecordsPaged = await foodRecords
                //.Select(item => ConvertFoodRecordRequestToResponseDTO(item))
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return foodRecordsPaged;
            
        }

        public async Task<FoodRecordResponse> GetById(string id)
        {
            var foodRecord = await _context.FoodRecords.FindAsync(id);

            if (foodRecord == null)
            {
                throw new Exception("Food doesn't exist");
            }

            return ConvertFoodRecordRequestToResponseDTO(foodRecord);

        }

        //public async Task<ICollection<FoodRecord>> SearchFoodRecords(string query)
        //{

        //    var foodRecords = await _context.FoodRecords.ToListAsync();



        //    var filteredFoodRecords = foodRecords.Where(
        //        item => item.Name.Contains(query) || item.FoodCategory.Contains(query)
        //    );


        //    return filteredFoodRecords.ToList();
        //}

        public async Task<FoodRecordResponse> Update(FoodRecord request, string id)
        {
            var exists = await _context.FoodRecords.AnyAsync(f => f.Id == id);
            if (!exists)
            {
                return null;
            }

            _context.FoodRecords.Update(request);

            await _context.SaveChangesAsync();

            return ConvertFoodRecordRequestToResponseDTO(request);
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
