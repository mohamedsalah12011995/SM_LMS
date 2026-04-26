using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("Actions", Schema = "Contact")]
    public class Actions
    {
        public Actions()
        {
            ActionFiles = new HashSet<ActionFiles>();

        }

        public int Id { get; set; }

        [ForeignKey("Complain")]
        public int ContactId { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("Status")]
        public int? StatusId { get; set; }

        [ForeignKey("FromUser")]
        public int? FromUserId { get; set; }


        public int? ToReferenceId { get; set; }
        public string Note { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User FromUser { get; set; }
        public virtual Status Status { get; set; }
        public virtual Reference ToReference { get; set; }
        public virtual ContactU Complain { get; set; }

        public ICollection<ActionFiles> ActionFiles { get; set; } = new List<ActionFiles>();

    }
}
