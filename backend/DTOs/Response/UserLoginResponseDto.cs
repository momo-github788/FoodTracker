namespace SuperHeroApi.Dtos.Response {
    public class UserLoginResponseDto {
        public bool Success { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
