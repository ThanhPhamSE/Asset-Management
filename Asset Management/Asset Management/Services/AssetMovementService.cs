using Asset_Management.Models;
using Asset_Management.Repositories.IRepositories;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;

namespace Asset_Management.Services
{
    public class AssetMovementService : IAssetMovementService
    {
        private readonly IAssetMovementRepository _repository;

        public AssetMovementService(IAssetMovementRepository repository)
        {
            _repository = repository;
        }

        public void AddMovement(AssetMovementViewModel movementViewModel)
        {
            var asset = _repository.GetAssetByCode(movementViewModel.AssetCode);
            var fromLocation = _repository.GetLocationByName(movementViewModel.FromLocation);
            var toLocation = _repository.GetLocationByName(movementViewModel.ToLocation);

            if (asset == null || fromLocation == null || toLocation == null)
            {
                throw new ArgumentException("Invalid asset or location details.");
            }

            var movement = new AssetMovement
            {
                AssetId = asset.AssetId,
                FromLocationId = fromLocation.LocationId,
                ToLocationId = toLocation.LocationId,
                MoveDate = movementViewModel.MoveDate,
                ResponsiblePerson = movementViewModel.ResponsiblePerson
            };

            _repository.AddMovement(movement);
        }

        public IEnumerable<AssetMovementViewModel> GetAllMovements()
        {
            return _repository.GetAllMovements().Select(am => new AssetMovementViewModel
            {
                MovementId = am.MovementId,
                AssetCode = am.Asset.AssetCode,
                AssetName = am.Asset.AssetName,
                FromLocation = am.FromLocation.LocationName,
                ToLocation = am.ToLocation.LocationName,
                MoveDate = am.MoveDate,
                ResponsiblePerson = am.ResponsiblePerson
            });
        }
    }
}
