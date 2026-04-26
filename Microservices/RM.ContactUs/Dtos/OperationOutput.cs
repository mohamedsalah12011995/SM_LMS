

using RM.Core.Helpers;

namespace RM.ContactUs.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string ComplainId = "ComplainId";
            public const string ContactUsEntity = "ContactUsEntity";
            public const string CategoryEntity = "CategoryEntity";
            public const string EntityID = "EntityID";
            public const string Pagination = "Pagination";
            public const string ItemId = "ItemId";
            public const string ActionStatus = "ActionStatus";
            public const string Action = "Action";
            public const string HeadQuartiers = "HeadQuartiers";
            public const string Departments = "Departments";
            public const string ChildReferences = "ChildReferences";
            public const string ContactActions = "ContactActions";
            public const string Code = "Code";
            public const string FeedbackEntity = "FeedbackEntity";
            public const string ViewEvaluation = "ViewEvaluation";
            public const string ContactFeedbacks = "ContactFeedbacks";
            public const string Regions = "Regions";
            public const string EntityItems = "EntityItems";
            public const string SuggestionsCount = "SuggestionsCount";
            public const string ComplaintsCount = "ComplaintsCount";
            public const string SuggestionsStatistics = "SuggestionsStatistics";
            public const string ComplaintsStatistics = "ComplaintsStatistics";
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