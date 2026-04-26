using RM.Core.Helpers;

namespace RM.WorkFlow.Dtos.DyFormEntities
{
    public class FormListDataResult
    {
        public ApplicationOperation.Pagination Pagination { get; set; }
        public List<object> ListFormValue { get; set; }
        public HeaderUserActions HeaderUserActions { get; set; }

    }
    public class HeaderUserActions // for list
    {
        public bool AllowAddNew { get; set; }
        public bool AllowActivate { get; set; }
        public bool AllowDelete { get; set; }
    }
}
