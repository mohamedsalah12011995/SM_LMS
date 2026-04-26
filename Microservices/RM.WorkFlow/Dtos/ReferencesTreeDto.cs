using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.WorkFlow.Dtos
{
    public class ReferencesTreeDto
    {
        [JsonIgnore]
        public int? _Id { get; set; }
        [JsonIgnore]
        public int? _referencesMajorId { get; set; }
        [JsonIgnore]
        public int? _referenceId { get; set; }
        [JsonIgnore]
        public int? _createdBy { get; set; }
        [JsonIgnore]
        public int? _updatedBy { get; set; }
        public string ID { set { _Id = Accessor.Set(value); } get { return Accessor.Get(_Id); } }
        public string ReferencesMajorId { set { _referencesMajorId = Accessor.Set(value); } get { return Accessor.Get(_referencesMajorId); } }
        public string ReferenceId { set { _referenceId = Accessor.Set(value); } get { return Accessor.Get(_referenceId); } }

        public string Label { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }


        [JsonIgnore]
        public int? _parentId { get; set; }
        public string ParentId { set { _parentId = Accessor.Set(value); } get { return Accessor.Get(_parentId); } }
        public List<ReferencesTreeDto> Children { get; set; }
        public List<ReferenceJobRoleDto> ReferenceJobRole { get; set; }
    }

    public class ReferenceJobRoleDto
    {
        [JsonIgnore]
        public int? _id { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }
        [JsonIgnore]
        public int? _referenceId { get; set; }
        public string ReferenceId { set { _referenceId = Accessor.Set(value); } get { return Accessor.Get(_referenceId); } }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
