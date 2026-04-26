
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System;
using System.Text.Json.Serialization;

namespace RM.ContactUs.Dtos
{
    public class Feedback:BaseDto<Feedback,Models.Feedback>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ContactUsId { get; set; }
        public string id { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string contactUsId { set { ContactUsId = Accessor.Set(value); } get { return Accessor.Get<int?>(ContactUsId); } }
        public DateTime? EvaluationDate { get; set; }
        public string EvaluationDateString { get; set; }
        public bool? IsPositive { get; set; }
        public string StatusAr { get; set; }
        public string StatusEn { get; set; }
        public string Note { get; set; }
        public bool? IsClosed { get; set; }

        public static TypeAdapterConfig SelectConfig(bool FullDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Feedback, Feedback>()
                .Map(dest => dest.EvaluationDateString, src =>  FullDate ?
                 src.EvaluationDate.ToString("yyyy-MM-dd h:mm:ss tt") : src.EvaluationDate.ToString("yyyy-MM-dd"))

                .Config;
        }
    }
}
