using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Request {
    public class UserRegisterRequest {

        [DisplayName("Username")]
        [Required(ErrorMessage = "Username is required")]
        [MinLength(6, ErrorMessage = "Minimum length of 6 characters is required")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "Email Address is required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Minimum length of 8 characters is required")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        [Required(ErrorMessage = "Confirm Password is required")]
        public string ConfirmPassword { get; set; }
    }
}
