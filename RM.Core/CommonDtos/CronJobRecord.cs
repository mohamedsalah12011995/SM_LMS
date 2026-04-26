using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Core.CommonDtos
{
    public record CronJobRecord
    {
        public int CronTypeId { get; set; }
        public string Data { get; set; }
    }
}
