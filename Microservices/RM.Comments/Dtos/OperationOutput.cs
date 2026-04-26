using RM.Core.Helpers;

namespace RM.Comments.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string CommentsEntity = "CommentsEntity";
            public const string Pagination = "Pagination";
            public const string EntitiesItem = "EntitiesItem";
        }
        public static OperationOutput GetOperationOutput(Enums.ServiceMessages header,int? code=null, params OutputDictionary[] outputs)
        {
            var result = (OperationOutput)Activator.CreateInstance(typeof(OperationOutput))!;

            if (outputs is not null)
            {
                result.Output = new Dictionary<string, object>();
                foreach (var output in outputs)
                    result.Output.Add(output.key, output.value);
            }
            result.Header = ApplicationOperation.OperationResult(header);
            if(code != null)
               result.Header.Code = code.Value;

            return result;
        }
    }
    public record OutputDictionary(string key, object value);
}