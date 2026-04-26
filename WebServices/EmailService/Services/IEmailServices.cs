using EmailService.Records;

namespace EmailService.Services
{
    public interface IEmailServices
    {
        Task<bool> SendEmailAsync(EmailInfoRecord emailInfo);
    }
}
