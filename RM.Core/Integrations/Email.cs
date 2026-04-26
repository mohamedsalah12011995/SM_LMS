using Irony.Parsing;
using MailKit.Net.Smtp;
using MimeKit;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace RM.Core.Integrations
{
    public class Email
    {
        public static MimeMessage CreateEmailMessage(Message message, EmailConfiguration _emailConfig)
        {
            var emailMessage = new MimeMessage();
            BodyBuilder bodyBuilder = new BodyBuilder();

            emailMessage.From.Add(new MailboxAddress(_emailConfig.DisplayName, _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            bodyBuilder.HtmlBody = message.Content;
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }
        public static bool Send(MimeMessage mailMessage, EmailConfiguration _emailConfig)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, _emailConfig.UseSsl);
                    //client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                    return true;
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    return false;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        public static async Task<bool> SendEmailAsync(string ToEmail, string Subject, string Body, string EmailServiceUrl, string Token, List<string> AttachmentPaths = null, List<EmailAttachment> Attachments = null, List<string> BCCEmailAddresses = null)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request;
            var EmailInfo = new { toEmail = ToEmail, subject = Subject, body = Body , attachmentPaths = AttachmentPaths , attachments = Attachments, BCCEmailAddresses };

            var RequestEncapsulation = JsonSerializer.Serialize(EmailInfo);
            request = new HttpRequestMessage(HttpMethod.Post, EmailServiceUrl);
            request.Headers.Add("Authorization", Token);
            request.Content = new StringContent(RequestEncapsulation, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.SendAsync(request);
            return JsonSerializer.Deserialize<bool>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
