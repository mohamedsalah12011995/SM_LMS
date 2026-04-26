using RM.Core.CommonDtos;

namespace RM.Exams.Records.Emails
{
    public record EmailInfoRecord
    {
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public List<string> AttachmentPaths { get; set; } = default!;
        public List<EmailAttachment> Attachments { get; set; } = default!;
        public List<string> BCCEmailAddresses { get; set; } = default!;
    }
}
