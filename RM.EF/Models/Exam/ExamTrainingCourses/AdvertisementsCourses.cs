#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("AdvertisementsCourses", Schema = "ExamTraining")]
    public class AdvertisementsCourses
    {
        public int Id { get; set; }

        public int? CourseAdvertisementId { get; set; }

        public int? TrainingCourseScheduleId { get; set; }
        public int? TrainingCourseTypeId { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }



        public virtual TrainingCourseType TrainingCourseType { get; set; }
        public virtual TrainingCourseSchedule TrainingCourseSchedule { get; set; }



    }
}
