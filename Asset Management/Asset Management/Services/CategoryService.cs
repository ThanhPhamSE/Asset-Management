using Asset_Management.Models;
using Asset_Management.Repositories.IRepositories;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;

namespace Asset_Management.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryViewModel
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description
            }).ToList();
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };
        }

        public async Task AddCategoryAsync(CategoryViewModel model)
        {
            // Kiểm tra danh mục đã tồn tại (case-insensitive)
            bool exists = await _categoryRepository.ExistsByNameAsync(model.CategoryName);
            if (exists)
            {
                throw new InvalidOperationException("Danh mục đã tồn tại!");
            }

            var category = new Category
            {
                CategoryName = model.CategoryName,
                Description = model.Description
            };
            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(CategoryViewModel model)
        {
            var category = await _categoryRepository.GetByIdAsync(model.CategoryId);
            if (category == null)
            {
                throw new InvalidOperationException("Danh mục không tồn tại!");
            }

            // Nếu tên danh mục được thay đổi, kiểm tra xem tên mới đã tồn tại hay chưa (case-insensitive)
            if (!string.Equals(category.CategoryName, model.CategoryName, StringComparison.OrdinalIgnoreCase))
            {
                bool exists = await _categoryRepository.ExistsByNameAsync(model.CategoryName);
                if (exists)
                {
                    throw new InvalidOperationException("Danh mục đã tồn tại!");
                }
            }

            category.CategoryName = model.CategoryName;
            category.Description = model.Description;
            await _categoryRepository.UpdateAsync(category);
        }


        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}
