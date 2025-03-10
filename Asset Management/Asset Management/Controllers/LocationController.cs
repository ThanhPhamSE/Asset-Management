using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Asset_Management.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task<IActionResult> List()
        {
            IEnumerable<LocationViewModel> locations = await _locationService.GetAllLocationsAsync();
            return View(locations);
        }

        [HttpPost]
        public async Task<IActionResult> AddLocation(LocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _locationService.AddLocationAsync(model);
                return RedirectToAction(nameof(List));
            }

            IEnumerable<LocationViewModel> locations = await _locationService.GetAllLocationsAsync();
            return View("List", locations);
        }

        [HttpPost]
        public async Task<IActionResult> EditLocation(LocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _locationService.UpdateLocationAsync(model);
                return RedirectToAction(nameof(List));
            }

            IEnumerable<LocationViewModel> locations = await _locationService.GetAllLocationsAsync();
            return View("List", locations);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            await _locationService.DeleteLocationAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}
