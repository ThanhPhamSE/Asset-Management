using Asset_Management.Models;
using Asset_Management.ViewModels;

namespace Asset_Management.Services.IServices
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationViewModel>> GetAllLocationsAsync();
        Task<LocationViewModel> GetLocationByIdAsync(int id);
        Task AddLocationAsync(LocationViewModel model);
        Task UpdateLocationAsync(LocationViewModel model);
        Task DeleteLocationAsync(int id);
    }
}
