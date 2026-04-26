using RM.Core.Helpers;

namespace RM.InitiativesProjects.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string InitiativesProjectsEntity = "InitiativesProjectsEntity";
            public const string InitiativesProjectsTypeEntity = "InitiativesProjectsTypeEntity";
            public const string BeneficiariesEntity = "BeneficiariesEntity";
            public const string EntityID = "EntityID";
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
