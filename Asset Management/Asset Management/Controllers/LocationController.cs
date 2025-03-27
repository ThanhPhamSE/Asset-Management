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
                try
                {
                    await _locationService.AddLocationAsync(model);
                    TempData["SuccessLocationMessage"] = "Thêm Location thành công!";
                }
                catch (InvalidOperationException ex)
                {
                    TempData["ErrorLocationMessage"] = ex.Message;
                }
                catch (Exception ex)
                {
                    TempData["ErrorLocationMessage"] = $"Có lỗi xảy ra: {ex.Message}";
                }
                return RedirectToAction("List");
            }

            TempData["ErrorLocationMessage"] = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại thông tin.";
            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> EditLocation(LocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _locationService.UpdateLocationAsync(model);
                    TempData["SuccessLocationMessage"] = "Cập nhật Location thành công!";
                }
                catch (InvalidOperationException ex)
                {
                    TempData["ErrorLocationMessage"] = ex.Message;
                }
                catch (Exception ex)
                {
                    TempData["ErrorLocationMessage"] = $"Có lỗi xảy ra: {ex.Message}";
                }
                return RedirectToAction("List");
            }

            TempData["ErrorLocationMessage"] = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại thông tin.";
            return RedirectToAction("List");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            await _locationService.DeleteLocationAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}
