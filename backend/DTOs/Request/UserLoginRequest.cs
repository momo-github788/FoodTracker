using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Request {
    public class UserLoginRequest {

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
