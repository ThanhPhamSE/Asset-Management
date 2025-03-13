using Asset_Management.Models;

namespace Asset_Management.Repositories.IRepositories
{
    public interface IAssetRepository
    {
        Task<IEnumerable<Asset>> GetAllAsync();
        Task<Asset> GetByIdAsync(int id);
        Task<IEnumerable<Asset>> FilterByAsync(int? categoryId, int? statusId, int? locationId);
        Task AddAsync(Asset asset);
        Task UpdateAsync(Asset asset);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<IEnumerable<Status>> GetStatusesAsync();
        Task<IEnumerable<Location>> GetLocationsAsync();

        Task<Asset> GetAssetByCodeAsync(string assetCode);
    }
}
