using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.WorkFlow.Dtos.DyFormEntities
{

    public class FormValuesActions
    {
        [JsonIgnore]
        public int? _id { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }

        [JsonIgnore]
        public int? _jobRoleId { get; set; }
        public string JobRoleId { set { _jobRoleId = Accessor.Set(value); } get { return Accessor.Get(_jobRoleId); } }
        [JsonIgnore]
        public int? _formId { get; set; }
        public string FormId { set { _formId = Accessor.Set(value); } get { return Accessor.Get(_formId); } }


        [JsonIgnore]
        public int? _formValueId { get; set; }
        public string FormValueId { set { _formValueId = Accessor.Set(value); } get { return Accessor.Get(_formValueId); } }


        [JsonIgnore]
        public int? _engineActionJobRoleId { get; set; }
        public string EngineActionJobRoleId { set { _engineActionJobRoleId = Accessor.Set(value); } get { return Accessor.Get(_engineActionJobRoleId); } }

        [JsonIgnore]
        public int? _actionId { get; set; }
        public string ActionId { set { _actionId = Accessor.Set(value); } get { return Accessor.Get(_actionId); } }

        [JsonIgnore]
        public int? _fromUserId { get; set; }
        public string FromUserId { set { _fromUserId = Accessor.Set(value); } get { return Accessor.Get(_fromUserId); } }


        [JsonIgnore]
        public int? _toReferenceId { get; set; }
        public string ToReferenceId { set { _toReferenceId = Accessor.Set(value); } get { return Accessor.Get(_toReferenceId); } }

        [JsonIgnore]
        public int? _createdBy { get; set; }
        public string CreatedBy { set { _createdBy = Accessor.Set(value); } get { return Accessor.Get(_createdBy); } }

        [JsonIgnore]
        public int? _transferToReferenceId { get; set; }
        public string TransferToReferenceId { set { _transferToReferenceId = Accessor.Set(value); } get { return Accessor.Get(_transferToReferenceId); } }

        public DateTime? CreatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }
        public bool? IsReturned { get; set; }
        public bool? IsRejected { get; set; }
        public string Notes { get; set; }
        public string ActionNameAr { get; set; }
        public string ActionNameEn { get; set; }
        public bool AllowEditNote { get; set; }
        public bool? IsClosed { get; set; }
    }
}
