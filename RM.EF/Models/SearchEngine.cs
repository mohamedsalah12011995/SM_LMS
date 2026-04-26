using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Models
{
    public partial class SearchEngine
    {
        public int Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public int? EntityId { get; set; }
        public string Url { get; set; }
        public DateTime? Date { get; set; }
        public string SearchNameAr { get; set; }
        public string SearchNameEn { get; set; }
    }
}
