using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Policies {
    public class PasswordPolicy : PasswordValidator<User> {
        public override async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password) {
            IdentityResult result = await base.ValidateAsync(manager, user, password);
            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

            if (password.ToLower().Contains(user.UserName.ToLower())) {
                errors.Add(new IdentityError {
                    Description = "Password cannot contain username"
                });
            }
            if (password.Contains("123")) {
                errors.Add(new IdentityError {
                    Description = "Password cannot contain 123 numeric sequence"
                });
            }

            if(password.Length < 8) {
                errors.Add(new IdentityError {
                    Description = "Password must be atleast 8 characters"
                });
            }
            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
