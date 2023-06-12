using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperHeroApi.Auth;
using SuperHeroApi.Controllers;
using SuperHeroApi.Exceptions;
using SuperHeroApi.Models;
using System.Xml.Linq;
using backend.Data;
using backend.Models;
using backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace SuperHeroApi.Services {
    public class RoleService : IRoleService {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(IUnitOfWork unitOfWork, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<bool> CreateRoleIfNotExists(string name) {
            // Check if role exists in Identity DB
            var roleExist = await _roleManager.RoleExistsAsync(name);

            // Check if role doesn't exist
            if (!roleExist) {

                var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));
                // Check if role was added successfully
                if (roleResult.Succeeded) {
                    return true;
                }
            }
            return false;
                
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRoles() {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }

        public async Task<IEnumerable<User>> GetAllUsers() {
            var users = await _userManager.Users.ToListAsync();

            return users;
        }

        public async Task<bool> AddUserToRole(string email, string roleName)
        {
            // Check if user exist
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) {
                throw new BadRequestException("User does not exist.");
            }

            // Check if role exist
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist) {
                throw new BadRequestException("Role does not exist.");
            }

            // Assign role to user 
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded) {
                return true;
            } else {
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetUserRoles(string email) {
            // Check if user exist
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) {
                throw new BadRequestException("User does not exist.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return userRoles;
        }

        public async Task<bool> RemoveRoleFromUser(string email, string roleName) {
            // Check if user exist
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) {
                throw new BadRequestException("User does not exist.");
            }

            // Check if role exist
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist) {
                throw new BadRequestException("Role does not exist.");
            }


            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded) {
                return true;
            }

            return false;
        }
    }
}
