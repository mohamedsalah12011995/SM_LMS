using RM.Core.Helpers;

namespace RM.Feedbacks.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKey
        {
            public const string Id = "Id";
            public const string FeedbacksEntity = "FeedbacksEntity";
            public const string EntityID = "EntityID";
            public const string Pagination = "Pagination";
            public const string EntityItems = "EntityItems";
            public const string TotalCount = "TotalCount";
            public const string HelpfulCount = "HelpfulCount";
            public const string NotHelpfulCount = "NotHelpfulCount";
            public const string TotalRate = "TotalRate";
            public const string YearsStatistics = "YearsStatistics";
        }
        public static OperationOutput GetOperationOutput(Enums.ServiceMessages header, params OutputDictionary[] outputs)
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