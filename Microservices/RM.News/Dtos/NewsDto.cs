using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.News.Interfaces;
using System.Text.Json.Serialization;


namespace RM.News.Dtos
{

    public class NewsDto : BaseDto<NewsDto, Models.News>, IFilterationNews<Models.News>
    {
        public NewsDto()
        {
            PublishEntity = new List<Reference>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? ApprovedBy { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string approvedBy { set { ApprovedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ApprovedBy); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string NewsSourceAr { get; set; }
        public string NewsSourceEn { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string NewsContentAr { get; set; }
        public string NewsContentEn { get; set; }
        public string ThumpPic { get; set; }
        public string OriginalPic { get; set; }
        public string OriginalPicBase64 { get; set; }
        public string StatusString { get; set; }
        public string NewsDateH { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string PersonCreatedBy { get; set; }
        public string TagsAr { get; set; }
        public string TagsEn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public bool? ShowInHome { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? NewsDate { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string NewsDateString { get; set; }
        public string ActivatedDateString { get; set; }
        public string DeletedDateString { get; set; }
        public bool? IsHomePage { get; set; }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public NewsDto PrevNews { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public NewsDto NextNews { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<NewsDto> RelatedNews { get; set; }
        public List<TagsDto> ListOfTags { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }
        public bool IntranetRequestType { get; set; }
        public List<Reference> PublishEntity { get; set; }

        public ExpressionStarter<Models.News> Filteration(List<int> publishNews)
        {
            var filter = PredicateBuilder.New<Models.News>(true);

            if (publishNews != null)
            {
                foreach (var id in publishNews)
                {
                    int tempId = id; // To avoid closure issue
                    filter.Or(u => u.Id == tempId);
                }
                filter.Or(u => u.ReferenceId == ReferenceId);
            }
            else
                filter.And(u => u.ReferenceId == ReferenceId);

            //if (ReferenceId.HasValue)
            //    filter.And(u => u.ReferenceId == ReferenceId || publishNews.Contains(u.Id));

            if (EntityId.HasValue)
                filter.And(u => u.EntityId == EntityId);

            if (!string.IsNullOrEmpty(PersonCreatedBy))
                filter.And(u => u.CreatedByNavigation.Name.Contains(PersonCreatedBy));

            if (!string.IsNullOrEmpty(PersonUpdatedBy))
                filter.And(u => u.UpdatedByNavigation.Name.Contains(PersonUpdatedBy));

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(TitleAr))
                filter.And(u => u.TitleAr.Contains(TitleAr));

            if (!string.IsNullOrEmpty(TitleEn))
                filter.And(u => u.TitleEn.Contains(TitleEn));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);
            return filter;
        }


        public static TypeAdapterConfig SelectConfig(Enums.UsersRoles RequestUserRole, string newsThumbsGetPath, string newsImagesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.News, NewsDto>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.NewsDateString, src => src.NewsDate.HasValue ? src.NewsDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.ThumpPic, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.OriginalPic) ? $"{newsThumbsGetPath}/{src.OriginalPic}" : null)
                .Map(dest => dest.OriginalPic, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.OriginalPic) ? $"{newsImagesGetPath}/{src.OriginalPic}" : null)
                .Map(dest => dest.StatusString, src => RequestUserRole != Enums.UsersRoles.NormalUser ? src.IsActive == true ? "فعال" : "غير فعال" : null)

                .Config;

        }

        public static TypeAdapterConfig SelectConfigNewsDetail(bool IsLocal, string GetPath, string IntranetGetPath, string newsThumbsGetPath, string newsImagesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.News, NewsDto>()
                .Map(dest => dest.NewsContentAr, src => Strings.ReplaceUrlsInContent(IsLocal, src.NewsContentAr, GetPath, IntranetGetPath))
                .Map(dest => dest.NewsContentEn, src => Strings.ReplaceUrlsInContent(IsLocal, src.NewsContentEn, GetPath, IntranetGetPath))
                .Map(dest => dest.ThumpPic, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.OriginalPic) ? $"{newsThumbsGetPath}/{src.OriginalPic}" : null)
                .Map(dest => dest.OriginalPic, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.OriginalPic) ? $"{newsImagesGetPath}/{src.OriginalPic}" : null)
                .Map(dest => dest.NewsDateString, src => src.NewsDate.HasValue ? src.NewsDate.Value.ToString("yyyy-MM-dd") : string.Empty)



                .Config;

        }

        public TypeAdapterConfig UpdateConfig(int? userId, string newsImagesSavePath, string newsThumbsSavePath)
        {
            return new TypeAdapterConfig()
                .NewConfig<NewsDto, Models.News>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)

                .Map(dest => dest.TagsAr, src => Strings.ConvertListToString(src.ListOfTags.Select(x => x.NameAr).ToList(), "$"))
                .Map(dest => dest.TagsEn, src => Strings.ConvertListToString(src.ListOfTags.Select(x => x.NameEn).ToList(), "$"))


                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId, string newsImagesSavePath, string newsThumbsSavePath)
        {
            return new TypeAdapterConfig()
                  .NewConfig<NewsDto, Models.News>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IsActive, src => false)

                .Map(dest => dest.TagsAr, src => Strings.ConvertListToString(src.ListOfTags.Select(x => x.NameAr).ToList(), "$"))
                .Map(dest => dest.TagsEn, src => Strings.ConvertListToString(src.ListOfTags.Select(x => x.NameEn).ToList(), "$"))




                .Config;
        }




    }


}
