using AuctionBackend.Models;
using System.Net;
using System.Net.Mail;


namespace AuctionBackend.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration config;
        public EmailService(IConfiguration config)
        {
            this.config = config;
        }
        public async Task SendEmail(EmailDto request)
        {
            var username = config.GetValue<string>("EmailUsername");
            var password = config.GetValue<string>("EmailPassword");
            var host = config.GetValue<string>("EmailHost");
            var port = 587;


            var smtp = new SmtpClient(host, port);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(username,password);

            var message = new MailMessage
            {
                From = new MailAddress(username),  // From address
                Subject = request.Subject,         // Email subject
                Body = request.Body,
            };
            message.To.Add(request.To);
            await smtp.SendMailAsync(message);
            /*
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(config.GetSection("EmailUsername").Value, config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
            */
        }
    }
}
