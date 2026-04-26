using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Models
{

    [Table("Actions", Schema = "Orders")]
    //public class OrderActions
    //{

    //    public int Id { get; set; }

    //    [ForeignKey("Order")]
    //    public int OrderId { get; set; }

    //    [ForeignKey("CreatedByNavigation")]
    //    public int? CreatedBy { get; set; }
    //    public DateTime? CreatedDate { get; set; }

    //    [ForeignKey("Status")]
    //    public int? StatusId { get; set; }

    //    [ForeignKey("FromUser")]
    //    public int? FromUserId { get; set; }


    //    public string Note { get; set; }

    //    public virtual User CreatedByNavigation { get; set; }
    //    public virtual User FromUser { get; set; }
    //    public virtual OrderStatus Status { get; set; }
    //    public virtual Order Order { get; set; }

    //}

    public class OrderActions
    {

        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("Type")]
        public int? TypeId { get; set; }

        public string Note { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual Order Order { get; set; }
        public virtual MajorLookup Type { get; set; }




    }
}
