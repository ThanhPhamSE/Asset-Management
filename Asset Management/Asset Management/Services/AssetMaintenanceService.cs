using Asset_Management.Models;
using Asset_Management.Repositories;
using Asset_Management.Repositories.IRepositories;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;

namespace Asset_Management.Services
{
    public class AssetMaintenanceService : IAssetMaintenanceService
    {
        private readonly IAssetMaintenanceRepository _maintenanceRepository;

        public AssetMaintenanceService(IAssetMaintenanceRepository maintenanceRepository)
        {
            _maintenanceRepository = maintenanceRepository;
        }

        public async Task<IEnumerable<AssetMaintenanceViewModel>> GetMaintenanceHistoryAsync()
        {
            var maintenanceHistory = await _maintenanceRepository.GetMaintenanceHistoryAsync();

            return maintenanceHistory.Select(m => new AssetMaintenanceViewModel
            {
                MaintenanceId = m.MaintenanceId,
                AssetId = m.AssetId,
                AssetName = m.Asset?.AssetName, // Lấy tên tài sản (nếu có)
                MaintenanceDate = m.MaintenanceDate,
                MaintenanceType = m.MaintenanceType,
                MaintenanceCost = m.MaintenanceCost,
                StatusId = m.StatusId,
                StatusName = m.Status?.StatusName, // Lấy tên trạng thái (nếu có)
                Notes = m.Notes
            });
        }


        public async Task<IEnumerable<Asset>> GetAssetsNeedingMaintenanceAsync()
        {
            return await _maintenanceRepository.GetAssetsNeedingMaintenanceAsync();
        }

        public async Task<AssetMaintenanceViewModel> GetByIdAsync(int id)
        {
            var maintenance = await _maintenanceRepository.GetByIdAsync(id);

            if (maintenance == null)
            {
                return null; // Trả về null nếu không tìm thấy bản ghi
            }

            return new AssetMaintenanceViewModel
            {
                MaintenanceId = maintenance.MaintenanceId,
                AssetId = maintenance.AssetId,
                AssetName = maintenance.Asset?.AssetName, // Lấy tên tài sản (nếu có)
                MaintenanceDate = maintenance.MaintenanceDate,
                MaintenanceType = maintenance.MaintenanceType,
                MaintenanceCost = maintenance.MaintenanceCost,
                StatusId = maintenance.StatusId,
                StatusName = maintenance.Status?.StatusName, // Lấy tên trạng thái (nếu có)
                Notes = maintenance.Notes
            };
        }


        public async Task AddMaintenanceAsync(AssetMaintenanceViewModel maintenanceViewModel)
        {
            var maintenance = new AssetMaintenance
            {
                AssetId = maintenanceViewModel.AssetId,
                MaintenanceDate = maintenanceViewModel.MaintenanceDate,
                MaintenanceType = maintenanceViewModel.MaintenanceType,
                MaintenanceCost = maintenanceViewModel.MaintenanceCost,
                StatusId = 3,
                Notes = maintenanceViewModel.Notes
            };

            await _maintenanceRepository.AddAsync(maintenance);
        }

        public async Task EditMaintenanceAsync(AssetMaintenanceViewModel maintenanceViewModel)
        {
            var existingMaintenance = await _maintenanceRepository.GetByIdAsync(maintenanceViewModel.MaintenanceId);
            if (existingMaintenance == null)
            {
                throw new Exception("Maintenance record not found.");
            }

            existingMaintenance.AssetId = maintenanceViewModel.AssetId;
            existingMaintenance.MaintenanceDate = maintenanceViewModel.MaintenanceDate;
            existingMaintenance.MaintenanceType = maintenanceViewModel.MaintenanceType;
            existingMaintenance.MaintenanceCost = maintenanceViewModel.MaintenanceCost;
            existingMaintenance.StatusId = maintenanceViewModel.StatusId;
            existingMaintenance.Notes = maintenanceViewModel.Notes;

            await _maintenanceRepository.EditAsync(existingMaintenance);
        }

        public async Task<IEnumerable<StatusViewModel>> GetStatusesAsync()
        {
            var statuses = await _maintenanceRepository.GetStatusesAsync() ?? new List<Status>();
            return statuses.Select(s => new StatusViewModel { StatusId = s.StatusId, StatusName = s.StatusName });
        }
    }
}
