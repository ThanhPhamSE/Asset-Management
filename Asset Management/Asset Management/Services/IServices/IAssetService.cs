using Asset_Management.Models;
using Asset_Management.ViewModels;

namespace Asset_Management.Services.IServices
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetViewModel>> GetAllAssetsAsync();
        Task<AssetViewModel> GetAssetByIdAsync(int id);
        Task<IEnumerable<AssetViewModel>> FilterAssetsAsync(int? categoryId, int? statusId, int? locationId);
        Task<(IEnumerable<AssetViewModel>, int)> FilterAssetsAsync(int? categoryId, int? statusId, int? locationId, string? searchTerm, int page, int pageSize);

        Task AddAssetAsync(AssetViewModel assetViewModel);
        Task UpdateAssetAsync(AssetViewModel assetViewModel);
        Task DeleteAssetAsync(int id);

        Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();
        Task<IEnumerable<StatusViewModel>> GetStatusesAsync();
        Task<IEnumerable<LocationViewModel>> GetLocationsAsync();

        Task<Asset> GetAssetByCodeAsync(string assetCode);
    }
}

