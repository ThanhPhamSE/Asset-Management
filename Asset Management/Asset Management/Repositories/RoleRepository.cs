using Asset_Management.Data;
using Asset_Management.Models;
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
        public async Task<IEnumerable<Roles>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Roles> GetRoleByIdAsync(string id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task AddRoleAsync(Roles role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(Roles role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(string id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Roles?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper());
        }
    }
}
