using Asset_Management.Models;

namespace Asset_Management.Services.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<Roles>> GetAllRolesAsync();
        Task<Roles> GetRoleByIdAsync(string id);
        Task AddRoleAsync(Roles role);
        Task UpdateRoleAsync(Roles role);
        Task DeleteRoleAsync(string id);
        Task<Roles?> GetRoleByNameAsync(string roleName);
    }
}
