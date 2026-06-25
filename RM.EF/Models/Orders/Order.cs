
#nullable disable

using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("Order", Schema = "Orders")]
    //public class Order
    //{
    //    public Order()
    //    {
    //        Actions = new HashSet<OrderActions>();
    //    }
    //    public int Id { get; set; }
    //    public string Code { get; set; }
    //    public string TitleAr { get; set; }
    //    public string TitleEn { get; set; }
    //    public string Details { get; set; }
    //    public string Url { get; set; }
    //    [ForeignKey("CreatedByNavigation")]
    //    public int? CreatedBy { get; set; }
    //    public DateTime? CreatedDate { get; set; }
    //    public DateTime? ModifiedDate { get; set; }

    //    [ForeignKey("ModifiedByNavigation")]
    //    public int? ModifiedBy { get; set; }
    //    public int? ReferenceId { get; set; }
    //    public int? EntityId { get; set; }
    //    public bool? IsDeleted { get; set; }
    //    public bool? IsActive { get; set; }

    //    [ForeignKey("FirstStatus")]
    //    public int? FirstStatusId { get; set; }
    //    [ForeignKey("LastStatus")]
    //    public int? LastStatusId { get; set; }

    //    [ForeignKey("FirstAction")]
    //    public int? FirstActionId { get; set; }

    //    [ForeignKey("LastAction")]
    //    public int? LastActionId { get; set; }

    //    [ForeignKey("LastUserAction")]
    //    public int? LastActionUser { get; set; }
    //    public DateTime? LastStatusDate { get; set; }

    //    public virtual User CreatedByNavigation { get; set; }
    //    public virtual User ModifiedByNavigation { get; set; }
    //    public virtual Reference Reference { get; set; }
    //    public virtual Entity Entity { get; set; }

    //    public virtual Status FirstStatus { get; set; }
    //    public virtual Status LastStatus { get; set; }
    //    public virtual Actions FirstAction { get; set; }
    //    public virtual Actions LastAction { get; set; }
    //    public virtual User LastUserAction { get; set; }
    //    public virtual ICollection<OrderActions> Actions { get; set; }
    //}

    public class Order
    {
        public Order()
        {
            Actions = new HashSet<OrderActions>();
        }
        public int Id { get; set; }
        [ForeignKey("Reference")]

        public int? ReferenceId { get; set; }
        [ForeignKey("Entity")]

        public int? EntityId { get; set; }
        public string Code { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }

        public virtual Reference Reference { get; set; }
        public virtual Entity Entity { get; set; }

        public virtual ICollection<OrderActions> Actions { get; set; }
    }
}
