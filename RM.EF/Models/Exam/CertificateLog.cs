using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("CertificateLog", Schema = "Exams")]
    public class CertificateLog
    {
        public int Id { get; set; }

        [ForeignKey("Certificate")]

        public int? CertificateId { get; set; }

        [ForeignKey("UserApplicationExam")]

        public int? UserApplicationExamId { get; set; }

        [ForeignKey("ExternalCourseExam")]

        public int? ExternalCourseExamsId { get; set; }

        [ForeignKey("InternalCourseExam")]

        public int? InternalCourseExamsId { get; set; }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Signature { get; set; }
        public string LogoUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public virtual Certificate Certificate { get; set; }
        public virtual ExternalCourseExams ExternalCourseExam { get; set; }
        public virtual InternalCourseExams InternalCourseExam { get; set; }
        public virtual UserApplicationExam UserApplicationExam { get; set; }

    }
}
