#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Models
{

    public class Recommendations
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public string ContentAr { get; set; }
        public string ContentEn { get; set; }

        [ForeignKey("Entity")]
        public int? EntityId { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }


        [ForeignKey("UpdatedByNavigation")]
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("Reference")]
        public int? ReferenceId { get; set; }

        public virtual Entity Entity { get; set; }


        public virtual Reference Reference { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }

        public virtual List<QuestionsRecommendations> QuestionRecommendationLessAverages { get; set; } = new List<QuestionsRecommendations>();

        public virtual List<QuestionsRecommendations> QuestionRecommendationAverages { get; set; } = new List<QuestionsRecommendations>();

        public virtual List<QuestionsRecommendations> QuestionRecommendationAboveAverages { get; set; } = new List<QuestionsRecommendations>();



    }
}
