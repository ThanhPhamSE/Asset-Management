using Asset_Management.Models;
using Asset_Management.Repositories;
using Asset_Management.Repositories.IRepositories;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Graph.Models;

namespace Asset_Management.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<Roles> _roleManager;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, RoleManager<Roles> roleManager, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<UserViewModel> GetUserByIdAsync(string userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<bool> ToggleUserStatusAsync(string userId)
        {
            return await _userRepository.ToggleUserStatusAsync(userId);
        }

        // Lấy danh sách Roles
        public async Task<List<RolesViewModel>> GetRolesAsync()
        {
            return await _roleRepository.GetRolesAsync();
        }

        // Thêm User vào hệ thống
        //public async Task<bool> AddUserAsync(UserViewModel model)
        //{
        //    var user = new Users
        //    {
        //        UserName = model.UserName,
        //        FullName = model.FullName,
        //        Email = model.Email,
        //        LockoutEnabled = !model.IsActive
        //    };

        //    // Tạo tài khoản mới
        //    var result = await _userRepository.AddUserAsync(user, model.Password);
        //    if (!result) return false;

        //    // Thêm user vào vai trò
        //    return await _userRepository.AddToRoleAsync(user, model.RoleId);
        //}

        public async Task<bool> AddUserAsync(UserViewModel model)
        {
            var user = new Users
            {
                UserName = model.UserName,
                FullName = model.FullName,
                Email = model.Email,
                LockoutEnabled = !model.IsActive
            };

            //Console.WriteLine($"Before Saving - LockoutEnabled: {user.LockoutEnabled}");

            // Tạo tài khoản mới
            var result = await _userRepository.AddUserAsync(user, model.Password);
            if (!result) return false;

            // Kiểm tra lại sau khi lưu
            //Console.WriteLine($"Before Saving - LockoutEnabled: {user.LockoutEnabled}");

            return await _userRepository.AddToRoleAsync(user, model.RoleId);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            return await _userRepository.DeleteUserAsync(userId);
        }

    }

}



