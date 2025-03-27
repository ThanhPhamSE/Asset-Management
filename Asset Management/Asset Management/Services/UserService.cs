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
        private readonly UserManager<Users> _userManager;

        public UserService(IUserRepository userRepository, RoleManager<Roles> roleManager, IRoleRepository roleRepository, UserManager<Users> userManager)
        {
            _userRepository = userRepository;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _userManager = userManager;
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

        public async Task<ProfileViewModel> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName
            };
        }

        public async Task<bool> UpdateProfileAsync(string userId, ProfileViewModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            // Cập nhật thông tin cá nhân
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;
            var updateResult = await _userManager.UpdateAsync(user);

            // Nếu có đổi mật khẩu
            if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                var passwordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                return updateResult.Succeeded && passwordResult.Succeeded;
            }

            return updateResult.Succeeded;
        }
    }

}



