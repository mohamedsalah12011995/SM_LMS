namespace IntegrationService.Records.Nafath
{
    public record SendRequestRecord
    {
        public string Action { get; set; } = "SpRequest";
        public SendRequestParameters Parameters { get; set; } 
    }

    public record SendRequestParameters
    {
        public string Id { get; set; }
        public string Service { get; set; } = "Login";
    }

    public record SendRequestResponse
    {
        public string TransId { get; set; }
        public string Random { get; set; }
    }

}
