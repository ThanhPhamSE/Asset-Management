using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asset_Management.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        
        public async Task<IActionResult> List()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.AddCategoryAsync(model);
                    TempData["SuccessCategoryMessage"] = "Thêm danh mục thành công!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorCategoryMessage"] = $"Có lỗi xảy ra: {ex.Message}";
                }
                return RedirectToAction("List");
            }
            TempData["ErrorCategoryMessage"] = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại thông tin.";
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.UpdateCategoryAsync(model);
                    TempData["SuccessCategoryMessage"] = "Cập nhật danh mục thành công!";
                }
                catch (InvalidOperationException ex)
                {
                    TempData["ErrorCategoryMessage"] = ex.Message;
                }
                catch (Exception ex)
                {
                    TempData["ErrorCategoryMessage"] = $"Có lỗi xảy ra: {ex.Message}";
                }
                return RedirectToAction("List");
            }
            TempData["ErrorCategoryMessage"] = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại thông tin.";
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction("List");
        }
    }
}
