using Asset_Management.Services;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

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

        public async Task<IActionResult> ExportToExcel()
        {
            var maintenanceHistory = await _maintenanceService.GetMaintenanceHistoryAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Maintenance History");

                // Tiêu đề cột
                worksheet.Cells[1, 1].Value = "Asset";
                worksheet.Cells[1, 2].Value = "Maintenance Date";
                worksheet.Cells[1, 3].Value = "Maintenance Type";
                worksheet.Cells[1, 4].Value = "Maintenance Price";
                worksheet.Cells[1, 5].Value = "Note";
                worksheet.Cells[1, 6].Value = "Status";

                // Định dạng tiêu đề
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    range.AutoFitColumns();
                }

                // Thêm dữ liệu vào Excel
                int row = 2;
                foreach (var item in maintenanceHistory)
                {
                    worksheet.Cells[row, 1].Value = item.AssetName;
                    worksheet.Cells[row, 2].Value = item.MaintenanceDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 3].Value = item.MaintenanceType;
                    worksheet.Cells[row, 4].Value = item.MaintenanceCost;
                    worksheet.Cells[row, 5].Value = item.Notes;
                    worksheet.Cells[row, 6].Value = item.StatusName;
                    row++;
                }

                worksheet.Cells.AutoFitColumns();

                // Xuất file Excel
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MaintenanceHistory.xlsx");
            }
        }
    }
}
