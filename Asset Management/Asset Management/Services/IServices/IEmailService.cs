using Asset_Management.Models;

namespace Asset_Management.Services.IServices
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
