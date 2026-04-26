namespace RM.WorkFlow.Dtos.IntegrationEntities
{
    public class IntegrationIInputDto
    {
        public string idNumber { get; set; }
        public string idTypeCode { get; set; }

        public string ApplicationNo { get; set; }

        public string birthOfDate { get; set; }
        public string birthOfDateGeorogian { get; set; }
        public string birthOfDateHijri { get; set; }

        public bool birthOfDateIsHijri { get; set; }
        public bool CheckPersonalData { get; set; }
        public bool CheckApplicationNo { get; set; }
        public string EntityUrl { get; set; }


    }
}
