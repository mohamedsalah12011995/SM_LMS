using RM.Core.CommonDtos;

namespace EmailService.Records
{
    public record EmailInfoRecord
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> AttachmentPaths { get; set; }
        public List<EmailAttachment> Attachments { get; set; }
        public List<string> BCCEmailAddresses { get; set; }

    }
}
