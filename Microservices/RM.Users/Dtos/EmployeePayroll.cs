using System.Text.Json.Serialization;

namespace RM.Users.Dtos
{
    public class EmployeePayroll
    {
        [JsonPropertyName("header")]
        public Header? Header { get; set; }
        [JsonPropertyName("body")]
        public BodyIPayroll? Body { get; set; }
    }
    public class BodyIPayroll
    {
        public employeePayroll? employeePayroll { get; set; }
    }

    public class employeePayroll
    {
        public string? period { get; set; }
        public int? year { get; set; }
        public List<DeductionsAndAllowancesList>? deductionsList { get; set; }
        public List<DeductionsAndAllowancesList>? allowancesList { get; set; }
        public decimal? totalSallary { get; set; }




    }
}
public class DeductionsAndAllowancesList
{
    public decimal? amount { get; set; }
    public string? currencyName { get; set; }
    public string? financialElementName { get; set; }
}
