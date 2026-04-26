namespace IntegrationService.Records.Hars
{
    public record GetExternalData
    {
        public dynamic Object { get; set; }
        public string API { get; set; }
        public string HttpMethod { get; set; }
    }
}
