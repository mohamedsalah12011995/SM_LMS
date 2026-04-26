
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.Innovations.Dtos
{
    public class ContactUs:BaseDto<ContactUs,Models.ContactU>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string Name { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public int? CreatedBy { get; set; }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }

        [JsonIgnore]
        public int? ModifiedBy { get; set; }
        public string modifiedBy { set { ModifiedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ModifiedBy); } }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateString { get; set; }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public bool? IsDeleted { get; set; }

        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string FileUrl { get; set; }
        public string FileUrlBase64 { get; set; }
        public string ComplainId { get; set; }
        public string CategoryID { get; set; }
        public string SubCategoryID { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        //Properties for 940 Inquiry
        public string CategoryDesc { get; set; }
        public string SubCategory_Desc { get; set; }
        public string ProblemStatus_Desc { get; set; }
        public string Problem_Desc { get; set; }
        public string District_Desc { get; set; }
        public string Capcha { get; set; }
        public bool? IsFollowUpContactUsUser { get; set; }
        public int? FilterStatusId { get; set; }
        public bool? IsUnderImplementationStatus { get; set; }
        public int? LastStatusId { get; set; }
        public string LastStatusStringAr { get; set; }
        public string LastStatusStringEn { get; set; }
        public bool? TransferFromManager { get; set; }
        public bool? ReturnedToManager { get; set; }
        public string ReferenceNameAr { get; set; }
        public string ReferenceNameEn { get; set; }
        public string LastActionUserName{ get; set; }
        public string FromRefernceAr { get; set; }
        public string FromRefernceEn { get; set; }
        public string Notes { get; set; }
        public int? ProcessingTimesCount { get; set; }
        public string LastFeedbackNote { get; set; }
        public bool? IsImageAttached { get; set; }
        public bool? RejectedByOfficer { get; set; }
        public int?  LastActionUser { get; set; }
        public int? LastActionReference { get; set; }
        public int? ParentLastReference { get; set; }
        public bool? IdeaExist { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        //Properties for Mayor Complain Inquiry

        public List<Tuple<string, string,string>> Replies  { get; set; }
        public List<Tuple<string, string, string>> Comments  { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


    }
}
