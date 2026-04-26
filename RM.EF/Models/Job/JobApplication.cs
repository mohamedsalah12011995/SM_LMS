using RM.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("JobApplications", Schema = "Job")]
    public  class JobApplication
    {
        public int Id { get; set; } 
        public string Code { get; set; }
        public string FullName { get; set; }
        public int? ReferenceId { get; set; }
        public DateTime? BirthDay { get; set; }
        public int? Gendar { get; set; }
        public string IdCardNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FileAttachment { get; set; }
        public string TrainingCourses { get; set; }

        [ForeignKey("Qualification")]
        public int? QualificationId { get; set; }

        [ForeignKey("Grade")]
        public int? GradeId { get; set; }

        [ForeignKey("JobCareer")]
        public int? JobCareerId { get; set; }
      
        [ForeignKey("Specialist")]
        public int? SpecialistId { get; set; }
        public int? GradeYear { get; set; }
        public string Skills { get; set; }
        public int? CurrentState { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }

        public DateTime? DeletedDate { get; set; }
        [ForeignKey("DeletedByNavigation")]
        public int? DeletedBy { get; set; }

        [ForeignKey("UpdatedByNavigation")]
        public int? UpdatedBy { get; set; }

        public DateTime? ActivatedDate { get; set; }

        [ForeignKey("ActivatedByNavigation")]
        public int? ActivatedBy { get; set; }
        public virtual Reference Reference { get; set; }
        public  MajorLookup Qualification { get; set; }
        public  JobLookUp Specialist { get; set; }
        public JobLookUp Grade { get; set; }
        public  JobCareer JobCareer { get; set; }
        public  bool? IsSent { get; set; }
        public  string GenderIntegration { get; set; }

        public virtual User ActivatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }

        public virtual ICollection<JobApplicationExams>  JobApplicationExams { get; set; }

    }
}
