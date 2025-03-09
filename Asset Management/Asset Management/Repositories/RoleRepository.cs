using Asset_Management.Data;
using Asset_Management.Repositories.IRepositories;
using Asset_Management.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Asset_Management.Repositories
{
    public class RoleRepository: IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RolesViewModel>> GetRolesAsync()
        {
            return await _context.Roles
                .Select(r => new RolesViewModel { Id = r.Id, Name = r.Name })
                .ToListAsync();
        }
    }
}
