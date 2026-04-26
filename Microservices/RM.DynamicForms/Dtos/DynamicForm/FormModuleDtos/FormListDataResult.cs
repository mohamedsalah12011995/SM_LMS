using RM.Core.Helpers;

namespace RM.DynamicForms.Dtos.DynamicForm.FormModuleDtos
{
    public class FormListDataResult
    {
        public ApplicationOperation.Pagination Pagination { get; set; }
        public List<object> ListFormValue { get; set; }

    }

}
