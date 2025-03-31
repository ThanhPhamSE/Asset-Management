using Asset_Management.Models;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;

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
                TempData["ErrorCheckMessage"] = "Tạo Asset Check không thành công. Kiểm tra lại thông tin";
                return View(model);
            }

            await _assetCheckService.AddAsync(model);
            TempData["SuccessCheckMessage"] = "Tạo Asset Check thành công!";
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
            TempData["SuccessCheckMessage"] = "Cập nhật Asset Check thành công!";
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


        public async Task<IActionResult> ExportToExcel()
        {
            var assetChecks = await _assetCheckService.GetAllAsync();
            ExcelPackage.License.SetNonCommercialPersonal("phamhoangthanh1582003@gmail.com");
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Asset Checks");

                // Tiêu đề cột
                worksheet.Cells[1, 1].Value = "Asset";
                worksheet.Cells[1, 2].Value = "Location";
                worksheet.Cells[1, 3].Value = "Check Date";
                worksheet.Cells[1, 4].Value = "Checked By";
                worksheet.Cells[1, 5].Value = "Notes";
                worksheet.Cells[1, 6].Value = "Status";

                // Định dạng tiêu đề
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    range.AutoFitColumns();
                }

                // Thêm dữ liệu
                int row = 2;
                foreach (var item in assetChecks)
                {
                    worksheet.Cells[row, 1].Value = item.AssetName;
                    worksheet.Cells[row, 2].Value = item.LocationName;
                    worksheet.Cells[row, 3].Value = item.CheckDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 4].Value = item.CheckedBy;
                    worksheet.Cells[row, 5].Value = item.Notes;
                    worksheet.Cells[row, 6].Value = item.StatusName;
                    row++;
                }

                // Tự động căn chỉnh cột
                worksheet.Cells.AutoFitColumns();

                // Xuất file Excel
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AssetChecks.xlsx");
            }
        }
    }
}
