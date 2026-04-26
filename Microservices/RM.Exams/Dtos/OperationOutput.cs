
using RM.Core.Helpers;

namespace RM.Exams.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string ExamsEntity = "ExamsEntity";
            public const string ExamQuesionEntity = "ExamQuesionEntity";
            public const string EntityID = "EntityID";
            public const string Pagination = "Pagination";

            public const string NotExpiredServeys = "NotExpiredServeys";
            public const string ExpiredServeys = "ExpiredServeys";
            public const string AnswersCount = "AnswersCount";
            public const string ExamResult = "ExamResult";
            public const string ExamsList = "ExamsList";
            public const string TypesList = "TypesList";

            public const string UserAnswers = "UserAnswers";
            public const string Courses = "Courses";
            public const string CourseEntity = "CourseEntity";
            public const string CourseTypes = "CourseTypes";
            public const string DepartmentReferences = "DepartmentReferences";
            public const string CourseScheduleEntity = "CourseScheduleEntity";
            public const string CourseAdvertisements = "CourseAdvertisements";
            public const string CourseAdvertisementEntity = "CourseAdvertisementEntity";
            public const string Grades = "Grades";
            public const string Traniee = "Traniee";
            public const string ExamTimeCounter = "ExamTimeCounter";
            public const string CertifcateEntity = "CertifcateEntity";
            public const string Certificates = "Certificates";
            public const string CertificationThemes = "CertificationThemes";

            public const string UserDepartments = "UserDepartments";


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
