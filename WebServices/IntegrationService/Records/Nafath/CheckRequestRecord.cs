namespace IntegrationService.Records.Nafath
{
    public record CheckRequestRecord
    {
        public string Action { get; set; } = "CheckSpRequest";
        public CheckRequestParameters Parameters { get; set; }
    }

    public record CheckRequestParameters
    {
        public string Id { get; set; }
        public string TransId { get; set; }
        public string Random { get; set; }
    }

    public record CheckRequestResponse
    {
        public string Status { get; set; }
        public object Person { get; set; }
        public string AccessToken { get; set; }

    }
}
