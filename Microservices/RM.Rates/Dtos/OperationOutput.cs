using RM.Core.Helpers;
using static RM.Core.Helpers.Enums;

namespace RM.Rates.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public const string CommentsEntity = "RatesEntity";
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
