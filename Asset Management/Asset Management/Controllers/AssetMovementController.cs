using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml.Style;
using OfficeOpenXml;

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
                TempData["ErrorAssetMovementMessage"] = $"Thêm Asset Movement không thành công kiểm tra lại thông tin";
                return View(model);
            }

            try
            {
                _assetMovementService.AddMovement(model);
                TempData["SuccessAssetMovementMessage"] = "Thêm Asset Movement thành công!";
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

        [HttpGet]
        public IActionResult ExportToExcel()
        {
            var movements = _assetMovementService.GetAllMovements();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Asset Movements");

                // Header
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Asset Code";
                worksheet.Cells[1, 3].Value = "Asset Name";
                worksheet.Cells[1, 4].Value = "From Location";
                worksheet.Cells[1, 5].Value = "To Location";
                worksheet.Cells[1, 6].Value = "Move Date";
                worksheet.Cells[1, 7].Value = "Responsible Person";

                using (var headerRange = worksheet.Cells[1, 1, 1, 7])
                {
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Dữ liệu
                int row = 2;
                foreach (var movement in movements)
                {
                    worksheet.Cells[row, 1].Value = movement.MovementId;
                    worksheet.Cells[row, 2].Value = movement.AssetCode;
                    worksheet.Cells[row, 3].Value = movement.AssetName;
                    worksheet.Cells[row, 4].Value = movement.FromLocation;
                    worksheet.Cells[row, 5].Value = movement.ToLocation;
                    worksheet.Cells[row, 6].Value = movement.MoveDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 7].Value = movement.ResponsiblePerson;
                    row++;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AssetMovements.xlsx");
            }
        }

    }
}

