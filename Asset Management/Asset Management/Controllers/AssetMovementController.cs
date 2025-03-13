using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asset_Management.Controllers
{
    public class AssetMovementController : Controller
    {
        private readonly IAssetMovementService _assetMovementService;
        private readonly IAssetService _assetService;
        private readonly ILocationService _locationService;

        public AssetMovementController(IAssetMovementService assetMovementService, IAssetService assetService, ILocationService locationService)
        {
            _assetMovementService = assetMovementService;
            _assetService = assetService;
            _locationService = locationService;
        }

        public async Task<IActionResult> List()
        {
            var movements = _assetMovementService.GetAllMovements();
            return View(movements);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string assetCode)
        {
            ViewBag.Assets = new SelectList(await _assetService.GetAllAssetsAsync(), "AssetCode", "AssetName", assetCode);
            ViewBag.Locations = new SelectList(await _locationService.GetAllLocationsAsync(), "LocationName", "LocationName");

            var model = new AssetMovementViewModel
            {
                AssetCode = assetCode // Lưu lại Asset đã chọn
            };

            if (!string.IsNullOrEmpty(assetCode))
            {
                var asset = await _assetService.GetAssetByCodeAsync(assetCode);
                model.FromLocation = asset?.Location.LocationName;
            }

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Create(AssetMovementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                  .Select(e => e.ErrorMessage)
                                  .ToList();
                Console.WriteLine("Validation Errors: " + string.Join(", ", errors));
                ViewBag.Assets = new SelectList(await _assetService.GetAllAssetsAsync(), "AssetCode", "AssetName");
                ViewBag.Locations = new SelectList(await _locationService.GetAllLocationsAsync(), "LocationName", "LocationName");

                return View(model);
            }

            try
            {
                _assetMovementService.AddMovement(model);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }

            ViewBag.Assets = new SelectList(await _assetService.GetAllAssetsAsync(), "AssetCode", "AssetName");
            ViewBag.Locations = new SelectList(await _locationService.GetAllLocationsAsync(), "LocationName", "LocationName");

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetCurrentLocation(string assetCode)
        {
            var asset = await _assetService.GetAssetByCodeAsync(assetCode);
            return Json(asset?.Location);
        }
    }
}

