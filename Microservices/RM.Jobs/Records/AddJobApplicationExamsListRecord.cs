
namespace RM.Jobs.Records
{
    public record AddJobApplicationExamsListRecord
    {
        public List<JobApplicationExamRecord> JobApplicationExamsList { get; set; }  = new List<JobApplicationExamRecord>();
        public List<JobApplicationRecord> JobApplicationList { get; set; } = new List<JobApplicationRecord>();
    }

    public record JobApplicationExamRecord
    {
        public string examId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public record JobApplicationRecord
    {
        public string ID { get; set; }
        public int? NextState { get; set; }

    }
}
