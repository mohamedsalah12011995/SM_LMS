
using RM.Core.Helpers;

namespace RM.Competitions.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string Code = "Code";
            public const string CompetitionEntity = "CompetitionEntity";
            public const string LookupsEntity = "LookupsEntity";
            public const string EntityID = "CompetitionID";
            public const string Pagination = "Pagination";
            public const string CompetitorsType = "CompetitorsType";
            public const string CountryCities = "CountryCities";
            public const string AttchmentType = "AttchmentType";
            public const string CandidateType = "CandidateType";
            public const string AttachmentCompleteType = "AttachmentCompleteType";
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
