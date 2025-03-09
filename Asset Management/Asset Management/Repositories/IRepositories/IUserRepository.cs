using Asset_Management.Models;
using Asset_Management.ViewModels;

namespace Asset_Management.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
        Task<UserViewModel> GetUserByIdAsync(string userId);
        Task<bool> ToggleUserStatusAsync(string userId);
        Task<bool> AddToRoleAsync(Users user, string roleId);
        Task<bool> AddUserAsync(Users user, string password);
        Task<bool> DeleteUserAsync(string userId);
    }
}
