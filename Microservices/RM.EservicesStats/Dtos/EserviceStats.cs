
using System;
using System.Text.Json.Serialization;

namespace RM.EservicesStats.Dtos
{
    public class EserviceStats
    {
       public string? Month { get; set; }
        public string? Year { get; set; }
        public decimal? NewCount { get; set; }
        public string? ReNewCount { get; set; }

    }
}
