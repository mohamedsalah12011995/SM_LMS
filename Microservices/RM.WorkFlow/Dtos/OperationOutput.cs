using RM.Core.Helpers;
using static RM.Core.Helpers.Enums;

namespace RM.WorkFlow.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {

        public class OperationOutputKeys
        {
            #region WORKFLOW

            public const string Code = "Code";
            public const string WorkFlowEntity = "WorkFlowEntity";
            public const string WorkFlowActionsList = "WorkFlowActionsList";
            public const string JobRolesList = "JobRolesList";
            public const string UserActions = "UserActions";
            public const string ActionId = "ActionId";
            public const string Action = "Action";
            public const string FormValueActions = "FormValueActions";
            public const string FormValue = "FormValue";
            public const string EntityName = "EntityName";
            public const string TransferRefrences = "TransferRefrences";
            public const string Steps = "Steps";
            public const string DynamicForm = "DynamicForm";
            public const string References = "References";
            public const string Id = "Id";
            public const string FieldsUrl = "FieldsUrl";
            public const string ItemActivation = "ItemActivation";
            public const string Pagination = "Pagination";
            public const string ReferencesEntity = "ReferencesEntity";
            public const string EntityFormValue = "EntityFormValue";
            public const string EntityObj = "EntityObj";
            public const string UserPermission = "UserPermission";
            public const string EngineActionEmailData = "EngineActionEmailData";

            #endregion

        }

        public static OperationOutput GetOperationOutput(ServiceMessages header, params OutputDictionary[] outputs)
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

