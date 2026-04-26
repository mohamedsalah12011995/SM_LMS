using System.Text.Json.Serialization;

namespace IntegrationService.Records.Hars
{
    public record GetLastPayroll
    {
        [JsonPropertyName("employeeId")]
        public int? EmployeeId { get; set; }
    }

    public record GetLastPayrollResponse
    {
        [JsonPropertyName("Response")]
        public GetLastPayrollResponseBody ResponseBody { get; set; }
    }


    public record GetLastPayrollResponseBody
    {

        [JsonPropertyName("period")]
        public string Period { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("DeductionsList")]
        public List<Financial> DeductionsList { get; set; } = new List<Financial>();

        [JsonPropertyName("AllowancesList")]
        public List<Financial> AllowancesList { get; set; } = new List<Financial>();

    }

    public record Financial
    {
        [JsonPropertyName("amount")]
        public double? Amount { get; set; }

        [JsonPropertyName("currencyName")]
        public string CurrencyName { get; set; }

        [JsonPropertyName("financialElementName")]
        public string FinancialElementName { get; set; }
    }
}
