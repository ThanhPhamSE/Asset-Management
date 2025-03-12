using Asset_Management.Models;

namespace Asset_Management.Repositories.IRepositories
{
    public interface IAssetMaintenanceRepository
    {
        Task<IEnumerable<AssetMaintenance>> GetMaintenanceHistoryAsync();
        Task<IEnumerable<Asset>> GetAssetsNeedingMaintenanceAsync();
        Task<AssetMaintenance> GetByIdAsync(int id);
        Task AddAsync(AssetMaintenance maintenance);

        Task EditAsync(AssetMaintenance maintenance);

        Task<IEnumerable<Status>> GetStatusesAsync();
        Task SaveAsync();
    }
}
