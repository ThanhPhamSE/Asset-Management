using Asset_Management.Data;
using Asset_Management.Models;
using Asset_Management.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Asset_Management.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public StatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            return await _context.Statuses.ToListAsync();
        }
    }
}
