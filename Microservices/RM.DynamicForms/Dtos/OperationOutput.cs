using RM.Core.Helpers;

namespace RM.DynamicForms.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {
        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string InputTypes = "InputTypes";
            public const string References = "References";
            public const string Entities = "Entities";
            public const string FormTypes = "FormTypes";
            public const string DynamicForm = "DynamicForm";
            public const string EntityID = "EntityID";
            public const string Pagination = "Pagination";
            public const string DataSources = "DataSources";
            public const string EntityFormValue = "EntityFormValue";
            public const string ItemActivation = "ItemActivation";
            public const string ItemFormValue = "ItemFormValue";
            public const string ItemId = "ItemId";
            public const string FieldsUrl = "FieldsUrl";
            public const string EntityObj = "EntityObj";
            public const string KeysAdvancedSearch = "KeysAdvancedSearch";
            public const string FormId = "FormId";
            public const string Departments = "Departments";
            public const string WorkFlow = "WorkFlow";
            public const string UserPermission = "UserPermission";
            public const string AccordionFields = "AccordionFields";
            public const string Themes = "Themes";
            public const string APIDatasourcesDto = "APIDatasourcesDto";
            public const string DataList = "DataList";
            public const string NewFormsId = "NewFormsId";
            public const string FormIsViewStatistic = "FormIsViewStatistic";
            public const string EngineActionsJobRole = "EngineActionsJobRole";



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
