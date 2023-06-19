using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Request {
    public class ResetPasswordRequest {
    
        public string PasswordResetToken { get; set; } = string.Empty;
        [Required, MinLength(8, ErrorMessage = "Please enter at least 6 characters, dude!")]
        public string Password { get; set; } = string.Empty;
        [Required, Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string EmailAddress { get; set; }
    }
}
