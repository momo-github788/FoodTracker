using backend.DTOs.Request;
using backend.DTOs.Response;

namespace backend.Services {
    public interface UserService {
        Task<bool> RegisterUser(UserRegisterRequestDto request);
        //Task<bool> RegisterAdmin(UserRegisterRequestDto request);
        Task<UserLoginResponseDto> Login(UserLoginRequestDto request);
    }
}
