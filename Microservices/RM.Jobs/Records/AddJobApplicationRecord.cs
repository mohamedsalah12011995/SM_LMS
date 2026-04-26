namespace RM.Jobs.Records
{
    public record AddJobApplicationRecord
    {
        public string jobCareerId {  get; set; }
        public string IdCardNumber { get; set; }
        public string BirthDay { get; set; }
        public string idTypeCode { get; set; }
        public bool? birthOfDateIsHijri { get; set; }
        public string currentLang { get; set; }
        public string FullName { get; set; }
        public string FileAttachmentBase64 { get; set; }
        public string referenceId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string gradeId { get; set; }
        public int? GradeYear { get; set; }

        public string qualificationId { get; set; }
        public string specialistId { get; set; }
        public List<JobLookUpRecord> ListOfSkills { get; set; } = new List<JobLookUpRecord>();
        public List<string> ListOfTrainingCourses { get; set; } = new List<string>();
    }

    public record JobLookUpRecord
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
    }
}
