using backend.DTOs.Request;
using backend.DTOs.Response;

namespace backend.Services {
    public interface UserService {

        //Task<UserDetailsResponse> GetUserDetailsById(string userId);
        Task<bool> RegisterUser(UserRegisterRequest request);
        //Task<bool> RegisterAdmin(UserRegisterRequestDto request);
        Task<UserLoginResponse> Login(UserLoginRequest request);
    }
}
