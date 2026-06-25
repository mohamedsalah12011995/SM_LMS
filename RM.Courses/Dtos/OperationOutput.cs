using RM.Core.Helpers;

namespace RM.Courses.Dtos.OperationOutput
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string CoursesEntity = "CoursesEntity";
            public const string EntityID = "EntityID";
            public const string Pagination = "Pagination";

            public const string CategoriesEntity = "CategoriesEntity";
            public const string InstructorsEntity = "InstructorsEntity";
            public const string TagsEntity = "TagsEntity";
            public const string SectionsEntity = "SectionsEntity";
            public const string LessonsEntity = "LessonsEntity";
            public const string MaterialsEntity = "MaterialsEntity";
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
