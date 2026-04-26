using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace RM.Core.CommonDtos
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
    }
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(List<MailboxAddress> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            //To.AddRange(to.Select(x => new MailboxAddress(x)));
            To.AddRange(to.Select(x => new MailboxAddress(x.Name, x.Address)));
            Subject = subject;
            Content = content;
        }
    }
}
