using RM.Core.Helpers;

namespace RM.Advertisements.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string AdvertisementsEntity = "AdvertisementsEntity";
            public const string EntityID = "EntityID";
            public const string Pagination = "Pagination";
            public const string DistrictLookup = "DistrictLookup";
            public const string References = "References";

        }
        public static OperationOutput GetOperationOutput(Enums.ServiceMessages header, params OutputDictionary[] outputs)
        {
            var result = (OperationOutput)Activator.CreateInstance(typeof(OperationOutput))!;
            var handler = new ApplicationOperation();

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
