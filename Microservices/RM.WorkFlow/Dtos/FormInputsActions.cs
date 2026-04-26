using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.WorkFlow.Dtos
{

    public class FormInputsActions
    {
        [JsonIgnore]
        public int? _id { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }

        [JsonIgnore]
        public int? _formId { get; set; }
        public string FormId { set { _formId = Accessor.Set(value); } get { return Accessor.Get(_formId); } }

        [JsonIgnore]
        public int? _formInputId { get; set; }
        public string FormInputId { set { _formInputId = Accessor.Set(value); } get { return Accessor.Get(_formInputId); } }

        [JsonIgnore]
        public int? _actionId { get; set; }
        public string ActionId { set { _actionId = Accessor.Set(value); } get { return Accessor.Get(_actionId); } }

        [JsonIgnore]
        public int? _engineActionJobRoleId { get; set; }
        public string EngineActionJobRoleId { set { _engineActionJobRoleId = Accessor.Set(value); } get { return Accessor.Get(_engineActionJobRoleId); } }

        [JsonIgnore]
        public int? _createdBy { get; set; }
        public string CreatedBy { set { _createdBy = Accessor.Set(value); } get { return Accessor.Get(_createdBy); } }

        [JsonIgnore]
        public int? _updatedBy { get; set; }
        public string UpdatedBy { set { _updatedBy = Accessor.Set(value); } get { return Accessor.Get(_updatedBy); } }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }


        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string DeletedDateString { get; set; }
        public string ActivatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string StatusString { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }



    }
}
