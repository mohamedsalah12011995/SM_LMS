using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Records
{
    public record AddFormValueViewStatistic
    {
        [JsonIgnore]
        public int? FormValueId { get; set; }
        public string formValueId { set { FormValueId = Accessor.Set(value); } get { return Accessor.Get(FormValueId); } }
        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }


        [JsonIgnore]
        public int? UserId { get; set; }
        public string userId { set { UserId = Accessor.Set(value); } get { return Accessor.Get(UserId); } }

        public string UserName { get; set; }

        [JsonIgnore]
        public int? UserReferenceId { get; set; }
        public string userReferenceId { set { UserReferenceId = Accessor.Set(value); } get { return Accessor.Get(UserReferenceId); } }

        public string TextValue { get; set; }
        public string UserReferenceNameAr { get; set; }
        public string UserReferenceNameEn { get; set; }
    }
}
