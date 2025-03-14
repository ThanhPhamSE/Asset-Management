using Asset_Management.Models;
using Asset_Management.Repositories.IRepositories;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;

namespace Asset_Management.Services
{
    public class AssetCheckService : IAssetCheckService
    {
        private readonly IAssetCheckRepository _assetCheckRepository;

        public AssetCheckService(IAssetCheckRepository assetCheckRepository)
        {
            _assetCheckRepository = assetCheckRepository;
        }

        public async Task<IEnumerable<AssetCheckViewModel>> GetAllAsync()
        {
            var assetChecks = await _assetCheckRepository.GetAllAsync();
            return assetChecks.Select(ac => new AssetCheckViewModel
            {
                CheckId = ac.CheckId,
                AssetId = ac.AssetId,
                AssetName = ac.Asset?.AssetName,
                LocationId = ac.LocationId,
                LocationName = ac.Location?.LocationName,
                StatusId = ac.StatusId,
                StatusName = ac.Status?.StatusName,
                CheckDate = ac.CheckDate,
                CheckedBy = ac.CheckedBy,
                Notes = ac.Notes
            });
        }

        public async Task<AssetCheckViewModel> AddAsync(AssetCheckViewModel assetCheckViewModel)
        {
            var assetCheck = new AssetCheck
            {
                AssetId = assetCheckViewModel.AssetId,
                LocationId = assetCheckViewModel.LocationId,
                StatusId = assetCheckViewModel.StatusId,
                CheckDate = assetCheckViewModel.CheckDate,
                CheckedBy = assetCheckViewModel.CheckedBy,
                Notes = assetCheckViewModel.Notes
            };

            var addedCheck = await _assetCheckRepository.AddAsync(assetCheck);
            return await MapToViewModel(addedCheck);
        }

        public async Task<AssetCheckViewModel> EditAsync(AssetCheckViewModel assetCheckViewModel)
        {
            var assetCheck = new AssetCheck
            {
                CheckId = assetCheckViewModel.CheckId,
                AssetId = assetCheckViewModel.AssetId,
                LocationId = assetCheckViewModel.LocationId,
                StatusId = assetCheckViewModel.StatusId,
                CheckDate = assetCheckViewModel.CheckDate,
                CheckedBy = assetCheckViewModel.CheckedBy,
                Notes = assetCheckViewModel.Notes
            };

            var updatedCheck = await _assetCheckRepository.EditAsync(assetCheck);
            return updatedCheck != null ? await MapToViewModel(updatedCheck) : null;
        }

        private async Task<AssetCheckViewModel> MapToViewModel(AssetCheck assetCheck)
        {
            return new AssetCheckViewModel
            {
                CheckId = assetCheck.CheckId,
                AssetId = assetCheck.AssetId,
                AssetName = assetCheck.Asset?.AssetName,
                LocationId = assetCheck.LocationId,
                LocationName = assetCheck.Location?.LocationName,
                StatusId = assetCheck.StatusId,
                StatusName = assetCheck.Status?.StatusName,
                CheckDate = assetCheck.CheckDate,
                CheckedBy = assetCheck.CheckedBy,
                Notes = assetCheck.Notes
            };
        }
    }
}
