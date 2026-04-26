
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System;
using System.Text.Json.Serialization;

namespace RM.Feedbacks.Dtos
{
    public class Feedbacks
    {

        [JsonIgnore]
        public int? Id { get; set; }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }

        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }


        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }

        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }


        public string PersonUpdatedBy { get; set; }
        public string PersonCreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string UpdatedDateString { get; set; }
        public string CreatedDateString { get; set; }

        public List<FeedbacksDataSource> FeedbacksDataSources { get; set; } = new List<FeedbacksDataSource>();

        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.Feedbacks> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Feedbacks>(true);

            if (ReferenceId.HasValue)
                filter.And(u => u.ReferenceId == ReferenceId);


            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(TitleAr))
                filter.And(u => u.TitleAr.Contains(TitleAr));
            if (!string.IsNullOrEmpty(TitleEn))
                filter.And(u => u.TitleEn.Contains(TitleEn));


            if (!string.IsNullOrEmpty(PersonCreatedBy))
                filter.And(u => u.CreatedByNavigation.Name.Contains(PersonCreatedBy));
            if (!string.IsNullOrEmpty(PersonUpdatedBy))
                filter.And(u => u.UpdatedByNavigation.Name.Contains(PersonUpdatedBy));


            filter.And(u => u.IsDeleted != true);

            return filter;

        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Feedbacks, Feedbacks>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.FeedbacksDataSources, src => src.FeedbacksDataSources != null ? src.FeedbacksDataSources.Where(x => x.IsDeleted != true).Adapt<List<Dtos.FeedbacksDataSource>>(Dtos.FeedbacksDataSource.SelectConfig()) : new List<FeedbacksDataSource>())
                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Feedbacks, Models.Feedbacks>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Ignore(dest => dest.FeedbacksDataSources)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Feedbacks, Models.Feedbacks>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Ignore(dest => dest.FeedbacksDataSources)

                .Config;
        }
    }
}
