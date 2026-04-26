using Microsoft.AspNetCore.Mvc;
using EmailService.Records;
using EmailService.Services;

namespace EmailService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailServiceController 
    {
        private readonly IEmailServices _emailService;

        public EmailServiceController(IEmailServices emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<bool> SendEmailAsync(EmailInfoRecord RequestedData)
        {
            return await _emailService.SendEmailAsync(RequestedData);
        }
    }
}
