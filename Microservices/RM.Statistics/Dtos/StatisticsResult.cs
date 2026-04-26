using Mapster;
using RM.Core.Helpers;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.Statistics.Dtos
{
    public class StatisticsResult
    {
        [JsonIgnore]
        public int? ItemId { get; set; }
        public string itemId { set { ItemId = Accessor.Set(value); } get { return Accessor.Get<int?>(ItemId); } }

        public string ArticleNameAr { get; set; }
        public string ArticleNameEn { get; set; }
        public string FrontIdentity { get; set; }
        public int? ViewsCount { get; set; }
        public int? RatingValue { get; set; }
        public string Url { get; set; }

    }

    public class InteractionStatisticsResult
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

        public static TypeAdapterConfig SelectConfig(bool isAvg, int month, List<RM.Models.InteractionStatistic> group, int yearSum)
        {
            return new TypeAdapterConfig()
                .NewConfig<MonthResult, MonthResult>()
                .Map(dest => dest.Month, src => month)
                .Map(dest => dest.MonthRate, src => 0)
                .Map(dest => dest.MonthCount, src => isAvg ? group.Count() > 0 ? group.Sum(x => x.Value) / group.Count() : 0 : group.Sum(x => x.Value))
                .Map(dest => dest.MonthAvg, src => group.Sum(x => x.Value) > 0 ? (double)(group.Sum(x => x.Value) * 100) / (double)yearSum : 0.0)

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

        public static TypeAdapterConfig SelectConfig(bool isAvg, int quarter, List<RM.Models.InteractionStatistic> group, int yearCount, int month1, List<RM.Models.InteractionStatistic> groupMonth1, int month2, List<RM.Models.InteractionStatistic> groupMonth2, int month3, List<RM.Models.InteractionStatistic> groupMonth3)
        {
            return new TypeAdapterConfig()
                .NewConfig<QuarterResult, QuarterResult>()
                .Map(dest => dest.Quarter, src => quarter)
                .Map(dest => dest.QuarterRate, src => 0)
                .Map(dest => dest.QuarterCount, src => isAvg ? group.Count() > 0?  group.Sum(x => x.Value) / group.Count() :0 : group.Sum(x => x.Value))
                .Map(dest => dest.QuarterAvg, src => group.Sum(x => x.Value) > 0 ? (double)(group.Sum(x => x.Value) * 100) / (double)yearCount : 0.0)
                .Map(dest => dest.Monthes, src => new List<MonthResult>
                {
                   new MonthResult().Adapt<MonthResult>(MonthResult.SelectConfig(isAvg,month1,groupMonth1,yearCount)),
                   new MonthResult().Adapt<MonthResult>(MonthResult.SelectConfig(isAvg, month2,groupMonth2,yearCount)),
                   new MonthResult().Adapt<MonthResult>(MonthResult.SelectConfig(isAvg, month3,groupMonth3,yearCount)),

                })
               //.Map(dest => dest.StatusResults, src => group.GroupBy(x => new { x.InteractionStatisticsType }, (key, dgroup) => new StatusResult
               //{
               //    Id = key.InteractionStatisticsType,
               //    NameAr = dgroup.First().InteractionStatisticsTypeNavigation != null ? dgroup.First().InteractionStatisticsTypeNavigation.NameAr : string.Empty,
               //    NameEn = dgroup.First().InteractionStatisticsTypeNavigation != null ? dgroup.First().InteractionStatisticsTypeNavigation.NameEn : string.Empty,
               //    Count =  dgroup.Sum(x=>x.Value),
               //    Avg = dgroup.Sum(x => x.Value) > 0 ? (double)(dgroup.Sum(x => x.Value) * 100) / group.Sum(x => x.Value) : 0.0,

               //}).ToList()
               //  .Concat(StatisticsTypes.Where(x => !group.Where(x => x.InteractionStatisticsTypeNavigation != null).Select(x => x.InteractionStatisticsTypeNavigation.Id).ToList().Contains(x.Id))
               //  .ToList().Adapt<List<StatusResult>>()).ToList().OrderBy(x => x.Id))

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

    }
}
