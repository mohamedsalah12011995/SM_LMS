#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RM.Models
{
    public partial class Advertisement
    {
        public int Id { get; set; }
        public int? ReferenceId { get; set; }
        public int? EntityId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string ImageUrl { get; set; }
        public string FileUrl { get; set; }

        public string Url { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsPopup { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsHomeSliderAd { get; set; }
        public int? ActivatedBy { get; set; }
        public int? AdvertisementOrder { get; set; }
        public int? SerialNum { get; set; }
        public string Code { get; set; }
        public string Destination { get; set; }
        [ForeignKey("Region")]
        public int? RegionId { get; set; }
        public virtual User ActivatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual MajorLookup Region { get; set; }
    }
}
