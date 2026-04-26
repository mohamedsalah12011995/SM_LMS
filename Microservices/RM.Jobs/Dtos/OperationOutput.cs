using RM.Core.Helpers;

namespace RM.Jobs.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string JobsCareerEntity = "JobsCareerEntity";
            public const string JobAdvertisementEntity = "JobAdvertisementEntity";
            public const string EntityID = "EntityID";
            public const string Pagination = "Pagination";
            public const string Qualifications = "Qualifications";
            public const string JobApplicationEntity = "JobApplicationEntity";
            public const string yearsLookup = "yearsLookup";
            public const string Specialization = "Specialization";
            public const string Specifications = "Specifications";
            public const string Tags = "Tags";
            public const string Skills = "Skills";
            public const string ActionState = "ActionState";
            public const string JobApplicationStatuses = "JobApplicationStatuses";
            public const string Gender = "Gender";
            public const string MilitaryApplicationEntity = "MilitaryApplicationEntity";
            public const string Grades = "Grades";
            public const string ExamsEntity = "ExamsEntity";
            public const string ExamTimeCounter = "ExamTimeCounter";
            public const string ExamResultStatus = "ExamResultStatus";

            public const string JobApplicationFromDate = "JobApplicationFromDate";
            public const string JobApplicationToDate = "JobApplicationToDate";

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