using RM.Core.Helpers;

namespace RM.Innovations.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string Code = "Code";
            public const string IdeaEntity = "IdeaEntity";
            public const string EntityID = "EntityID";
            public const string Pagination = "Pagination";
            public const string CommentsEntity = "CommentsEntity";
            public const string RatesEntity = "RatesEntity";
            public const string LatestCount = "LatestCount";
            public const string ProgressCount = "ProgressCount";
            public const string ExecutCount = "ExecutCount";
            public const string CompleteCount = "CompleteCount";
            public const string AllowManageComments = "AllowManageComments";

            public const string IdeasCount = "IdeasCount";
            public const string IdeasCommentsCount = "IdeasCommentsCount";
            public const string IdeasVoteCount = "IdeasVoteCount";
            public const string IdeasStatistics = "IdeasStatistics";
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
