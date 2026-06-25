#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Models
{
    [Table("UsersEntitiesReferences", Schema = "dbo")]

    public class UsersEntityReference
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? EntityId { get; set; }
        public int? ReferenceId { get; set; }

        public bool? Add { get; set; }
        public bool? Edit { get; set; }
        public bool? Delete { get; set; }
        public bool? List { get; set; }
        public bool? Activate { get; set; }
        public bool? Reports { get; set; }
        public bool? View { get; set; }

        public virtual Entity Entity { get; set; }
        public virtual User User { get; set; }
        public virtual Reference Reference { get; set; }

    }
}
