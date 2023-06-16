using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Request {
    public class TokenRequest {

        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }

    }
}
