

using RM.Core.Helpers;
using static RM.Core.Helpers.Enums;

namespace RM.News.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string NewsEntity = "NewsEntity";
            public const string EntityID = "EntityID";
            public const string TagsEntity = "TagsEntity";
            public const string TagsEntityEn = "TagsEntity";
            public const string Pagination = "Pagination";
            public const string Statistics = "Statistics";
            public const string References = "References";


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
