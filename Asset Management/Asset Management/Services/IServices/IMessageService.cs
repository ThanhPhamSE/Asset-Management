using Asset_Management.ViewModels.MessagesViewModels;

namespace Asset_Management.Services.IServices
{
    public interface IMessageService
    {
        Task<IEnumerable<MessagesUsersListViewModel>> GetUsers();
        Task<ChatViewModel> GetMessages(string selectUserId);
    }
}
