

namespace RM.Entities.Records
{
    public record SaveEntityRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string parentId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string FrontIdentity { get; set; }
        public string BackendIdentity { get; set; }
        public string CmsIdentity { get; set; }
        public string typeId { get; set; }
        public bool? Searchable { get; set; }
        public string FormId { get; set; }


    }
}
