#nullable disable


using RM.Courses.Models;

namespace RM.Models
{
    public partial class Entity
    {
        public Entity()
        {
            AdminMenus = new HashSet<AdminMenu>();
            Advertisements = new HashSet<Advertisement>();
            AmanahAwards = new HashSet<AmanahAward>();
            Articles = new HashSet<Article>();
            Attachments = new HashSet<Attachment>();
            Comments = new HashSet<Comment>();
            ContactUs = new HashSet<ContactU>();
            Documents = new HashSet<Document>();
            Eservices = new HashSet<Eservice>();
            ExternalSites = new HashSet<ExternalSite>();
            InitiativesProjects = new HashSet<InitiativesProject>();
            InteractionStatistics = new HashSet<InteractionStatistic>();
            Menus = new HashSet<Menu>();
            Multimedia = new HashSet<Multimedia>();
            News = new HashSet<News>();
            Officials = new HashSet<Official>();
            Partners = new HashSet<Partner>();
            QuestionsAnswers = new HashSet<QuestionsAnswer>();
            Rates = new HashSet<Rate>();
            ReferenceContents = new HashSet<ReferenceContent>();
            ReferencesMajors = new HashSet<ReferencesMajor>();
            Surveys = new HashSet<Survey>();
            TermsAndRegulations = new HashSet<TermsAndRegulation>();
            UsersEntities = new HashSet<UsersEntity>();
            UsersEntitiesReferences = new HashSet<UsersEntityReference>();

            JobAdvertisements = new HashSet<JobAdvertisement>();
            OpenDatas = new HashSet<OpenData>();
            OpenDataTemps = new HashSet<OpenDataTemp>();

            OpenDataRequests = new HashSet<OpenDataRequest>();
            ScientificLetters = new HashSet<ScientificLetters>();

            Order = new HashSet<Order>();
            LogInformations = new HashSet<LogInformation>();
            Exams = new HashSet<Exam>();
            InverseParent = new HashSet<Entity>();
            FormValue = new HashSet<FormValue>();
            Recommendations = new HashSet<Recommendations>();
            CronSettings = new HashSet<CronSettings>();
            Feedbacks = new HashSet<Feedbacks>();
            FeedbacksAnswerActions = new HashSet<FeedbacksAnswerAction>();
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? TypeId { get; set; }
        public string FrontIdentity { get; set; }
        public string BackendIdentity { get; set; }
        public string CmsIdentity { get; set; }
        public bool? Searchable { get; set; }
        public int? ReferenceId { get; set; }
        public int? ReferencesMajorId { get; set; }
        public bool? ShowMenuIdentifier { get; set; }
        public int? ParentId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Entity Parent { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual ReferencesMajor ReferencesMajor { get; set; }
        public virtual EntitiesType Type { get; set; }
        public virtual ICollection<AdminMenu> AdminMenus { get; set; }
        public virtual ICollection<Advertisement> Advertisements { get; set; }
        public virtual ICollection<AmanahAward> AmanahAwards { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ContactU> ContactUs { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Eservice> Eservices { get; set; }
        public virtual ICollection<ExternalSite> ExternalSites { get; set; }
        public virtual ICollection<InitiativesProject> InitiativesProjects { get; set; }
        public virtual ICollection<InteractionStatistic> InteractionStatistics { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<Multimedia> Multimedia { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Official> Officials { get; set; }
        public virtual ICollection<Partner> Partners { get; set; }
        public virtual ICollection<QuestionsAnswer> QuestionsAnswers { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<ReferenceContent> ReferenceContents { get; set; }
        public virtual ICollection<ReferencesMajor> ReferencesMajors { get; set; }
        public virtual ICollection<Survey> Surveys { get; set; }
        public virtual ICollection<TermsAndRegulation> TermsAndRegulations { get; set; }
        public virtual ICollection<UsersEntity> UsersEntities { get; set; }
        public virtual ICollection<UsersEntityReference> UsersEntitiesReferences { get; set; }

        public virtual ICollection<JobAdvertisement> JobAdvertisements { get; set; }
        public virtual ICollection<OpenData> OpenDatas { get; set; }
        public virtual ICollection<OpenDataTemp> OpenDataTemps { get; set; }


        public virtual ICollection<OpenDataRequest> OpenDataRequests { get; set; }
        public virtual ICollection<ScientificLetters> ScientificLetters { get; set; }

        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<LogInformation> LogInformations { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<PublishEntities> PublishEntities { get; set; }
        public virtual ICollection<Entity> InverseParent { get; set; }
        public virtual ICollection<FormValue> FormValue { get; set; }
        public virtual ICollection<Recommendations> Recommendations { get; set; }
        public virtual ICollection<CronSettings> CronSettings { get; set; }
        public virtual ICollection<Feedbacks> Feedbacks { get; set; }
        public virtual ICollection<FeedbacksAnswerAction>  FeedbacksAnswerActions { get; set; }
        public virtual ICollection<CronSettings> CronSubEntitySettings { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        //public virtual ICollection<CourseLesson> CourseLessons { get; set; }
        //public virtual ICollection<CourseLessonMaterial> CourseLessonMaterials { get; set; }
    


    }
}
