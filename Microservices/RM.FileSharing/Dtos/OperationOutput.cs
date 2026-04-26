
using RM.Core.Helpers;

namespace RM.FileSharing.Dtos
{
    public class OperationOutput: ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKey
        {
            public const string Id = "Id";
            public const string FileSharingEntity = "FileSharingEntity";
            public const string Directories = "Directories";
            public const string Files = "Files";
            public const string ADUsers = "ADUsers";
            public const string ADGroups = "ADGroups";
            public const string RoleTypes = "RoleTypes";
            public const string DirFileAccessRule = "DirFileAccessRule";
            public const string EntityID = "EntityID";
            public const string Pagination = "Pagination";
            public const string FileUploadSummary = "FileUploadSummary";            
            public const string ProccessId = "ProccessId";
            public const string StartCount = "StartCount";
            public const string FinishCount = "FinishCount";
            public const string StopProccess = "StopProccess";
            public const string Parents = "Parents";


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
