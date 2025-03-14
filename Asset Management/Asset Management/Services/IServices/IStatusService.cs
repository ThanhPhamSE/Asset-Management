using Asset_Management.ViewModels;

namespace Asset_Management.Services.IServices
{
    public interface IStatusService
    {
        Task<IEnumerable<StatusViewModel>> GetAllAsync();
    }
}
