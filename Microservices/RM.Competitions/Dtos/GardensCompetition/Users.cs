
using RM.Core.Helpers;

namespace RM.Competitions.Dtos
{
    public class Users
    {
        internal int? _id { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get<int?>(_id); } }
        public string OTPCode { get; set; }
        public string Phone { get; set; }
    }
}
