using RM.Core.Helpers;
using static RM.Core.Helpers.Enums;

namespace RM.Statistics.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string EntityID = "EntityID";
            public const string Statistics = "Statistics";
            public const string EntitiesLatestUpdate = "EntitiesLatestUpdate";
            public const string TotalGeneralNumber = "TotalGeneralNumber";
            public const string MostVisitedArticals = "MostVisitedArticals";
            public const string LowestVisitedArticals = "LowestVisitedArticals";
            public const string MostRatedArticals = "MostRatedArticals";
            public const string LowestRatedArticals = "LowestRatedArticals";
            public const string EntityTotalVisitorCount = "EntityTotalVisitorCount";
            public const string InteractionStatistics = "InteractionStatistics";
            public const string VisitorId = "VisitorId";
            public const string InteractionStatisticsTypes = "InteractionStatisticsTypes";
            public const string EntityItems = "EntityItems";
            public const string TotalCount = "TotalCount";
            public const string YearsStatistics = "YearsStatistics";
            public const string YearsList = "YearsList";


        }
        public static OperationOutput GetOperationOutput(ServiceMessages header, params OutputDictionary[] outputs)
        {
            var result = (OperationOutput)Activator.CreateInstance(typeof(OperationOutput))!;

            if (outputs is not null)
            {
                result.Output = new Dictionary<string, object>();
                foreach (var output in outputs)
                    result.Output.Add(output.key, output.value);
            }
            result.Header = ApplicationOperation.OperationResult(header);


            return result;
        }

    }

    public record OutputDictionary(string key, object value);

}
