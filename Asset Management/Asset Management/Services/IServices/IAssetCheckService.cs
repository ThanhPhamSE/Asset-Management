using Asset_Management.ViewModels;

namespace Asset_Management.Services.IServices
{
    public interface IAssetCheckService
    {
        Task<IEnumerable<AssetCheckViewModel>> GetAllAsync();
        Task<AssetCheckViewModel> AddAsync(AssetCheckViewModel assetCheckViewModel);
        Task<AssetCheckViewModel> EditAsync(AssetCheckViewModel assetCheckViewModel);
    }
}
