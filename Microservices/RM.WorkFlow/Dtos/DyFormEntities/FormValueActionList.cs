using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.WorkFlow.Dtos.DyFormEntities
{
    public class FormValueActionList
    {
        [JsonIgnore]
        public int? _id { get; set; }
        public string Id { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }

        //[JsonIgnore]
        //public int? _referenceId { get; set; }
        //public string ReferenceId { set { _referenceId = Accessor.Set(value); } get { return Accessor.Get(_referenceId); } }



        //[JsonIgnore]
        //public int? _createdBy { get; set; }
        //public string CreatedBy { set { _createdBy = Accessor.Set(value); } get { return Accessor.Get(_createdBy); } }

        public bool? IsActive { get; set; }
        public string Notes { get; set; }
        public string ActionNameAr { get; set; }
        public string ActionNameEn { get; set; }
        public string StatusStringAr { get; set; }
        public string StatusStringEn { get; set; }
        public string PersonCreatedBy { get; set; }
        public List<FormValueDetailsDto> FormValueDetails { get; set; }



    }
}
