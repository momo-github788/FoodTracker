using backend.Data;
using backend.DTOs.Request;
using backend.DTOs.Response;
using backend.Filter;
using backend.Models;
using backend.Services;
using backend.Wrappers;
using Microsoft.AspNetCore.Mvc;
using backend.Utils;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FoodRecordsController : ControllerBase {

        private readonly IHttpContextAccessor _context;
        private readonly FoodRecordsService _foodRecordsService;

        public FoodRecordsController(FoodRecordsService foodRecordsService, IHttpContextAccessor context) {
            _foodRecordsService = foodRecordsService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFoodRecordRequest request) {
            if (!ModelState.IsValid) {
                return BadRequest(new ApiResponse<FoodRecordResponse> {
                    Message = "Please enter all fields appropriately.",
                    Succeeded = false
                });
            }

            //Console.WriteLine(_context.HttpContext?.User.FindFirstValue("userId"));

 
            var response = await _foodRecordsService.Create(AuthUtils.getPrincipal(_context), request);
       

            return Ok(new ApiResponse<FoodRecordResponse> {
                Data = response,
                Message = "Food Record created successfully.",
                Succeeded = true
            });
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationAndFilterParams filter) {

            PaginationAndFilterParams validFilter = new PaginationAndFilterParams(
                filter.PageNumber, filter.PageSize, filter.FoodCategory, filter.Name, filter.SortDir, filter.SortBy
            );


            var foodRecords = await _foodRecordsService.GetAll(AuthUtils.getUserId(_context), filter);
            var totalRecords = foodRecords.Count();

            return Ok(new PaginationResponse<ICollection<FoodRecordResponse>>(foodRecords, validFilter.PageNumber, validFilter.PageSize, totalRecords));

        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id) {
            return Ok(
                new ApiResponse<FoodRecordResponse>() {
                    Data = await _foodRecordsService.GetById(AuthUtils.getPrincipal(_context), id),
                    Succeeded = true,
                }
            );
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateFoodRecordRequest request) {

            var result = await _foodRecordsService.Update(request);

            return Ok(new ApiResponse<FoodRecordResponse> {
                Data = result,
                Message = "Food Record updated successfully.",
                Succeeded = true
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, [FromQuery] PaginationAndFilterParams filter) {

            var result = await _foodRecordsService.Delete("eee", id, filter);

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