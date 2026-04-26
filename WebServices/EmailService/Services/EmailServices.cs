using MimeKit;
using RM.Core.CommonDtos;
using RM.Core.Services;
using EmailService.Records;
using EmailService.Helper;

namespace EmailService.Services
{
    public class EmailServices : WebBaseService, IEmailServices
    {
        public EmailConfiguration _emailConfig;

        public EmailServices(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, EmailConfiguration emailConfig)
            : base(httpContextAccessor, configuration)
        {
            _emailConfig = emailConfig;
        }

        public async Task<bool> SendEmailAsync(EmailInfoRecord emailInfo)
        {
            if (RequestOwner == null)
                return false;

            var gmailService = await GmailServiceHelper.GetGmailServiceAsync();

            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_emailConfig.DisplayName, _emailConfig.From));
                email.To.Add(new MailboxAddress("", emailInfo.ToEmail));
                email.Subject = emailInfo.Subject;



                var builder = new BodyBuilder { HtmlBody = emailInfo.Body };

                // Add the attachments if provided
                if (emailInfo.AttachmentPaths != null)
                {
                    foreach (var attachmentPath in emailInfo.AttachmentPaths)
                    {
                        if (File.Exists(attachmentPath))
                        {
                            builder.Attachments.Add(attachmentPath);
                        }
                    }
                }

                if (emailInfo.Attachments != null)
                {
                    foreach (var attachment in emailInfo.Attachments)
                    {
                        builder.Attachments.Add(attachment.FileName, attachment.FileBytes);
                    }
                }

                email.Body = builder.ToMessageBody();

                if (emailInfo.BCCEmailAddresses != null)
                {
                    foreach (var bccEmail in emailInfo.BCCEmailAddresses)
                    {
                        email.Bcc.Add(new MailboxAddress(bccEmail, bccEmail));
                    }
                }

                using (var stream = new MemoryStream())
                {
                    email.WriteTo(stream);
                    var rawMessage = Convert.ToBase64String(stream.ToArray())
                        .Replace("+", "-")
                        .Replace("/", "_")
                        .Replace("=", "");

                    var message = new Google.Apis.Gmail.v1.Data.Message { Raw = rawMessage };
                    await gmailService.Users.Messages.Send(message, "me").ExecuteAsync();

                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
