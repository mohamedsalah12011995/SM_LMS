

using RM.Core.Helpers;

namespace RM.Orders.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string OrderEntity = "OrderEntity";
            public const string OrderTypes = "OrderTypes";
            public const string Users = "Users";
            public const string BookActions = "BookActions";
            public const string FatwaActions = "FatwaActions";
            public const string OrderActionTypes = "OrderActionTypes";
            public const string Pagination = "Pagination";

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