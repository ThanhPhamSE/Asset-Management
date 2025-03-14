using Asset_Management.Models;

namespace Asset_Management.Repositories.IRepositories
{
    public interface IAssetCheckRepository
    {
        Task<IEnumerable<AssetCheck>> GetAllAsync();
        Task<AssetCheck> AddAsync(AssetCheck assetCheck);
        Task<AssetCheck> EditAsync(AssetCheck assetCheck);
    }
}
