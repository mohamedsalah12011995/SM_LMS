namespace RM.Models;


public class Course 
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string TitleAr { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;

    public string DescriptionAr { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public string? IntroVideoUrl { get; set; }

    public int CategoryId { get; set; }

    public int Level { get; set; }

    public string LanguageCode { get; set; } = "ar";

    public int EstimatedDurationMinutes { get; set; }

    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }

    public bool AllowCertificate { get; set; }

    public int Status { get; set; }

    public int? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public int? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }

    public int? ActivatedBy { get; set; }
    public DateTime? ActivatedDate { get; set; }
    public int? EntityId { get; set; }
    public int? ReferenceId { get; set; }

    public virtual CourseCategory Category { get; set; } 
    public virtual User ActivatedByNavigation { get; set; }
    public virtual User CreatedByNavigation { get; set; }
    public virtual User UpdatedByNavigation { get; set; }
    public virtual User DeletedByNavigation { get; set; }
    public virtual Entity Entity { get; set; }
    public virtual Reference Reference { get; set; }

    public ICollection<CourseSection> Sections { get; set; }
        = new List<CourseSection>();

    public ICollection<CourseInstructor> CourseInstructors { get; set; }
        = new List<CourseInstructor>();

    public ICollection<CourseTagMapping> CourseTagMappings { get; set; }
        = new List<CourseTagMapping>();

    public ICollection<CoursePrerequisite> CoursePrerequisites { get; set; }
        = new List<CoursePrerequisite>();

    public ICollection<CourseTargetAudience> CourseTargetAudiences { get; set; }
        = new List<CourseTargetAudience>();

    public ICollection<CourseLearningOutcome> CourseLearningOutcomes { get; set; }
        = new List<CourseLearningOutcome>();



}