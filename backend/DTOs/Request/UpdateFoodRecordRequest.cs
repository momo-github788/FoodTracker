using backend.Models;

namespace backend.DTOs.Request {
    public class UpdateFoodRecordRequest {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime DateTime { get; set; }
        public string FoodCategory { get; set; }
    }
}
