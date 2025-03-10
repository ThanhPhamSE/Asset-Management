using Asset_Management.ViewModels;

namespace Asset_Management.Services.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();
        Task<CategoryViewModel> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(CategoryViewModel model);
        Task UpdateCategoryAsync(CategoryViewModel model);
        Task DeleteCategoryAsync(int id);
    }
}
