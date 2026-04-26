using LinqKit;
using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Surveys.Dtos
{
    public class SurveyResultStatistices
    {
        [JsonIgnore]
        public int? SurveyId {  get; set; }
        public string surveyId { set { SurveyId = Accessor.Set(value); } get { return Accessor.Get(SurveyId); } }

        public string TitleAr {  get; set; }
        public string TitleEn {  get; set; }
        public List<YearResult> Years {  get; set; }
        public int? Day {  get; set; }
        public int? Week {  get; set; }
        public int? Month {  get; set; }
        public int? Year {  get; set; }
        public int? Total {  get; set; }
    }

    public class YearResult
    {
        public int Year { get; set; }
        public int YearRate { get; set; }
        public int YearCount { get; set; }
        public List<int> Monthes { get; set; }
        public List<double> MonthesAvg { get; set; }
        public List<int> MonthesCount { get; set; }
    }
}