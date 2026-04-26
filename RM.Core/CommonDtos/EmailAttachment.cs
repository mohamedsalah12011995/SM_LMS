using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Core.CommonDtos
{
    public class EmailAttachment
    {
        public string FileName {  get; set; }
        public byte[] FileBytes { get; set; }
    }
}
