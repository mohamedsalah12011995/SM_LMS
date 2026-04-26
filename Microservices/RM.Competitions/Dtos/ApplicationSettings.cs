namespace RM.Competitions.Dtos
{
    public class ApplicationSettings
    {
        public string DbConnectionString { get; set; }
        public string OTPCodeMessage { get; set; }
        public string LookupsSeviceUrl { get; set; }
        
        public SettingsGardensCompetition GardensCompetition { get; set; }
        public class SettingsGardensCompetition
        {
            public string AttachmentsSavePath { get; set; }
            public string AttachmentsGetPath { get; set; }
            public string CompetitionCandidateSuccessSMS { get; set; }
        }
    }

}

