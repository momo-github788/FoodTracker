using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class FoodRecordsController : ControllerBase {

        private readonly IFoodRecordsService _foodRecordsService;

        public FoodRecordsController(IFoodRecordsService foodRecordsService) {
            _foodRecordsService = foodRecordsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(FoodRecord foodRecord) {
            return Ok(await _foodRecordsService.Create(foodRecord));
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<FoodRecord>>> GetAll() {
            return Ok(await _foodRecordsService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ICollection<FoodRecord>>> GetById(string id) {
            return Ok(await _foodRecordsService.GetById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(FoodRecord foodRecord, string id) {
            return Ok(await _foodRecordsService.Update(foodRecord, id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) {
            return Ok(await _foodRecordsService.Delete(id));
        }
    }
}