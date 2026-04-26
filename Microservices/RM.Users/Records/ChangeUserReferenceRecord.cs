using DocumentFormat.OpenXml.Office2010.Excel;
using RM.Core.Helpers;

namespace RM.Users.Records
{
    public record ChangeUserReferenceRecord
    {
        public string ID {  get; set; }
        public string referenceId {  get; set; }
    }
}
