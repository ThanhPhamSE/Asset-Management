using Asset_Management.Models;

namespace Asset_Management.Repositories.IRepositories
{
    public interface IStatusRepository
    {
        Task<IEnumerable<Status>> GetAllAsync();
    }
}
