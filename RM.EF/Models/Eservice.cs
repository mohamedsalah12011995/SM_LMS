#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RM.Models
{
    public partial class Eservice
    {
        public Eservice()
        {
            InverseParent = new HashSet<Eservice>();
        }

        public int Id { get; set; }
        public int? ReferenceId { get; set; }
        public int? EntityId { get; set; }
        public int? ParentId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Path { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string DeliveryChannelAr { get; set; }
        public string DeliveryChannelEn { get; set; }
        public string ServiceLink { get; set; }
        public string CategoryAr { get; set; }
        public string CategoryEn { get; set; }
        public string TypeAr { get; set; }
        public string TypeEn { get; set; }
        public string Maturity { get; set; }
        public string FeesAr { get; set; }
        public string FeesEn { get; set; }
        public string FeesInk { get; set; }
        public string PaymentChannelAr { get; set; }
        public string PaymentChannelEn { get; set; }
        public string ExecutionTime { get; set; }
        public string ExecutionTimeEn { get; set; }
        public string ContactEn { get; set; }
        public string ContactAr { get; set; }
        public string RequirementsAr { get; set; }
        public string RequirementsEn { get; set; }
        public string AttachmentsAr { get; set; }
        public string AttachmentsEn { get; set; }
        public string ProceduresAr { get; set; }
        public string ProceduresEn { get; set; }
        public string UserGuid { get; set; }
        public bool? IsRoot { get; set; }
        public bool? IsDeleted { get; set; }
        public int? DeletedBy { get; set; }
        public bool? IsActive { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ActivatedBy { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public string IconUrl { get; set; }


        [ForeignKey("LessAverage")]
        public int? LessAverageId { get; set; }
        public Recommendations LessAverage { get; set; }


        [ForeignKey("Average")]
        public int? AverageId { get; set; }
        public Recommendations Average { get; set; }


        [ForeignKey("AboveAverage")]
        public int? AboveAverageId { get; set; }
        public Recommendations AboveAverage { get; set; }

        public virtual User ActivatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Eservice Parent { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<Eservice> InverseParent { get; set; }
    }
}
