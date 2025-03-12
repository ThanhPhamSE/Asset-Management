using Asset_Management.Data;
using Asset_Management.Models;
using Asset_Management.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Asset_Management.Repositories
{
    public class AssetMaintenanceRepository : IAssetMaintenanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AssetMaintenanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssetMaintenance>> GetMaintenanceHistoryAsync()
        {
            return await _context.AssetMaintenances
                .Include(a => a.Asset)
                .Include(s => s.Status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Asset>> GetAssetsNeedingMaintenanceAsync()
        {
            return await _context.Assets
                .Where(a => a.Status.StatusName == "Need Maintenance")
                .ToListAsync();
        }

        public async Task<AssetMaintenance> GetByIdAsync(int id)
        {
            return await _context.AssetMaintenances.FindAsync(id);
        }

        public async Task AddAsync(AssetMaintenance maintenance)
        {
            // Kiểm tra xem StatusId có tồn tại không
            var statusExists = await _context.Statuses.AnyAsync(s => s.StatusId == maintenance.StatusId);
            if (!statusExists)
            {
                throw new Exception($"StatusId {maintenance.StatusId} không tồn tại trong bảng Statuses.");
            }

            await _context.AssetMaintenances.AddAsync(maintenance);

            var asset = await _context.Assets.FindAsync(maintenance.AssetId);
            var maintenanceStatus = await _context.Statuses
                                                   .Where(s => s.StatusName == "Under Maintenance")
                                                   .Select(s => s.StatusId)
                                                   .FirstOrDefaultAsync();

            if (asset != null && maintenanceStatus != 0)
            {
                asset.StatusId = maintenanceStatus;
            }

            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(AssetMaintenance maintenance)
        {
            var existingMaintenance = await _context.AssetMaintenances.FindAsync(maintenance.MaintenanceId);
            if (existingMaintenance == null)
            {
                throw new Exception("Không tìm thấy thông tin bảo trì để cập nhật.");
            }

            // Kiểm tra xem StatusId có tồn tại trong bảng Statuses không
            bool statusExists = await _context.Statuses.AnyAsync(s => s.StatusId == maintenance.StatusId);
            if (!statusExists)
            {
                throw new Exception($"StatusId {maintenance.StatusId} không tồn tại trong bảng Statuses.");
            }

            existingMaintenance.AssetId = maintenance.AssetId;
            existingMaintenance.MaintenanceDate = maintenance.MaintenanceDate;
            existingMaintenance.MaintenanceType = maintenance.MaintenanceType;
            existingMaintenance.MaintenanceCost = maintenance.MaintenanceCost;
            existingMaintenance.StatusId = maintenance.StatusId;
            existingMaintenance.Notes = maintenance.Notes;

            if (maintenance.StatusId == 6)
            {
                var asset = await _context.Assets.FindAsync(maintenance.AssetId);
                if (asset != null)
                {
                    asset.StatusId = 1; // Available
                    _context.Assets.Update(asset);
                }
            }

            _context.AssetMaintenances.Update(existingMaintenance);
            await _context.SaveChangesAsync();
        }




        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Status>> GetStatusesAsync()
        {
            return await _context.Statuses.ToListAsync();
        }
    }
}
