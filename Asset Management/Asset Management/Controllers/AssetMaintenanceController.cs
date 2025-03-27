using Asset_Management.Services;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Asset_Management.Controllers
{
    public class AssetMaintenanceController : Controller
    {
        private readonly IAssetMaintenanceService _maintenanceService;

        public AssetMaintenanceController(IAssetMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        public async Task<IActionResult> List()
        {
            var maintenanceHistory = await _maintenanceService.GetMaintenanceHistoryAsync();
            return View(maintenanceHistory);
        }

        public async Task<IActionResult> Add()
        {
            var assets = await _maintenanceService.GetAssetsNeedingMaintenanceAsync();
            ViewBag.Assets = assets;
            return View(new AssetMaintenanceViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(AssetMaintenanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Assets = await _maintenanceService.GetAssetsNeedingMaintenanceAsync();
                TempData["ErrorAddMaintenanceMessage"] = "Thêm bảo trì không thành công! Kiểm tra lại thông tin";
                return View(model);
            }

            await _maintenanceService.AddMaintenanceAsync(model);
            TempData["SuccessAddMaintenanceMessage"] = "Thêm bảo trì thành công!";
            return RedirectToAction("List");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var maintenance = await _maintenanceService.GetByIdAsync(id);
            if (maintenance == null)
            {
                return NotFound();
            }

            ViewBag.Assets = await _maintenanceService.GetAssetsNeedingMaintenanceAsync();
            ViewBag.Statuses = await _maintenanceService.GetStatusesAsync();
            return View(maintenance);
        }

        // Xử lý cập nhật bảo trì
        [HttpPost]
        public async Task<IActionResult> Edit(AssetMaintenanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Assets = await _maintenanceService.GetAssetsNeedingMaintenanceAsync();
                foreach (var error in ModelState)
                {
                    foreach (var subError in error.Value.Errors)
                    {
                        Console.WriteLine($"Key: {error.Key}, Error: {subError.ErrorMessage}");
                    }
                }
                TempData["ErrorAddMaintenanceMessage"] = "Thay đổi bảo trì không thành công! Kiểm tra lại thông tin";
                return View(model);
            }

            try
            {
                await _maintenanceService.EditMaintenanceAsync(model);
                TempData["SuccessAddMaintenanceMessage"] = "Thay đổi bảo trì thành công!";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Assets = await _maintenanceService.GetAssetsNeedingMaintenanceAsync();
                TempData["ErrorAddMaintenanceMessage"] = "Thay đổi bảo trì không thành công! Kiểm tra lại thông tin";
                return View(model);
            }
        }
    }
}
