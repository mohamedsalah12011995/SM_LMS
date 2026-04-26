using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("JobCareers", Schema ="Job")]
    public class JobCareer
    {
        public int Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string JobLocationAr { get; set; }
        public string JobLocationEn { get; set; }
        public string JobCondationsAr { get; set; }
        public string JobCondationsEn { get; set; }
        public string Tags { get; set; }
        public string Skills { get; set; }
        public string Specifications { get; set; }
        public string SpecificationsEn { get; set; }
        public int? MaxLimit { get; set; }
        [ForeignKey("JobAdvertisement")]
        public int? JobAdvertisementId { get; set; }
        [ForeignKey("Qualification")]
        public int? QualificationId { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public int? Type { get; set; }
        public bool? IsNoticeBeneficiaries { get; set; }
        public  MajorLookup Qualification { get; set; }
        public  JobAdvertisement JobAdvertisement { get; set; }

    }
}
