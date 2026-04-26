using RM.Core.Helpers;

namespace RM.FAQs.Records
{
    public record GetFAQListRecord
    {
        public string referenceId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string QuestionAr { get; set; }
        public string QuestionEn { get; set; }
        public string AnswerAr { get; set; }
        public string AnswerEn { get; set; }

        public string PersonUpdatedBy { get; set; }
        public string PersonCreatedBy { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
