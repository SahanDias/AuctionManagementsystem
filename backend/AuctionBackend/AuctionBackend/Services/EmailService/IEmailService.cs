using AuctionBackend.Models;

namespace AuctionBackend.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmail(EmailDto request);
    }
}
