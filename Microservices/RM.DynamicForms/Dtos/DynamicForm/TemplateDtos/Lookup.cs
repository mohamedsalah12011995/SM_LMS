using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class Lookup
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public static TypeAdapterConfig ReferencLookupConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<RM.Models.Reference, Lookup>()
                .Config;

        }

        public static TypeAdapterConfig FormTypeLookupConfig()
        {
            return new TypeAdapterConfig()
               .NewConfig<RM.Models.FormType, Lookup>()
               .Map(dest => dest.NameAr, src => src.TypeAr)
               .Map(dest => dest.NameEn, src => src.TypeEn)
                .Config;
        }

        public static TypeAdapterConfig FormsDatasourceLookupConfig()
        {
            return new TypeAdapterConfig()
               .NewConfig<RM.Models.FormsDataSource, Lookup>()
               .Map(dest => dest.NameAr, src => src.TextAr)
               .Map(dest => dest.NameEn, src => src.TextEn)
                .Config;
        }

    }

    public class FormsDataSourceLookup : BaseDto<FormsDataSourceLookup, RM.Models.FormsDataSource>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get(ParentId); } }

        public List<FormsDataSourceLookup> SubList { get; set; } = new List<FormsDataSourceLookup>();


        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<RM.Models.FormsDataSource, FormsDataSourceLookup>()
                .Map(dest => dest.SubList, src => src.InverseParent.Any() ? src.InverseParent.Adapt<List<FormsDataSourceLookup>>() : new List<FormsDataSourceLookup>())

                .Config;

        }


    }


}
