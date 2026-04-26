

namespace RM.Jobs.Records
{
    public record UpdateJobApplicationExamsListRecord
    {
        public List<UpdateJobApplicationRecord> JobApplicationList { get; set; } = new List<UpdateJobApplicationRecord>();
   
        public string jobCareerId {  get; set; }
        public string examId {  get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public record UpdateJobApplicationRecord
    {
        public string ID { get; set; }

    }
}
