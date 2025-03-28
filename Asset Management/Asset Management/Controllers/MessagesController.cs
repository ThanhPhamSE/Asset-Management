using Asset_Management.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Asset_Management.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _messageService.GetUsers();
            return View(users);
        }

        public async Task<IActionResult> Chat(string selectedUserId)
        {
            var chatViewModel = await _messageService.GetMessages(selectedUserId);
            return View(chatViewModel);
        }
    }
}
