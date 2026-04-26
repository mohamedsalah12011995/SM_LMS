using LinqKit;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.Exams.Certifications
{

    public class CertificateListInputDto : BaseDto<CertificateListInputDto, Models.Certificate>, IFilteration<Models.Certificate>
    {
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.Certificate> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Certificate>(true);

            filter.And(u => u.ReferenceId == ReferenceId);


            if (!string.IsNullOrEmpty(TitleAr))
                filter.And(u => u.TitleAr.Contains(TitleAr));

            if (!string.IsNullOrEmpty(TitleEn))
                filter.And(u => u.TitleEn.Contains(TitleEn));

            filter.And(u => u.IsDeleted != true);

            return filter;
        }




    }
}
