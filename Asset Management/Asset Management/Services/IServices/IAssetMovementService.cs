using Asset_Management.ViewModels;

namespace Asset_Management.Services.IServices
{
    public interface IAssetMovementService
    {
        void AddMovement(AssetMovementViewModel movement);
        IEnumerable<AssetMovementViewModel> GetAllMovements();
    }
}
