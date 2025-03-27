using Asset_Management.Data;
using Asset_Management.Models;
using Asset_Management.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Asset_Management.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly ApplicationDbContext _context;

        public AssetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Asset>> GetAllAsync()
        {
            return await _context.Assets
                .Include(a => a.Category)
                .Include(a => a.Status)
                .Include(a => a.Location)
                .ToListAsync();
        }

        public async Task<Asset?> GetByIdAsync(int id)
        {
            return await _context.Assets
                .Include(a => a.Category)
                .Include(a => a.Status)
                .Include(a => a.Location)
                .FirstOrDefaultAsync(a => a.AssetId == id);
        }

        public async Task<IEnumerable<Asset>> FilterByAsync(int? categoryId, int? statusId, int? locationId)
        {
            var query = _context.Assets.AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(a => a.CategoryId == categoryId);

            if (statusId.HasValue)
                query = query.Where(a => a.StatusId == statusId);

            if (locationId.HasValue)
                query = query.Where(a => a.LocationId == locationId);

            return await query
                .Include(a => a.Category)
                .Include(a => a.Status)
                .Include(a => a.Location)
                .ToListAsync();
        }

        public async Task AddAsync(Asset asset)
        {
            await _context.Assets.AddAsync(asset);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Asset asset)
        {
            _context.Assets.Update(asset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset != null)
            {
                _context.Assets.Remove(asset);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Assets.AnyAsync(a => a.AssetId == id);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Status>> GetStatusesAsync()
        {
            return await _context.Statuses.ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync()
        {
            return await _context.Locations.ToListAsync();
        }

        public async Task<Asset> GetAssetByCodeAsync(string assetCode)
        {
            return await _context.Assets
                .FirstOrDefaultAsync(a => a.AssetCode == assetCode);
        }

        public async Task<bool> ExistsAsync(string assetCode)
        {
            return await _context.Assets.AnyAsync(a => a.AssetCode == assetCode);
        }
    }
}
