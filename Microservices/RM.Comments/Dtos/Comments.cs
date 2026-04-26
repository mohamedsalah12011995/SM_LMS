using DocumentFormat.OpenXml.Wordprocessing;
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.Comments.Dtos
{
    public class Comments:BaseDto<Comments, Models.Comment>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        [JsonIgnore]
        public int? AdvertismentId { get; set; }
        public string advertismentId { set { AdvertismentId = Accessor.Set(value); } get { return Accessor.Get<int?>(AdvertismentId); } }

        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Text { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public DateTime? CreatedDate { get; set; }
        [JsonIgnore]

        public int? CreatedBy { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsApproved { get; set; }
        [JsonIgnore]
        public int? ApprovedBy { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string approvedBy { set { ApprovedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ApprovedBy); } }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? UpdatedDate { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ReplyText { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? ReplyDate { get; set; }
        [JsonIgnore]
        public int? RepliedBy { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string repliedBy { set { RepliedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(RepliedBy); } }

        [JsonIgnore]
        public int? ItemId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string itemId { set { ItemId = Accessor.Set(value); } get { return Accessor.Get<int?>(ItemId); } }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ItemUrl { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CommenterName { get; set; }
        public string Email { get; set; }
        public bool? IsAgreeTerms { get; set; }
        public string CreatedDateString { get; set; }
        public string ReplyDateString { get; set; }
        public string RepliedUser { get; set; }
        public string IsApprovedString { get; set; }
        public string IsApprovedStringEn { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.Comment> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Comment>(true);

            filter.And(u => u.ReferenceId == ReferenceId);
            filter.And(u => u.EntityId == EntityId);

            if (ItemId.HasValue)
                filter.And(u => u.ItemId == ItemId);

            if (!string.IsNullOrEmpty(ItemUrl))
                filter.And(u => u.ItemUrl == ItemUrl);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(CommenterName))
                filter.And(u => u.CommenterName.Contains(CommenterName));
            if (!string.IsNullOrEmpty(Email))
                filter.And(u => u.Email.Contains(Email));
            if (!string.IsNullOrEmpty(Text))
                filter.And(u => u.Text.Contains(Text));

            if (IsApproved.HasValue)
                filter.And(u => u.IsApproved == IsApproved);

            return filter;

        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Comment, Comments>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.RepliedUser, src => src.RepliedByNavigation != null ? src.RepliedByNavigation.Name : string.Empty)
                .Map(dest => dest.ReplyDateString, src => src.RepliedByNavigation != null ? src.ReplyDate.GetValueOrDefault().ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.IsApprovedString, src => src.IsApproved.HasValue ? src.IsApproved == true ? "مقبول" : "مرفوض" : "بالانتظار")
                .Map(dest => dest.IsApprovedStringEn, src => src.IsApproved.HasValue ? src.IsApproved == true ? "Approved" : "Not Approved" : "Waiting")
                   .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Comments, Models.Comment>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsApproved, src => false)
                .Config;
        }
    }
}