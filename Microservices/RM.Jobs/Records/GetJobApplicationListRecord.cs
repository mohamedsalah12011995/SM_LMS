using RM.Core.Helpers;

namespace RM.Jobs.Records
{
    public record GetJobApplicationListRecord
    {

        public string referenceId { get; set; }
        public int? CurrentState { get; set; }
        public string jobCareerId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ExamFromDate { get; set; }
        public DateTime? ExamToDate { get; set; }
        public string BirthDay { get; set; }
        public string gradeId { get; set; }
        public string gendar {  get; set; }
        public int? GradeYear { get; set; }
        public string qualificationId { get; set; }
        public string specialistId { get; set; }
        public string FullName { get; set; }

        public string IdCardNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public int? ExamResultStatusId { get; set; }
        public int? FromResult { get; set; }
        public int? ToResult { get; set; }
        public List<JobLookupRecord> ListOfTags { get; set; } = new List<JobLookupRecord>();
        public List<JobLookupRecord> ListOfSkills { get; set; } = new List<JobLookupRecord>();
        public ApplicationOperation.Pagination Pagination { get; set; }

    }

    public record JobLookupRecord
    {
        public string Name { get; set; }

    }


}
