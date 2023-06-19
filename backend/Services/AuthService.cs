using backend.DTOs.Request;
using backend.DTOs.Response;
using System.Security.Policy;
using backend.Models;

namespace backend.Services {
    public interface AuthService {
        Task<string> ForgotPassword(string emailAddress);
        Task<bool> ResetPassword(ResetPasswordRequest request);
        Task<User> RegisterUser(UserRegisterRequest request);
        //Task<bool> RegisterAdmin(UserRegisterRequestDto request);
        Task<UserLoginResponse> GoogleLogin(UserLoginRequest request, Url responseUrl);
        Task<UserLoginResponse> Login(UserLoginRequest request);
    }
}
