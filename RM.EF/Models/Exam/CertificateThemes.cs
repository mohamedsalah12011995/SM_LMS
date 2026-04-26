using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("CertificateThemes", Schema = "Exams")]

    public class CertificateThemes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Color { get; set; }
        public string ThumbnailPicture { get; set; }
        public bool IsActive { get; set; }
    }
}
