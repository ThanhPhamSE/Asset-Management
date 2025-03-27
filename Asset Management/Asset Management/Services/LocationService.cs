using Asset_Management.Models;
using Asset_Management.Repositories.IRepositories;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;

namespace Asset_Management.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<LocationViewModel>> GetAllLocationsAsync()
        {
            var locations = await _locationRepository.GetAllLocationsAsync();
            return locations.Select(l => new LocationViewModel
            {
                LocationId = l.LocationId,
                LocationName = l.LocationName,
                Description = l.Description
            }).ToList();
        }

        public async Task<LocationViewModel> GetLocationByIdAsync(int id)
        {
            var location = await _locationRepository.GetLocationByIdAsync(id);
            if (location == null) return null;

            return new LocationViewModel
            {
                LocationId = location.LocationId,
                LocationName = location.LocationName,
                Description = location.Description
            };
        }

        public async Task AddLocationAsync(LocationViewModel model)
        {
            // Kiểm tra Location đã tồn tại (case-insensitive)
            bool exists = await _locationRepository.ExistsByNameAsync(model.LocationName);
            if (exists)
            {
                throw new InvalidOperationException("Location đã tồn tại!");
            }

            var location = new Location
            {
                LocationName = model.LocationName,
                Description = model.Description
            };
            await _locationRepository.AddLocationAsync(location);
        }
        public async Task UpdateLocationAsync(LocationViewModel model)
        {
            var location = await _locationRepository.GetLocationByIdAsync(model.LocationId.Value);
            if (location == null)
            {
                throw new InvalidOperationException("Location không tồn tại!");
            }

            // Nếu tên Location được thay đổi, kiểm tra tên mới đã tồn tại chưa
            if (!string.Equals(location.LocationName, model.LocationName, StringComparison.OrdinalIgnoreCase))
            {
                bool exists = await _locationRepository.ExistsByNameAsync(model.LocationName);
                if (exists)
                {
                    throw new InvalidOperationException("Location đã tồn tại!");
                }
            }

            location.LocationName = model.LocationName;
            location.Description = model.Description;
            await _locationRepository.UpdateLocationAsync(location);
        }

        public async Task DeleteLocationAsync(int id)
        {
            await _locationRepository.DeleteLocationAsync(id);
        }
    }
}
