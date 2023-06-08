using backend.Data;
using backend.DTOs.Request;
using backend.DTOs.Response;
using backend.Filter;
using backend.Models;
using backend.Services;
using backend.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class FoodRecordsController : ControllerBase {

        private readonly FoodRecordsService _foodRecordsService;
        private readonly ApplicationDbContext _context;

        public FoodRecordsController(FoodRecordsService foodRecordsService, ApplicationDbContext context) {
            _foodRecordsService = foodRecordsService;
            _context = context; 
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFoodRecordRequest request) {
            if(!ModelState.IsValid) {
                return BadRequest(new {
                    message = "Please fill in all the fields"
                });
            }
            return Ok(await _foodRecordsService.Create(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationAndFilterParams filter) {

            PaginationAndFilterParams validFilter = new PaginationAndFilterParams(
                filter.PageNumber, filter.PageSize, filter.FoodCategory, filter.Name, filter.SortDir, filter.SortBy
            );


            var foodRecords = await _foodRecordsService.GetAll(filter);
            var totalRecords = foodRecords.Count();

            return Ok(new PaginationResponse<ICollection<FoodRecord>>(foodRecords, validFilter.PageNumber, validFilter.PageSize, totalRecords));

        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            return Ok(
                new ApiResponse<FoodRecordResponse>() {
                    Data = await _foodRecordsService.GetById(id),
                    Succeeded = true,
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(FoodRecord request, string id) {
            return Ok(await _foodRecordsService.Update(request, id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, [FromQuery] PaginationAndFilterParams filter) {

            await _foodRecordsService.Delete(id, filter);

            return await GetAll(filter);
        }
    }
}