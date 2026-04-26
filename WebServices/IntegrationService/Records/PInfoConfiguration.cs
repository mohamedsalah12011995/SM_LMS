namespace IntegrationService.Records
{
    public record PInfoConfiguration
    {
        public string ServerHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
