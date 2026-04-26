using RM.Core.Helpers;

namespace RM.Lookups.Records
{
    record class CronSettingsRecord
    {
        public string ID { get; set; }

        public int? CronTypeId { get; set; }

        public List<string> Emails { get; set; }

        public string entityId { get; set; }

        public string surveyId { get; set; }
        public string referenceId { get; set; }
        public bool? IsActive { get; set; }
    }
}
