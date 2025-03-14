using Asset_Management.Models;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asset_Management.Controllers
{
    public class AssetCheckController : Controller
    {
        private readonly IAssetCheckService _assetCheckService;
        private readonly IAssetService _assetService;
        private readonly ILocationService _locationService;
        private readonly IStatusService _statusService;

        public AssetCheckController(
            IAssetCheckService assetCheckService,
            IAssetService assetService,
            ILocationService locationService,
            IStatusService statusService)
        {
            _assetCheckService = assetCheckService;
            _assetService = assetService;
            _locationService = locationService;
            _statusService = statusService;
        }

        // 1. Hiển thị danh sách Asset Checks
        public async Task<IActionResult> List()
        {
            var assetChecks = await _assetCheckService.GetAllAsync();
            return View(assetChecks);
        }

        // 2. Hiển thị form thêm Asset Check
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new AssetCheckViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AssetCheckViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                  .Select(e => e.ErrorMessage)
                                  .ToList();
                Console.WriteLine("Validation Errors: " + string.Join(", ", errors));
                await PopulateDropdowns();
                return View(model);
            }

            await _assetCheckService.AddAsync(model);

            return RedirectToAction(nameof(List));
        }





        // 3. Hiển thị form chỉnh sửa Asset Check
        public async Task<IActionResult> Edit(int id)
        {
            var assetCheck = await _assetCheckService.GetAllAsync();
            var check = assetCheck.FirstOrDefault(x => x.CheckId == id);
            if (check == null) return NotFound();

            await PopulateDropdowns();
            return View(check);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AssetCheckViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns();
                return View(model);
            }

            await _assetCheckService.EditAsync(model);
            return RedirectToAction(nameof(List));
        }

        // Hàm Load dữ liệu dropdown list
        private async Task PopulateDropdowns()
        {
            ViewBag.Assets = new SelectList(await _assetService.GetAllAssetsAsync(), "AssetId", "AssetName");
            ViewBag.Locations = new SelectList(await _locationService.GetAllLocationsAsync(), "LocationId", "LocationName");
            ViewBag.Statuses = new SelectList(await _statusService.GetAllAsync(), "StatusId", "StatusName");
        }

        [HttpGet]
        public async Task<IActionResult> GetAssetDetails(int assetId)
        {
            var asset = await _assetService.GetAssetByIdAsync(assetId);
            if (asset == null)
            {
                return NotFound();
            }

            var location = await _locationService.GetLocationByIdAsync(asset.LocationId);

            var response = new
            {
                locationId = asset.LocationId,
                locationName = location?.LocationName ?? "Unknown",
                statusId = asset.StatusId
            };

            Console.WriteLine($"Asset Details: {Newtonsoft.Json.JsonConvert.SerializeObject(response)}");

            return Json(response);

        }

    }
}
