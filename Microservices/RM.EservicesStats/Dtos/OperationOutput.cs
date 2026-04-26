

using RM.Core.Helpers;

namespace RM.EservicesStats.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string EserviceStatsEntity = "EserviceStatsEntity";
            public const string PrevEserviceStatsEntity = "PrevEserviceStatsEntity";
            public const string CurrEserviceStatsEntity = "CurrEserviceStatsEntity";
            public const string EntityID = "EntityID";
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
