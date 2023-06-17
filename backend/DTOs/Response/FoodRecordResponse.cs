using backend.Models;
using System.Text.Json.Serialization;

namespace backend.DTOs.Response {
    public class FoodRecordResponse {

        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime DateTime { get; set; }
        public string FoodCategory { get; set; }

        public string? UserId { get; set; }

    }
}
