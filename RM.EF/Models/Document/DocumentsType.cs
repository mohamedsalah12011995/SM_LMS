using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class DocumentsType
    {
        public DocumentsType()
        {
            Documents = new HashSet<Document>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
