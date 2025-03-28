using Asset_Management.Models;

namespace Asset_Management.Helpers
{
    public interface ICurrentUserService
    {
        string UserId {  get; }
        Task<Users> GetUser();
    }
}
