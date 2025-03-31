using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace Asset_Management.Controllers
{
    public class AssetController : Controller
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        public async Task<IActionResult> List(int? categoryId, int? statusId, int? locationId, string? searchTerm, int page = 1, int pageSize = 3)
        {
            categoryId = categoryId == 0 ? null : categoryId;
            statusId = statusId == 0 ? null : statusId;
            locationId = locationId == 0 ? null : locationId;

            var (assets, totalItems) = await _assetService.FilterAssetsAsync(categoryId, statusId, locationId, searchTerm, page, pageSize);

            ViewBag.Categories = await _assetService.GetCategoriesAsync();
            ViewBag.Statuses = await _assetService.GetStatusesAsync();
            ViewBag.Locations = await _assetService.GetLocationsAsync();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asset = await _assetService.GetAssetByIdAsync(id);
            if (asset == null)
            {
                return NotFound();
            }

            await _assetService.DeleteAssetAsync(id);
            return RedirectToAction("List"); // Quay về danh sách mà không vào View mới
        }

        public async Task<IActionResult> ExportToExcel(int? categoryId, int? statusId, int? locationId, string? searchTerm)
        {
            try
            {
                // 🔹 Cấu hình giấy phép đúng cách (EPPlus 8+)
                ExcelPackage.License.SetNonCommercialPersonal("phamhoangthanh1582003@gmail.com"); // Hoặc SetNonCommercialOrganization

                var (assets, _) = await _assetService.FilterAssetsAsync(categoryId, statusId, locationId, searchTerm, 1, int.MaxValue);

                if (assets == null || !assets.Any())
                {
                    TempData["ErrorMessage"] = "Không có dữ liệu để xuất!";
                    return RedirectToAction(nameof(List));
                }

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Assets");

                    var headers = new string[] { "ID", "Tên tài sản", "Mã tài sản", "Danh mục", "Trạng thái", "Vị trí", "Ngày tạo", "Giá mua" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    using (var range = worksheet.Cells[1, 1, 1, headers.Length])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    int row = 2;
                    foreach (var asset in assets)
                    {
                        worksheet.Cells[row, 1].Value = asset.AssetId;
                        worksheet.Cells[row, 2].Value = asset.AssetName;
                        worksheet.Cells[row, 3].Value = asset.AssetCode;
                        worksheet.Cells[row, 4].Value = asset.CategoryName;
                        worksheet.Cells[row, 5].Value = asset.StatusName;
                        worksheet.Cells[row, 6].Value = asset.LocationName;
                        worksheet.Cells[row, 7].Value = asset.PurchaseDate.ToString("yyyy-MM-dd");
                        worksheet.Cells[row, 8].Value = asset.PurchasePrice;
                        row++;
                    }

                    worksheet.Cells.AutoFitColumns();

                    // 👉 Không dùng `using` để tránh bị đóng stream quá sớm
                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;

                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Assets.xlsx");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi xuất Excel: {ex.Message}";
                return RedirectToAction(nameof(List));
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn một file Excel hợp lệ!";
                return RedirectToAction(nameof(List));
            }

            try
            {
                ExcelPackage.License.SetNonCommercialPersonal("phamhoangthanh1582003@gmail.com");

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            TempData["ErrorMessage"] = "File Excel không hợp lệ!";
                            return RedirectToAction(nameof(List));
                        }

                        int rowCount = worksheet.Dimension.Rows;
                        List<AssetViewModel> assets = new List<AssetViewModel>();

                        for (int row = 2; row <= rowCount; row++) // Bỏ qua dòng tiêu đề
                        {
                            var assetViewModel = new AssetViewModel
                            {
                                AssetName = worksheet.Cells[row, 1].Value?.ToString(),
                                AssetCode = worksheet.Cells[row, 2].Value?.ToString(),
                                CategoryId = int.TryParse(worksheet.Cells[row, 3].Value?.ToString(), out int catId) ? catId : 0,
                                StatusId = int.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out int statusId) ? statusId : 0,
                                LocationId = int.TryParse(worksheet.Cells[row, 5].Value?.ToString(), out int locId) ? locId : 0,
                                PurchaseDate = DateTime.TryParse(worksheet.Cells[row, 6].Value?.ToString(), out DateTime purchaseDate) ? purchaseDate : DateTime.Now,
                                PurchasePrice = decimal.TryParse(worksheet.Cells[row, 7].Value?.ToString(), out decimal price) ? price : 0
                            };

                            if (!string.IsNullOrEmpty(assetViewModel.AssetCode)) // Kiểm tra hợp lệ
                            {
                                assets.Add(assetViewModel);
                            }
                        }


                        foreach (var asset in assets)
                        {
                            try
                            {
                                await _assetService.AddAssetAsync(asset);
                            }
                            catch (InvalidOperationException ex)
                            {
                                TempData["ErrorMessage"] = $"Lỗi với tài sản {asset.AssetCode}: {ex.Message}";
                            }
                        }
                    }
                }

                TempData["SuccessMessage"] = "Nhập tài sản từ Excel thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi nhập Excel: {ex.Message}";
            }

            return RedirectToAction(nameof(List));
        }



    }
}
