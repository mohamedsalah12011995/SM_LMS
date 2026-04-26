using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.IntegrationEntities
{
    public class IdTypeLookups
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }
        [JsonPropertyName("body")]

        public Body Body { get; set; }
    }

    public class IdType
    {
        public string code { get; set; }
        public string nameAr { get; set; }
        public string nameEn { get; set; }

    }
    public class Body
    {
        public Body()
        {
            IdNumberTypes = new List<IdType>();
        }
        [JsonPropertyName("idNumberTypes")]
        public List<IdType> IdNumberTypes { get; set; }
    }
}
