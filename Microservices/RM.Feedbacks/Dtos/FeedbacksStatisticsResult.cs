using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Feedbacks.Dtos
{
    public class FeedbacksStatisticsResult
    {
        [JsonIgnore]
        public int? FeedbacksId { get; set; }
        public string feedbacksId { set { FeedbacksId = Accessor.Set(value); } get { return Accessor.Get(FeedbacksId); } }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public int? ThisDay { get; set; }
        public int? ThisWeek { get; set; }
        public int? ThisMonth { get; set; }
        public int? ThisYear { get; set; }
        public int? Total { get; set; }
        public List<YearResult> Years { get; set; } = new List<YearResult>();

    }

    public class GroupedDataSource
    {
        public bool? IsHelpful { get; set; }
        public string HelpfulAr { get; set; }
        public string HelpfulEn { get; set; }
        public List<DataSourcesStatistics> DataSources { get; set; } = new List<DataSourcesStatistics>();

    }
    public class DataSourcesStatistics
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string TextAr { get; set; }
        public string TextEn { get; set; }

        public bool? HasNote { get; set; }
        public string Note { get; set; }

        public bool? IsHelpful { get; set; }

        public int? Count { get; set; } = 0;
        public double? Avg { get; set; } = 0;
        public Recommendations Recommendation { get; set; }

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

        public static TypeAdapterConfig SelectConfig(int month, List<RM.Models.FeedbacksAnswerAction> group, int yearCount)
        {
            return new TypeAdapterConfig()
                .NewConfig<MonthResult, MonthResult>()
                .Map(dest => dest.Month, src => month)
                .Map(dest => dest.MonthRate, src => group.Count() > 0 ? group.Count(x => x.IsHelpful == true) * 10 / group.Count() : 0)
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
        public List<GroupedDataSource> DataSources { get; set; } = new List<GroupedDataSource>();

        public static TypeAdapterConfig SelectConfig(List<RM.Models.FeedbacksDataSource> dataSources, List<RM.Models.Recommendations> Recomendations, int quarter, List<RM.Models.FeedbacksAnswerAction> group, int yearCount, int month1, List<RM.Models.FeedbacksAnswerAction> groupMonth1, int month2, List<RM.Models.FeedbacksAnswerAction> groupMonth2, int month3, List<RM.Models.FeedbacksAnswerAction> groupMonth3)
        {
            return new TypeAdapterConfig()
                .NewConfig<QuarterResult, QuarterResult>()
                .Map(dest => dest.Quarter, src => quarter)
                .Map(dest => dest.QuarterRate, src => group.Count() > 0 ? group.Count(x => x.IsHelpful == true) * 10 / group.Count() : 0)
                .Map(dest => dest.QuarterCount, src => group.Count())
                .Map(dest => dest.QuarterAvg, src => group.Count() > 0 ? (double)(group.Count() * 100) / (double)yearCount : 0.0)
                .Map(dest => dest.Monthes, src => new List<MonthResult>
                {
                   new MonthResult().Adapt<MonthResult>(MonthResult.SelectConfig(month1,groupMonth1,yearCount)),
                   new MonthResult().Adapt<MonthResult>(MonthResult.SelectConfig(month2,groupMonth2,yearCount)),
                   new MonthResult().Adapt<MonthResult>(MonthResult.SelectConfig(month3,groupMonth3,yearCount)),

                })
               .Map(dest => dest.DataSources, src => group.SelectMany(x => x.FeedbacksAnswers).Any() ? group.SelectMany(x => x.FeedbacksAnswers)
               .GroupBy(x => new { x.FeedbacksDataSource.IsHelpful }, (key, hgroup) => new GroupedDataSource
               {
                   IsHelpful = key.IsHelpful,
                   HelpfulAr = key.IsHelpful == true ? "مفيد":"غير مفيد",
                   HelpfulEn = key.IsHelpful == true ? "Helpful" : "Not Helpful",
                   DataSources = hgroup.GroupBy(x => new { x.FeedbacksDataSourceId }, (key, dgroup) => new DataSourcesStatistics
                   {
                       Id = key.FeedbacksDataSourceId,
                       TextAr = dgroup.First().FeedbacksDataSource.TextAr,
                       TextEn = dgroup.First().FeedbacksDataSource.TextEn,
                       HasNote = dgroup.First().FeedbacksDataSource.HasNote,
                       IsHelpful = dgroup.First().FeedbacksDataSource.IsHelpful,
                       Note = dgroup.First().Text,
                       Count = dgroup.Count(),
                       Avg = dgroup.Count() > 0 ? (double)(dgroup.Count() * 100) / group.Count() : 0.0,
                       Recommendation = Recommendation(dgroup.Count() > 0 ? (double)(dgroup.Count() * 100) / group.Count() : 0.0, dgroup.First().FeedbacksDataSource, Recomendations),

                   }).ToList()
                   .Concat(dataSources.Where(x => !hgroup.Select(x => x.FeedbacksDataSourceId).ToList().Contains(x.Id) && x.IsHelpful == key.IsHelpful)
                   .ToList().Adapt<List<DataSourcesStatistics>>()).ToList()
               }).ToList() 
               : new List<GroupedDataSource> 
               {
                   new GroupedDataSource { IsHelpful=true, HelpfulAr ="مفيد", HelpfulEn = "Helpful", DataSources = dataSources.Where(x=>x.IsHelpful == true).Adapt<List<DataSourcesStatistics>>().ToList()},
                   new GroupedDataSource { IsHelpful=false, HelpfulAr ="غير مفيد", HelpfulEn = "Not Helpful", DataSources = dataSources.Where(x=>x.IsHelpful == false).Adapt<List<DataSourcesStatistics>>().ToList()}
               })

                .Config;
        }

        private static Recommendations Recommendation(double? rate, RM.Models.FeedbacksDataSource dataSource, List<RM.Models.Recommendations> Recomendations)
        {
            if (rate < 50)
                return  Recomendations.FirstOrDefault(x => x.Id == dataSource.LessAverageId).Adapt<Recommendations>(Recommendations.SelectConfig());
            else if (rate == 50)
                return Recomendations.FirstOrDefault(x => x.Id == dataSource.AverageId).Adapt<Recommendations>(Recommendations.SelectConfig());
            else if (rate > 50)
                return Recomendations.FirstOrDefault(x => x.Id == dataSource.AboveAverageId).Adapt<Recommendations>(Recommendations.SelectConfig());
            else return new Recommendations();
        }
    }


    }