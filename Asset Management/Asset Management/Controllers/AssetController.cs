using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Asset_Management.Controllers
{
    public class AssetController : Controller
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        public async Task<IActionResult> List(int? categoryId, int? statusId, int? locationId)
        {
            categoryId = categoryId == 0 ? null : categoryId;
            statusId = statusId == 0 ? null : statusId;
            locationId = locationId == 0 ? null : locationId;

            var assets = await _assetService.FilterAssetsAsync(categoryId, statusId, locationId);
            ViewBag.Categories = await _assetService.GetCategoriesAsync();
            ViewBag.Statuses = await _assetService.GetStatusesAsync();
            ViewBag.Locations = await _assetService.GetLocationsAsync();
            return View(assets);
        }

        //public async Task<IActionResult> Details(int id)
        //{
        //    var asset = await _assetService.GetAssetByIdAsync(id);
        //    if (asset == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(asset);
        //}

        public async Task<IActionResult> CreateAsset()
        {
            ViewBag.Categories = await _assetService.GetCategoriesAsync();
            ViewBag.Statuses = await _assetService.GetStatusesAsync();
            ViewBag.Locations = await _assetService.GetLocationsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsset(AssetViewModel assetViewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine(error); // Debug lỗi
                }

                ViewBag.Categories = await _assetService.GetCategoriesAsync();
                ViewBag.Statuses = await _assetService.GetStatusesAsync();
                ViewBag.Locations = await _assetService.GetLocationsAsync();
                return View(assetViewModel);
            }

            try
            {
                await _assetService.AddAssetAsync(assetViewModel);
                TempData["SuccessMessage"] = "Thêm tài sản thành công!";
            }
            catch (InvalidOperationException ex)
            {
                // Thêm lỗi vào ModelState cho trường AssetCode để hiển thị thông báo lỗi
                ModelState.AddModelError("AssetCode", ex.Message);
                ViewBag.Categories = await _assetService.GetCategoriesAsync();
                ViewBag.Statuses = await _assetService.GetStatusesAsync();
                ViewBag.Locations = await _assetService.GetLocationsAsync();
                return View(assetViewModel);
            }

            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AssetViewModel assetViewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine(error); // Debug lỗi
                }

                ViewBag.Categories = await _assetService.GetCategoriesAsync();
                ViewBag.Statuses = await _assetService.GetStatusesAsync();
                ViewBag.Locations = await _assetService.GetLocationsAsync();
                return View(assetViewModel);
            }

            try
            {
                await _assetService.UpdateAssetAsync(assetViewModel);
                TempData["SuccessMessage"] = "Cập nhật tài sản thành công!";
            }
            catch (InvalidOperationException ex)
            {
                // Thêm lỗi vào ModelState cho trường AssetCode để hiển thị thông báo lỗi cho người dùng
                ModelState.AddModelError("AssetCode", ex.Message);
                ViewBag.Categories = await _assetService.GetCategoriesAsync();
                ViewBag.Statuses = await _assetService.GetStatusesAsync();
                ViewBag.Locations = await _assetService.GetLocationsAsync();
                return View(assetViewModel);
            }

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var asset = await _assetService.GetAssetByIdAsync(id);
            if (asset == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _assetService.GetCategoriesAsync();
            ViewBag.Statuses = await _assetService.GetStatusesAsync();
            ViewBag.Locations = await _assetService.GetLocationsAsync();

            return View(asset);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _assetService.GetAssetByIdAsync(id);
            if (asset == null)
            {
                return NotFound();
            }
            return View(asset);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _assetService.DeleteAssetAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}
