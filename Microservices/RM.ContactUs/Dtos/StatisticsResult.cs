using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.ContactUs.Dtos
{
    public class StatisticsResult
    {

        public int? ThisDay { get; set; }
        public int? ThisWeek { get; set; }
        public int? ThisMonth { get; set; }
        public int? ThisYear { get; set; }
        public int? Total { get; set; }
        public List<YearResult> Years { get; set; } = new List<YearResult>();

    }

        public class YearResult 
    {
        public int Year { get; set; }
        public int YearRate { get; set; }
        public int YearCount { get; set; }
        public List<QuarterResult> Quarters { get; set; } = new List<QuarterResult>();
    }

    public class MonthResult
    {
        public int Month { get; set; }
        public int MonthRate { get; set; }
        public int MonthCount { get; set; }
        public double MonthAvg { get; set; }

        public static TypeAdapterConfig SelectConfig(int month, List<RM.Models.ContactU> group, int yearCount)
        {
            return new TypeAdapterConfig()
                .NewConfig<MonthResult, MonthResult>()
                .Map(dest => dest.Month, src => month)
                .Map(dest => dest.MonthRate, src => 0)
                .Map(dest => dest.MonthCount, src => group.Count())
                .Map(dest => dest.MonthAvg, src => group.Count() > 0 ? (double)(group.Count() * 100) / (double)yearCount : 0.0)

                .Config;
        }

    }

    public class QuarterResult
    {
        public int Quarter { get; set; }
        public int QuarterRate { get; set; }
        public int QuarterCount { get; set; }
        public double QuarterAvg { get; set; }

        public List<MonthResult> Monthes { get; set; }

        public List<StatusResult> StatusResults { get; set; } = new List<StatusResult>();
        public Recommendations Recommendation { get; set; }


        public static TypeAdapterConfig SelectConfig(List<Models.MajorStatus> majorStatuses,int quarter, List<RM.Models.ContactU> group, int yearCount, int month1, List<RM.Models.ContactU> groupMonth1, int month2, List<RM.Models.ContactU> groupMonth2, int month3, List<RM.Models.ContactU> groupMonth3)
        {
            return new TypeAdapterConfig()
                .NewConfig<QuarterResult, QuarterResult>()
                .Map(dest => dest.Quarter, src => quarter)
                .Map(dest => dest.QuarterRate, src => 0)
                .Map(dest => dest.QuarterCount, src => group.Count())
                .Map(dest => dest.QuarterAvg, src => group.Count() > 0 ? (double)(group.Count() * 100) / (double)yearCount : 0.0)
                .Map(dest => dest.Monthes, src => new List<MonthResult>
                {
                   new MonthResult().Adapt<MonthResult>(MonthResult.SelectConfig(month1,groupMonth1,yearCount)),
                   new MonthResult().Adapt<MonthResult>(MonthResult.SelectConfig(month2,groupMonth2,yearCount)),
                   new MonthResult().Adapt<MonthResult>(MonthResult.SelectConfig(month3,groupMonth3,yearCount)),

                })
               .Map(dest => dest.StatusResults, src => group.GroupBy(x => new { x.LastStatusId }, (key, dgroup) => new StatusResult
               {
                   Id = key.LastStatusId,
                   NameAr = dgroup.First().LastStatus != null? dgroup.First().LastStatus.NameAr:string.Empty,
                   NameEn = dgroup.First().LastStatus != null ? dgroup.First().LastStatus.NameEn:string.Empty,
                   Count = dgroup.Count(),
                   Avg = dgroup.Count() > 0 ? (double)(dgroup.Count() * 100) / group.Count() : 0.0,

               }).ToList()
                 .Concat(majorStatuses.Where(x => !group.Select(x => x.LastStatusId).ToList().Contains(x.Id))
                 .ToList().Adapt<List<StatusResult>>()).ToList().OrderBy(x => x.Id))

                .Config;
        }

        public class StatusResult
        {
            [JsonIgnore]
            public int? Id { get; set; }
            public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

            public string NameAr { get; set; }
            public string NameEn { get; set; }

            public int? Count { get; set; } = 0;
            public double? Avg { get; set; } = 0;

        }

        private static Recommendations GetRecommendation(double? rate, RM.Models.FeedbacksDataSource dataSource, List<RM.Models.Recommendations> Recomendations)
        {
            if (rate < 50)
                return Recomendations.FirstOrDefault(x => x.Id == dataSource.LessAverageId).Adapt<Recommendations>(Recommendations.SelectConfig());
            else if (rate == 50)
                return Recomendations.FirstOrDefault(x => x.Id == dataSource.AverageId).Adapt<Recommendations>(Recommendations.SelectConfig());
            else if (rate > 50)
                return Recomendations.FirstOrDefault(x => x.Id == dataSource.AboveAverageId).Adapt<Recommendations>(Recommendations.SelectConfig());
            else return new Recommendations();
        }

    }


  }