using Asset_Management.Data;
using Asset_Management.Models;
using Asset_Management.Repositories.IRepositories;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Asset_Management.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<Roles> _roleManager;

        public UserRepository(ApplicationDbContext context, UserManager<Users> userManager, RoleManager<Roles> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            var userList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "No Role",
                    IsActive = user.LockoutEnd == null || user.LockoutEnd <= System.DateTimeOffset.Now
                });
            }

            return userList;
        }

        public async Task<UserViewModel> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            return new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? "No Role",
                //IsActive = user.LockoutEnd == null || user.LockoutEnd <= System.DateTimeOffset.Now
                IsActive = !user.LockoutEnabled
            };
        }

        public async Task<bool> ToggleUserStatusAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.LockoutEnd = user.LockoutEnd == null || user.LockoutEnd <= System.DateTimeOffset.Now
                ? System.DateTimeOffset.MaxValue
                : null;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUserAsync(Users user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        public async Task<bool> AddToRoleAsync(Users user, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return false;

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
