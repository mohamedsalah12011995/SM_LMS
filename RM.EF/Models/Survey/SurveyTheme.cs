#nullable disable

using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{

    [Table("Themes", Schema = "Survey")]
    public class SurveyTheme
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Survey> Surveys { get; set; } = new List<Survey>();


    }
}
