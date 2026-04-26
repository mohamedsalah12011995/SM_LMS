using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System;
using System.Text.Json.Serialization;

namespace RM.Innovations.Dtos
{
    public class IdeaComments : BaseDto<IdeaComments, Models.IdeaComment>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string Text { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public DateTime? CreatedDate { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsApproved { get; set; }

        public string IsApprovedString { get; set; }
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
        public int? IdeaId { get; set; }
        public string ideaId { set { IdeaId = Accessor.Set(value); } get { return Accessor.Get<int?>(IdeaId); } }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CommenterName { get; set; }
        public string Email { get; set; }
        public bool? IsAgreeTerms { get; set; }
        public string CreatedDateString { get; set; }
        public string ReplyDateString { get; set; }
        public string RepliedUser { get; set; }
        public string Capcha { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.IdeaComment> Filteration()
        {
            var filter = PredicateBuilder.New<Models.IdeaComment>(true);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (!string.IsNullOrEmpty(CommenterName))
                filter.And(u => u.CommenterName.Contains(CommenterName));

            if (!string.IsNullOrEmpty(Email))
                filter.And(u => u.Email.Contains(Email));

            if (!string.IsNullOrEmpty(Text))
                filter.And(u => u.Text.Contains(Text));

            if (IsApproved.HasValue)
                filter.And(u => u.IsApproved == IsApproved);

            filter.And(u => u.IdeaId == IdeaId);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.IdeaComment, IdeaComments>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.RepliedUser, src => src.RepliedByNavigation != null ? src.RepliedByNavigation.Name : null)
                .Map(dest => dest.ReplyDateString, src => src.RepliedByNavigation != null ? src.ReplyDate.GetValueOrDefault().ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.IsApprovedString, src => src.IsApproved.HasValue ? src.IsApproved == true ? "مقبول" : "مرفوض" : "بالانتظار")

                 .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<IdeaComments, Models.IdeaComment>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Config;
        }
    }
}
