
#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("AdvertisementCareers", Schema = "Job")]

    public class AdvertisementCareer
    {
        public int? Id { get; set; }
        public int? AdvertisementId { get; set; }
        public int? CareerId { get; set; }
        public int? MaxLimit { get; set; }


        [NotMapped]
        public virtual JobAdvertisement Advertisement { get; set; }
        [NotMapped]
        public virtual JobCareer Career { get; set; }
    }
}
