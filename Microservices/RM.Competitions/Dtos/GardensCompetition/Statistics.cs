namespace RM.Competitions.Dtos
{
    public class Statistics
    {
        public CompetitorsClassifications CandidatedCompetitors { get; set; } = new CompetitorsClassifications();
        public CompetitorsClassifications RegisterdCompetitors { get; set; } = new CompetitorsClassifications();
        public CompetitorsClassifications WaitingCompetitors { get; set; } = new CompetitorsClassifications();
        public CompetitorsClassifications RejectedCompetitors { get; set; } = new CompetitorsClassifications();
        public CompetitorsClassifications CompetitorsCompleteAttachments { get; set; } = new CompetitorsClassifications();


        public class CompetitorsClassifications
        {
            public int Total { get; set; }
            public int Students { get; set; }
            public int Teams { get; set; }
            public int Companies { get; set; }
        }
    }

}
