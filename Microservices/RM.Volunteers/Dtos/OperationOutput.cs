

using RM.Core.Helpers;

namespace RM.Volunteers.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string VolunteersEntity = "VolunteersEntity";
            public const string Qualifications= "Qualifications";
            public const string AgeRanges = "AgeRanges";
            public const string Districts= "Districts";
            public const string Genders = "Genders";
            public const string VolunteeringFields = "VolunteeringFields";
            public const string EntityID = "EntityID";
            public const string Pagination = "Pagination";
            public const string Cities = "Cities";
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