#nullable disable

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Models
{
    [Table("QuestionsRecommendations", Schema = "Survey")]
    public class QuestionsRecommendations
    {
        public int Id { get; set; }

        [ForeignKey("LessAverage")]
        public int? LessAverageId { get; set; }
        public Recommendations LessAverage { get; set; }


        [ForeignKey("Average")]
        public int? AverageId { get; set; }
        public Recommendations Average { get; set; }


        [ForeignKey("AboveAverage")]
        public int? AboveAverageId { get; set; }
        public Recommendations AboveAverage { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public virtual SurveyQuestion Question { get; set; } 

    }
}
