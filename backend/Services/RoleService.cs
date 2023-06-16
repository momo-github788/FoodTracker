using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Services  {
    public interface RoleService {

        Task<bool> CreateRoleIfNotExists(string name);
        Task<IEnumerable<IdentityRole>> GetAllRoles();
        Task<IEnumerable<User>> GetAllUsers();
        Task<bool> AddUserToRole(string email, string roleName);
        Task<IEnumerable<string>> GetUserRoles(string email);
        Task<bool> RemoveRoleFromUser(string email, string roleName);

    }
}
