
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.Documents.Dtos
{
    public class Documents:BaseDto<Documents,Models.Document>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? TypeId { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        [JsonIgnore]
        public int? RootParentId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(this.Id); } }
        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(this.TypeId); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(this.ReferenceId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(this.CreatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(this.DeletedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(this.UpdatedBy); } }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get<int?>(this.ParentId); } }

        
        public string rootParentId { set { RootParentId = Accessor.Set(value); } get { return Accessor.Get<int?>(this.RootParentId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(this.EntityId); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(this.ActivatedBy); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string Url { get; set; }
        public string UrlBase64 { get; set; }
        public string CreatedPerson { get; set; }
        public string UpdatedPerson { get; set; }
        public string StatusString { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsFinalRoot { get; set; }
        public bool? IsActive { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }


        public List<Documents> ListOfDocuments { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Document> DocumentsFilteration()
        {
            var filter = PredicateBuilder.New<Document>(true);

            filter.And(u => u.ReferenceId == ReferenceId);

            if(EntityId.HasValue)
            filter.And(u => u.EntityId == EntityId);

            filter.And(u => u.IsFinalRoot == true);

            if (ParentId.HasValue)
                filter.And(u => u.ParentId == ParentId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));
            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);
            filter.And(u => u.IsDeleted != true);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig(string DocumentsGetPath, bool FullDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<Document, Documents>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? FullDate ?
                 src.CreatedDate.Value.ToString("yyyy-MM-dd h:mm:ss tt") : src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)

                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? FullDate ?
                 src.UpdatedDate.Value.ToString("yyyy-MM-dd h:mm:ss tt") : src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)

                .Map(dest => dest.ListOfDocuments, src => src.InverseParent!=null? src.InverseParent.Where(v => v.IsDeleted == false && src.IsActive == true).Adapt<List<Documents>>(Documents.SelectConfig(DocumentsGetPath,false)):new List<Documents>())
                .Map(dest => dest.Url, src => DocumentsGetPath +"/"+src.Url)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير فعال")
                .Map(dest => dest.CreatedPerson, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name:string.Empty)
                .Map(dest => dest.UpdatedPerson, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name:string.Empty)
                .Ignore(x => x.rootParentId)
                .Map(dest => dest.RootParentId, src => src.Parent != null ? src.Parent.ParentId:0)

                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Documents, Document>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Documents, Document>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IsActive, src => false)
                .Map(dest => dest.IsFinalRoot, src => true)
                .Config;
        }

    }
}
