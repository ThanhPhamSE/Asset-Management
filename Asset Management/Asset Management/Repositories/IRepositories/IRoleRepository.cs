using Asset_Management.ViewModels;

namespace Asset_Management.Repositories.IRepositories
{
    public interface IRoleRepository
    {
        Task<List<RolesViewModel>> GetRolesAsync();
    }
}
