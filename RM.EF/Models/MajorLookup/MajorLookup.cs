
#nullable disable


using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("MajorLookups", Schema = "dbo")]

    public partial class MajorLookup
    {
        public MajorLookup()
        {
            InverseParent = new HashSet<MajorLookup>();
            VolunteerAges = new HashSet<Volunteer>();
            VolunteerDistricts = new HashSet<Volunteer>();
            VolunteerGenders = new HashSet<Volunteer>();
            VolunteerQualifications = new HashSet<Volunteer>();
            OpenDataDistricts = new HashSet<OpenData>();
            OpenDataTypes= new HashSet<OpenData>();

            OpenDataStatisticsDistricts = new HashSet<OpenDataStatistics>();
            OpenDataStatisticsTypes = new HashSet<OpenDataStatistics>();

            OpenDataTempDistricts = new HashSet<OpenDataTemp>();
            OpenDataTempTypes = new HashSet<OpenDataTemp>();
            AdvertisementRegions = new HashSet<Advertisement>();
            ScientificLettersDegrees = new HashSet<ScientificLetters>();

            OrderActionTypes = new HashSet<OrderActions>();

            IdeaActions = new HashSet<IdeaAction>();
            IdeaCategoryNavigations = new HashSet<Idea>();
            IdeaPriorityNavigations = new HashSet<Idea>();
            IdeaStatusNavigations = new HashSet<Idea>();
            IdeaTypeNavigations = new HashSet<Idea>();


        }

        public int Id { get; set; }
        public int? MapId { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int TypeId { get; set; }
        public int? ParentId { get; set; }
        public int? ReferenceId { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }

        public virtual MajorLookup Parent { get; set; }
        public virtual MajorLookupsType Type { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual ICollection<MajorLookup> InverseParent { get; set; }
        public virtual ICollection<Volunteer> VolunteerAges { get; set; }
        public virtual ICollection<Volunteer> VolunteerDistricts { get; set; }
        public virtual ICollection<Volunteer> VolunteerGenders { get; set; }
        public virtual ICollection<Volunteer> VolunteerQualifications { get; set; }
        public virtual ICollection<Volunteer> VolunteerFields { get; set; }
        public virtual List<PermitWorkSite> PermitWorkSites { get; set; }
        public virtual ICollection<OpenData> OpenDataDistricts { get; set; }
        public virtual ICollection<OpenData> OpenDataTypes { get; set; }

        public virtual ICollection<OpenDataStatistics> OpenDataStatisticsDistricts { get; set; }
        public virtual ICollection<OpenDataStatistics> OpenDataStatisticsTypes { get; set; }

        public virtual ICollection<OpenDataTemp> OpenDataTempDistricts { get; set; }
        public virtual ICollection<OpenDataTemp> OpenDataTempTypes { get; set; }
        public virtual ICollection<Advertisement> AdvertisementRegions { get; set; }
        public virtual ICollection<ScientificLetters> ScientificLettersDegrees { get; set; }
        public virtual ICollection<OrderActions> OrderActionTypes { get; set; }

        public virtual ICollection<IdeaAction> IdeaActions { get; set; }
        public virtual ICollection<Idea> IdeaCategoryNavigations { get; set; }
        public virtual ICollection<Idea> IdeaPriorityNavigations { get; set; }
        public virtual ICollection<Idea> IdeaStatusNavigations { get; set; }
        public virtual ICollection<Idea> IdeaTypeNavigations { get; set; }



    }
}
