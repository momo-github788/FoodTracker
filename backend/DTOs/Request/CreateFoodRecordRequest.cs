namespace backend.DTOs.Request {
    public class CreateFoodRecordRequest {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime DateTime { get; set; }
    }
}
