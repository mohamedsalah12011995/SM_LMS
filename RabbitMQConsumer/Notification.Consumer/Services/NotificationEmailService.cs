using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Notification.Consumer.Interfaces;
using Notification.Consumer.Records;
using RabbitMQ.Client;
using RabbitMQ.Core.Dtos;
using RabbitMQ.Core.Services;
using RM.Core.CommonDtos;
using System.Diagnostics;

namespace Notification.Consumer.Services
{
    public class NotificationEmailService : INotificationService, IDisposable
    {
        private readonly RabbitMQConfiguration _rabbitConfiguration;

        public EmailConfiguration _emailConfig;
        public IRabbitMqService _rabbitMqService { get; }
        public RabbitMQ.Client.IModel _model;
        public IConnection _connection;
        private readonly ILogger<NotificationEmailService> logger;


        public NotificationEmailService(IConfiguration configuration,

               EmailConfiguration emailConfig,
              IOptions<RabbitMQConfiguration> options,
              IRabbitMqService rabbitMqService,
              ILogger<NotificationEmailService> logger)

        {
            _emailConfig = emailConfig;
            _rabbitConfiguration = options.Value;
            this.logger = logger;

            _rabbitMqService = rabbitMqService;
            (_connection, _model) = _rabbitMqService.CreateConnection();
            logger.LogWarning("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", "startup", "startup", null);

        }


        private async Task<bool> SendEmailAsync(EmailInfoRecord emailInfo)
        {
            using (ISmtpClient smtp = new SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, _emailConfig.UseSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None);
                    await smtp.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    if (emailInfo.BCCEmailAddresses != null)
                    {
                        await SendByBCCEmailList(emailInfo, smtp);
                    }

                    else
                    {
                        await CreateMimeMessage(emailInfo.ToEmail, emailInfo, smtp);
                    }

                    await smtp.DisconnectAsync(true);

                    logger.LogWarning("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", "Email sent", "Email sent", null);

                    return true;
                }
                catch (Exception ex)
                {
                    await smtp.DisconnectAsync(true);
                    logger.LogError("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", ex.Message, ex.StackTrace, null);
                    return false;
                }
            }
        }

        private async Task SendByBCCEmailList(EmailInfoRecord emailInfo, ISmtpClient smtp)
        {
            foreach (var bccEmail in emailInfo.BCCEmailAddresses)
            {
                await CreateMimeMessage(bccEmail, emailInfo, smtp);
                Thread.Sleep(1000);
            }

        }

        private async Task CreateMimeMessage(string emailTo, EmailInfoRecord emailInfo, ISmtpClient smtp)
        {
            using (MimeMessage msg = new MimeMessage())
            {
                var builder = new BodyBuilder { HtmlBody = emailInfo.Body };

                msg.From.Add(new MailboxAddress(_emailConfig.DisplayName, _emailConfig.From));
                msg.To.Add(new MailboxAddress("", emailTo));
                msg.Subject = emailInfo.Subject;
                msg.Body = builder.ToMessageBody();

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

                try
                {
                    await smtp.SendAsync(msg);

                }
                catch (Exception ex)
                {
                    logger.LogError("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", ex.Message, ex.StackTrace, null);
                    Thread.Sleep(1000);

                }
            }
        }



        public async Task ReadMessage(CancellationToken cancellationToken)
        {
            await _rabbitMqService.ConsumeMessageAsync<EmailInfoRecord>(_model, _rabbitConfiguration.Queue, true, async emailInfo =>
            {
                var result = await SendEmailAsync(emailInfo);
                return new ResultDto { IsValid = result };
            }, cancellationToken);
        }

        // test 
        public async Task<bool> Process(string msg)
        {

            await Task.Delay(1000);
            bool result = !string.IsNullOrEmpty(msg);
            Debug.WriteLine(msg);
            return result;
        }

        public void Dispose()
        {
            _model?.Close();
            _model?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
