using Asset_Management.Models;
using Asset_Management.ViewModels;

namespace Asset_Management.Services.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
        Task<UserViewModel> GetUserByIdAsync(string userId);
        Task<bool> ToggleUserStatusAsync(string userId);
        Task<List<RolesViewModel>> GetRolesAsync();
        Task<bool> AddUserAsync(UserViewModel model);
        Task<bool> DeleteUserAsync(string userId);
        Task<ProfileViewModel> GetProfileAsync(string userId);
        Task<bool> UpdateProfileAsync(string userId, ProfileViewModel model);
    }
}
