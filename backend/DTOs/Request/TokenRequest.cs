using System.ComponentModel.DataAnnotations;

namespace SuperHeroApi.Dtos.Request {
    public class TokenRequest {

        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }

    }
}
