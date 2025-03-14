using Asset_Management.Repositories.IRepositories;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels;

namespace Asset_Management.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;

        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public async Task<IEnumerable<StatusViewModel>> GetAllAsync()
        {
            var statuses = await _statusRepository.GetAllAsync();
            return statuses.Select(s => new StatusViewModel
            {
                StatusId = s.StatusId,
                StatusName = s.StatusName
            });
        }
    }
}
