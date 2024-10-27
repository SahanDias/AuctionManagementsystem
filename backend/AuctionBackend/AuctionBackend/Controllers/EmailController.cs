using AuctionBackend.Models;
using AuctionBackend.Services.EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Net.Mail;

namespace AuctionBackend.Controllers
{
    [Route("api/Email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;
        public EmailController(IEmailService emailService)
        {
            this.emailService = emailService;
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail (EmailDto request)
        {
            await emailService.SendEmail(request);

            return Ok();


        }
    }
}
