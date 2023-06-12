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

        public FoodRecordsController(FoodRecordsService foodRecordsService) {
            _foodRecordsService = foodRecordsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFoodRecordRequest request) {
            if(!ModelState.IsValid) {
                return BadRequest(new ApiResponse<FoodRecordResponse> {
                    Message = "Please enter all fields appropriately.",
                    Succeeded = false
                });
            }
            return Ok(new ApiResponse<FoodRecordResponse> {
                Data = await _foodRecordsService.Create(request),
                Message = "Food Record created successfully.",
                Succeeded = true
            });
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
        public async Task<ActionResult> GetById(string id) {
            return Ok(
                new ApiResponse<FoodRecordResponse>() {
                    Data = await _foodRecordsService.GetById(id),
                    Succeeded = true,
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(FoodRecord request) {

            var result = await _foodRecordsService.Update(request);

            if (result == null) {
                return BadRequest(new ApiResponse<FoodRecordResponse> {
                    Message = "Food record does not exist",
                    Errors = new string[] { "Food Record with Id " + request.Id + " does not exist" },
                    Succeeded = false
                });
            }
            return Ok(new ApiResponse<FoodRecordResponse> {
                Data = result,
                Message = "Food Record updated successfully.",
                Succeeded = true
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, [FromQuery] PaginationAndFilterParams filter) {

            var result = await _foodRecordsService.Delete(id, filter);

            if(result == null) {
                return BadRequest(new ApiResponse<FoodRecordResponse> {
                    Message = "Food record does not exist",
                    Errors = new string[] { "Food Record with Id " + id + " does not exist" },
                    Succeeded = false
                });
            }

            return await GetAll(filter);
        }
    }
}