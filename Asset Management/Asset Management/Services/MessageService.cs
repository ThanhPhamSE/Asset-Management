using Asset_Management.Data;
using Asset_Management.Helpers;
using Asset_Management.Services.IServices;
using Asset_Management.ViewModels.MessagesViewModels;
using Microsoft.EntityFrameworkCore;

namespace Asset_Management.Services
{
    public class MessageService:IMessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public MessageService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
         
        public async Task<ChatViewModel> GetMessages(string selectUserId)
        {
            var currentUserId = _currentUserService.UserId;
            var selectedUser = await _context.Users.FirstOrDefaultAsync(i => i.Id == selectUserId);
            var selectedUserName = "";
            if(selectedUser != null)
            {
                selectedUserName = selectedUser.UserName;
            }

            var chatViewModel = new ChatViewModel()
            {
                CurrentUserId = currentUserId,
                ReceiverId = selectUserId,
                ReceiverUserName = selectedUserName
            };

            var messages = await _context.Messages.Where(i => (i.SenderId == currentUserId
                                                              || i.SenderId == selectUserId)
                                                              && (i.ReceiverId == currentUserId
                                                              || i.ReceiverId == selectUserId)).Select(i => new UserMessagesListViewModel
            {
                Id = i.Id,
                Text = i.Text,
                Date = i.Date.ToShortDateString(),
                Time = i.Date.ToShortTimeString(),
                IsCurrentUserSentMessage = i.SenderId == currentUserId
            }).ToListAsync();
            chatViewModel.Messages = messages;
            return chatViewModel;
        }

        public async Task<IEnumerable<MessagesUsersListViewModel>> GetUsers()
        {
            var currentUserId = _currentUserService.UserId;
            var users = await _context.Users.Where(i => i.Id != currentUserId).Select(i => new MessagesUsersListViewModel()
            {
                Id = i.Id,
                UserName = i.UserName,
                LastMessage = _context.Messages.Where(m => (m.SenderId == currentUserId || m.SenderId == i.Id) && (m.ReceiverId == currentUserId || m.ReceiverId == i.Id)).OrderByDescending(m => m.Date).Select(m => m.Text).FirstOrDefault() ?? string.Empty
            }).ToListAsync();
            return users;
        }
    }
}
