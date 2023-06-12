using SuperHeroApi.Dtos.Request;
using SuperHeroApi.Dtos.Response;

namespace backend.Services {
    public interface UserService {
        Task<bool> RegisterUser(UserRegisterRequestDto request);
        //Task<bool> RegisterAdmin(UserRegisterRequestDto request);
        Task<UserLoginResponseDto> Login(UserLoginRequestDto request);
    }
}
