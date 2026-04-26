
using System;
using System.Collections.Generic;


#nullable disable

namespace RM.Models
{
    public partial class Reference
    {
        public Reference()
        {
            AdminMenus = new HashSet<AdminMenu>();
            Advertisements = new HashSet<Advertisement>();
            AmanahAwards = new HashSet<AmanahAward>();
            Articles = new HashSet<Article>();
            ArticlesPublishes = new HashSet<ArticlesPublish>();
            Attachments = new HashSet<Attachment>();
            Comments = new HashSet<Comment>();
            ContactUs = new HashSet<ContactU>();
            Documents = new HashSet<Document>();
            Entities = new HashSet<Entity>();
            Eservices = new HashSet<Eservice>();
            ExternalSites = new HashSet<ExternalSite>();
            GovServices = new HashSet<GovService>();
            InitiativesProjects = new HashSet<InitiativesProject>();
            InteractionStatistics = new HashSet<InteractionStatistic>();
            InverseParent = new HashSet<Reference>();
            Menus = new HashSet<Menu>();
            Multimedia = new HashSet<Multimedia>();
            News = new HashSet<News>();
            Officials = new HashSet<Official>();
            Partners = new HashSet<Partner>();
            QuestionsAnswers = new HashSet<QuestionsAnswer>();
            Rates = new HashSet<Rate>();
            ReferenceContents = new HashSet<ReferenceContent>();
            ReferencesJobRoles = new HashSet<ReferencesJobRole>();
            Roles = new HashSet<Role>();
            Surveys = new HashSet<Survey>();

            TermsAndRegulations = new HashSet<TermsAndRegulation>();
            Users = new HashSet<User>();
            JobAdvertisements = new HashSet<JobAdvertisement>();
            MajorLookups = new HashSet<MajorLookup>();
            OpenDatas = new HashSet<OpenData>();
            OpenDataTemps = new HashSet<OpenDataTemp>();
            OpenDataRequests = new HashSet<OpenDataRequest>();
            OpenDataStatistics = new HashSet<OpenDataStatistics>();
            ScientificLetters = new HashSet<ScientificLetters>();
            Order = new HashSet<Order>();
            LogInformations = new HashSet<LogInformation>();
            Exams = new HashSet<Exam>();
            Form = new HashSet<Form>();
            UsersEntitiesReferences = new HashSet<UsersEntityReference>();
            EnginReferences = new HashSet<Engine>();
            FormValuesActionsToReferences = new HashSet<FormValuesActions>();
            WorkFlowActionsReferences = new HashSet<WorkFlowActions>();

            Ideas = new HashSet<Idea>();
            IdeasTo = new HashSet<Idea>();
            IdeasCompetentAuthorities = new HashSet<IdeasCompetentAuthority>();
            Recommendations = new HashSet<Recommendations>();
            CronSettings = new HashSet<CronSettings>();
            Feedbacks = new HashSet<Feedbacks>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? ReferencesMajorId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }


        public bool? IsDeleted { get; set; }
        public bool? HasContent { get; set; }
        public int? ParentId { get; set; }
        public int? DeletedBy { get; set; }
        public bool? IsPortal { get; set; }
        public string Url { get; set; }
        public DateTime? DeletedDate { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual Reference Parent { get; set; }
        public virtual ReferencesMajor ReferencesMajor { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<AdminMenu> AdminMenus { get; set; }
        public virtual ICollection<Advertisement> Advertisements { get; set; }
        public virtual ICollection<AmanahAward> AmanahAwards { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<ArticlesPublish> ArticlesPublishes { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ContactU> ContactUs { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Entity> Entities { get; set; }
        public virtual ICollection<Eservice> Eservices { get; set; }
        public virtual ICollection<ExternalSite> ExternalSites { get; set; }
        public virtual ICollection<GovService> GovServices { get; set; }
        public virtual ICollection<InitiativesProject> InitiativesProjects { get; set; }
        public virtual ICollection<InteractionStatistic> InteractionStatistics { get; set; }
        public virtual ICollection<Reference> InverseParent { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<Multimedia> Multimedia { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Official> Officials { get; set; }
        public virtual ICollection<Partner> Partners { get; set; }
        public virtual ICollection<QuestionsAnswer> QuestionsAnswers { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<ReferenceContent> ReferenceContents { get; set; }
        public virtual ICollection<ReferencesJobRole> ReferencesJobRoles { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Survey> Surveys { get; set; }

        public virtual ICollection<TermsAndRegulation> TermsAndRegulations { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<JobAdvertisement> JobAdvertisements { get; set; }
        public virtual ICollection<MajorLookup> MajorLookups { get; set; }
        public virtual ICollection<OpenData> OpenDatas { get; set; }
        public virtual ICollection<OpenDataTemp> OpenDataTemps { get; set; }

        public virtual ICollection<OpenDataRequest> OpenDataRequests { get; set; }

        public virtual ICollection<OpenDataStatistics> OpenDataStatistics { get; set; }
        public virtual ICollection<FAQ> FAQs { get; set; }
        public virtual ICollection<ScientificLetters> ScientificLetters { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<LogInformation> LogInformations { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<PublishEntities> PublishEntities { get; set; }
        public virtual ICollection<Form> Form { get; set; }

        public virtual ICollection<UsersEntityReference> UsersEntitiesReferences { get; set; }
        public virtual ICollection<Engine> EnginReferences { get; set; }
        public virtual ICollection<FormValuesActions> FormValuesActionsToReferences { get; set; }
        public virtual ICollection<WorkFlowActions> WorkFlowActionsReferences { get; set; }
        public virtual ICollection<Idea> Ideas { get; set; }
        public virtual ICollection<Idea> IdeasTo { get; set; }
        public virtual ICollection<IdeasCompetentAuthority> IdeasCompetentAuthorities { get; set; }
        public virtual ICollection<Recommendations> Recommendations { get; set; }
        public virtual ICollection<CronSettings> CronSettings { get; set; }
        public virtual ICollection<Feedbacks> Feedbacks { get; set; }

    }
}
