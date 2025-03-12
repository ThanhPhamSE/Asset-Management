using Asset_Management.Models;
using Asset_Management.ViewModels;

namespace Asset_Management.Services.IServices
{
    public interface IAssetMaintenanceService
    {
        Task<IEnumerable<AssetMaintenanceViewModel>> GetMaintenanceHistoryAsync();
        Task<IEnumerable<Asset>> GetAssetsNeedingMaintenanceAsync();
        Task<AssetMaintenanceViewModel> GetByIdAsync(int id);
        Task AddMaintenanceAsync(AssetMaintenanceViewModel maintenanceViewModel);
        Task EditMaintenanceAsync(AssetMaintenanceViewModel maintenanceViewModel);

        Task<IEnumerable<StatusViewModel>> GetStatusesAsync();
    }
}
