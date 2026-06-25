#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class EidFiterRequest
    {
        public int Id { get; set; }
        public long? Code { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? RequesterType { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public string LocationDesc { get; set; }
        public string Description { get; set; }
        public string RequiredSupport { get; set; }
        public int? DistrictId { get; set; }





    }
}
