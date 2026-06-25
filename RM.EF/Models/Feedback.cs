#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Models
{
    [Table("Feedback", Schema = "Contact")]
    public class Feedback
    {
        public int Id { get; set; }
        public DateTime EvaluationDate { get; set; }

        [ForeignKey("ContactUs")]
        public int? ContactUsId { get; set; }
        public bool IsPositive { get; set; }
        public string Note { get; set; }
        public bool IsClosed { get; set; }
        public virtual ContactU ContactUs { get; set; }
    }
}
