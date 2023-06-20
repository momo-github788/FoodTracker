namespace backend.DTOs.Response {
    public class UserLoginResponse {
        public bool Success { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
