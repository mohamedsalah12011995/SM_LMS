using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Permits.Dtos
{
    public class ProjectUsers
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        [JsonIgnore]
        public int? UserId { get; set; }
        public string userId { set { UserId = Accessor.Set(value); } get { return Accessor.Get<int?>(UserId); } }

        [JsonIgnore]
        public int? ProjectId { get; set; }
        public string projectId { set { ProjectId = Accessor.Set(value); } get { return Accessor.Get<int?>(ProjectId); } }

        public bool? IsEmployee { get; set; }



    }

    public class UserLookup
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string Name { get; set; }
        public string ReferneceNameAr { get; set; }
        public string ReferneceNameEn { get; set; }

    }

    public class UserProject
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }


        public string NameAr { get; set; }
        public string NameEn { get; set; }



    }

}
