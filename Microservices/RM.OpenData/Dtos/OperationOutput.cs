

using RM.Core.Helpers;

namespace RM.OpenData.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
                        
            public const string OpenDataEntity = "OpenDataEntity";
            public const string OpenDataTempEntity = "OpenDataTempEntity";
            public const string OpenDataStatistics = "OpenDataStatistics";

            public const string NewOpenDataTempCount = "NewOpenDataTempCount";
            public const string UpdatedOpenDataTempCount = "UpdatedOpenDataTempCount";


            public const string EntityID = "EntityID";
            public const string Pagination = "Pagination";

            public const string Years = "Years";
            public const string Months = "Months";

            public const string Districts = "Districts";
            public const string Types = "Types";
            public const string SubTypes = "SubTypes";
            public const string StatisticTypes = "StatisticTypes";


            public const string Rescue = "Rescue";
            public const string Infiltrations = "Infiltrations";
            public const string Smuggling = "Smuggling";
            public const string Confiscations = "Confiscations";
            public const string Arrests = "Arrests";
            public const string AllowPublish = "AllowPublish";

            public const string OpenDataEntityMonth = "OpenDataEntityMonth";
            public const string GregorianYears = "GregorianYears";

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