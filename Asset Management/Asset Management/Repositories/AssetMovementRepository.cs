using Asset_Management.Data;
using Asset_Management.Models;
using Asset_Management.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Asset_Management.Repositories
{
    public class AssetMovementRepository : IAssetMovementRepository
    {
        private readonly ApplicationDbContext _context;

        public AssetMovementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddMovement(AssetMovement movement)
        {
            _context.AssetMovements.Add(movement);

            var asset = _context.Assets.FirstOrDefault(a => a.AssetId == movement.AssetId);
            if (asset != null)
            {
                asset.Location = movement.ToLocation; // Cập nhật vị trí mới
            }

            _context.SaveChanges();
        }

        public IEnumerable<AssetMovement> GetAllMovements()
        {
            return _context.AssetMovements
                .Include(am => am.Asset)
                .Include(am => am.FromLocation)
                .Include(am => am.ToLocation)
                .ToList();
        }

        public Asset GetAssetByCode(string assetCode)
        {
            return _context.Assets.FirstOrDefault(a => a.AssetCode == assetCode);
        }

        public Location GetLocationByName(string locationName)
        {
            return _context.Locations.FirstOrDefault(l => l.LocationName == locationName);
        }
    }
}
