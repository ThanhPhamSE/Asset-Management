using Asset_Management.Data;
using Asset_Management.Models;
using Asset_Management.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Asset_Management.Repositories
{
    public class AssetCheckRepository : IAssetCheckRepository
    {
        private readonly ApplicationDbContext _context;

        public AssetCheckRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssetCheck>> GetAllAsync()
        {
            return await _context.AssetChecks
                .Include(ac => ac.Asset)
                .Include(ac => ac.Location)
                .Include(ac => ac.Status)
                .ToListAsync();
        }

        public async Task<AssetCheck> AddAsync(AssetCheck assetCheck)
        {
            _context.AssetChecks.Add(assetCheck);
            var asset = await _context.Assets.FindAsync(assetCheck.AssetId);
            if (asset != null)
            {
                asset.StatusId = assetCheck.StatusId; // Cập nhật StatusId của Asset
                await _context.SaveChangesAsync();
            }

            return assetCheck;
        }

        public async Task<AssetCheck> EditAsync(AssetCheck assetCheck)
        {
            var existing = await _context.AssetChecks.FindAsync(assetCheck.CheckId);
            if (existing == null)
            {
                return null;
            }

            existing.AssetId = assetCheck.AssetId;
            existing.LocationId = assetCheck.LocationId;
            existing.CheckDate = assetCheck.CheckDate;
            existing.CheckedBy = assetCheck.CheckedBy;
            existing.StatusId = assetCheck.StatusId;
            existing.Notes = assetCheck.Notes;

            var asset = await _context.Assets.FindAsync(assetCheck.AssetId);
            if (asset != null)
            {
                asset.StatusId = assetCheck.StatusId;
            }

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
