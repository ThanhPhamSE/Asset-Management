using Asset_Management.Models;

namespace Asset_Management.Repositories.IRepositories
{
    public interface IAssetMovementRepository
    {
        void AddMovement(AssetMovement movement);
        IEnumerable<AssetMovement> GetAllMovements();
        Asset GetAssetByCode(string assetCode);
        Location GetLocationByName(string locationName);
    }
}
