namespace RM.Core.Consts
{
    public class FiltersBy
    {
        public int? Id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string countryName { get; set; }
        public string cityName { get; set; }
        public int? clinicId { get; set; }
        public bool? status { get; set; }
        public int? UserTypeId { get; set; }
        public int? pageNumber { get; set; } = 1;
        public int? pageSize { get; set; } = 0;
        // 
        public int? referenceId { get; set; }
        public bool? showInHome { get; set; }
    }
}
