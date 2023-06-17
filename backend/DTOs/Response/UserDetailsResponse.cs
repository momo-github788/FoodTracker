namespace backend.DTOs.Response {
    public class UserDetailsResponse {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public ICollection<FoodRecordResponse> FoodRecords { get; set; }


    }
}
