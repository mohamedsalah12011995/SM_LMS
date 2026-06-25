#nullable disable

namespace RM.Models
{
    public partial class User
    {
        public User()
        {
            AdvertisementActivatedByNavigations = new HashSet<Advertisement>();
            AdvertisementCreatedByNavigations = new HashSet<Advertisement>();
            AdvertisementDeletedByNavigations = new HashSet<Advertisement>();
            AdvertisementUpdatedByNavigations = new HashSet<Advertisement>();
            AmanahAwardActivatedByNavigations = new HashSet<AmanahAward>();
            AmanahAwardCreatedByNavigations = new HashSet<AmanahAward>();
            AmanahAwardDeletedByNavigations = new HashSet<AmanahAward>();
            AmanahAwardUpdatedByNavigations = new HashSet<AmanahAward>();
            ArticleActivatedByNavigations = new HashSet<Article>();
            ArticleCreatedByNavigations = new HashSet<Article>();
            ArticleUpdatedByNavigations = new HashSet<Article>();
            ArticlesPublishes = new HashSet<ArticlesPublish>();
            Attachments = new HashSet<Attachment>();
            BeneficiaryCreatedByNavigations = new HashSet<Beneficiary>();
            BeneficiaryDeletedByNavigations = new HashSet<Beneficiary>();
            CommentApprovedByNavigations = new HashSet<Comment>();
            CommentCreatedByNavigations = new HashSet<Comment>();
            CommentRepliedByNavigations = new HashSet<Comment>();
            ContactUCreatedByNavigations = new HashSet<ContactU>();
            ContactUModifiedByNavigations = new HashSet<ContactU>();
            DocumentActivatedByNavigations = new HashSet<Document>();
            DocumentCreatedByNavigations = new HashSet<Document>();
            DocumentDeletedByNavigations = new HashSet<Document>();
            DocumentUpdatedByNavigations = new HashSet<Document>();
            EserviceActivatedByNavigations = new HashSet<Eservice>();
            EserviceCreatedByNavigations = new HashSet<Eservice>();
            EserviceDeletedByNavigations = new HashSet<Eservice>();
            EserviceUpdatedByNavigations = new HashSet<Eservice>();
            ExternalSiteActivatedByNavigations = new HashSet<ExternalSite>();
            ExternalSiteCreatedByNavigations = new HashSet<ExternalSite>();
            ExternalSiteDeletedByNavigations = new HashSet<ExternalSite>();
            ExternalSiteUpdatedByNavigations = new HashSet<ExternalSite>();
            GovServiceActivatedByNavigations = new HashSet<GovService>();
            GovServiceCreatedByNavigations = new HashSet<GovService>();
            GovServiceDeletedByNavigations = new HashSet<GovService>();
            GovServiceUpdatedByNavigations = new HashSet<GovService>();
            InitiativesProjectActivatedByNavigations = new HashSet<InitiativesProject>();
            InitiativesProjectCreatedByNavigations = new HashSet<InitiativesProject>();
            InitiativesProjectDeletedByNavigations = new HashSet<InitiativesProject>();
            InitiativesProjectUpdatedByNavigations = new HashSet<InitiativesProject>();
            InvestmentActivatedByNavigations = new HashSet<Investment>();
            InvestmentCreatedByNavigations = new HashSet<Investment>();
            InvestmentDeletedByNavigations = new HashSet<Investment>();
            InvestmentUpdatedByNavigations = new HashSet<Investment>();
            Menus = new HashSet<Menu>();
            MobileApplicationActivatedByNavigations = new HashSet<MobileApplication>();
            MobileApplicationCreatedByNavigations = new HashSet<MobileApplication>();
            MobileApplicationDeletedByNavigations = new HashSet<MobileApplication>();
            MobileApplicationUpdatedByNavigations = new HashSet<MobileApplication>();
            MultimediaActivatedByNavigations = new HashSet<Multimedia>();
            MultimediaCreatedByNavigations = new HashSet<Multimedia>();
            MultimediaDeletedByNavigations = new HashSet<Multimedia>();
            MultimediaUpdatedByNavigations = new HashSet<Multimedia>();
            NewsActivatedByNavigations = new HashSet<News>();
            NewsCreatedByNavigations = new HashSet<News>();
            NewsDeletedByNavigations = new HashSet<News>();
            NewsUpdatedByNavigations = new HashSet<News>();
            OfficialActivatedByNavigations = new HashSet<Official>();
            OfficialCreatedByNavigations = new HashSet<Official>();
            OfficialDeletedByNavigations = new HashSet<Official>();
            OfficialUpdatedByNavigations = new HashSet<Official>();
            PartnerActivatedByNavigations = new HashSet<Partner>();
            PartnerCreatedByNavigations = new HashSet<Partner>();
            PartnerDeletedByNavigations = new HashSet<Partner>();
            PartnerUpdatedByNavigations = new HashSet<Partner>();
            QuestionsAnswerCreatedByNavigations = new HashSet<QuestionsAnswer>();
            QuestionsAnswerUpdatedByNavigations = new HashSet<QuestionsAnswer>();
            Rates = new HashSet<Rate>();
            ReferenceCreatedByNavigations = new HashSet<Reference>();
            ReferenceUpdatedByNavigations = new HashSet<Reference>();
            ReferencesMajorCreatedByNavigations = new HashSet<ReferencesMajor>();
            ReferencesMajorUpdatedByNavigations = new HashSet<ReferencesMajor>();
            Sessions = new HashSet<Session>();
            TermsAndRegulationActivatedByNavigations = new HashSet<TermsAndRegulation>();
            TermsAndRegulationCreatedByNavigations = new HashSet<TermsAndRegulation>();
            TermsAndRegulationDeletedByNavigations = new HashSet<TermsAndRegulation>();
            TermsAndRegulationUpdatedByNavigations = new HashSet<TermsAndRegulation>();
            UsersEntities = new HashSet<UsersEntity>();
            UsersPermissionLevels = new HashSet<UsersPermissionLevel>();
            UsersEntitiesReferences = new HashSet<UsersEntityReference>();

            JobAdvertisementActivatedByNavigations = new HashSet<JobAdvertisement>();
            JobAdvertisementCreatedByNavigations = new HashSet<JobAdvertisement>();
            JobAdvertisementDeletedByNavigations = new HashSet<JobAdvertisement>();
            JobAdvertisementUpdatedByNavigations = new HashSet<JobAdvertisement>();

            PermitsRequestActivatedByNavigations = new HashSet<PermitsRequest>();
            PermitsRequestCreatedByNavigations = new HashSet<PermitsRequest>();
            PermitsRequestDeletedByNavigations = new HashSet<PermitsRequest>();
            PermitsRequestUpdatedByNavigations = new HashSet<PermitsRequest>();

            PrintRequestCreatedByNavigations = new HashSet<PermitAction>();
            PrintRequestUpdatedByNavigations = new HashSet<PermitAction>();

            OpenDataCreatedByNavigations = new HashSet<OpenData>();
            OpenDataModifiedByNavigations = new HashSet<OpenData>();
            OpenDataTempCreatedByNavigations = new HashSet<OpenDataTemp>();
            OpenDataTempModifiedByNavigations = new HashSet<OpenDataTemp>();
            OpenDataRequestCreatedByNavigations = new HashSet<OpenDataRequest>();
            OpenDataRequestModifiedByNavigations = new HashSet<OpenDataRequest>();

            FAQCreatedByNavigations = new HashSet<FAQ>();
            FAQUpdatedByNavigations = new HashSet<FAQ>();
            ActionsCreatedByNavigations = new HashSet<Actions>();
            ActionsFromUserNavigations = new HashSet<Actions>();

            SurveyCreatedByNavigations = new HashSet<Survey>();
            SurveyDeletedByNavigations = new HashSet<Survey>();
            SurveyUpdatedByNavigations = new HashSet<Survey>();

            SurveyQuestionCreatedByNavigations = new HashSet<SurveyQuestion>();
            SurveyQuestionUpdatedByNavigations = new HashSet<SurveyQuestion>();
            SurveyQuestionDeletedByNavigations = new HashSet<SurveyQuestion>();

            SurveyDataSourceCreatedByNavigations = new HashSet<SurveyDataSource>();
            SurveyDataSourceDeletedByNavigations = new HashSet<SurveyDataSource>();
            SurveyDataSourceUpdatedByNavigations = new HashSet<SurveyDataSource>();

            SurveyAnswerActionCreatedByNavigations = new HashSet<SurveyAnswerAction>();

            ScientificLettersActivatedByNavigations = new HashSet<ScientificLetters>();
            ScientificLettersCreatedByNavigations = new HashSet<ScientificLetters>();
            ScientificLettersDeletedByNavigations = new HashSet<ScientificLetters>();
            ScientificLettersUpdatedByNavigations = new HashSet<ScientificLetters>();

            OrderCreatedByNavigations = new HashSet<Order>();
            OrderDeletedByNavigations = new HashSet<Order>();
            OrderActionsCreatedByNavigations = new HashSet<OrderActions>();

            ExamCreatedByNavigations = new HashSet<Exam>();
            ExamDeletedByNavigations = new HashSet<Exam>();
            ExamUpdatedByNavigations = new HashSet<Exam>();

            ExamQuestionCreatedByNavigations = new HashSet<ExamQuestion>();
            ExamQuestionUpdatedByNavigations = new HashSet<ExamQuestion>();
            ExamQuestionDeletedByNavigations = new HashSet<ExamQuestion>();

            ExamDataSourceCreatedByNavigations = new HashSet<ExamDataSource>();
            ExamDataSourceDeletedByNavigations = new HashSet<ExamDataSource>();
            ExamDataSourceUpdatedByNavigations = new HashSet<ExamDataSource>();

            ExamAnswerActionCreatedByNavigations = new HashSet<ExamAnswerAction>();

            JobApplicationExamCreatedByNavigations = new HashSet<JobApplicationExams>();
            JobApplicationExamUpdatedByNavigations = new HashSet<JobApplicationExams>();
            JobApplicationExamDeletedByNavigations = new HashSet<JobApplicationExams>();
            ProjectsUsers = new HashSet<ProjectsUsers>();

            ProjectsCreatedByNavigations = new HashSet<Project>();
            ProjectsUpdatedByNavigations = new HashSet<Project>();
            ProjectsDeletedByNavigations = new HashSet<Project>();
            PublishEntityCreatedByNavigations = new HashSet<PublishEntities>();
            FormCreatedByNavigations = new HashSet<Form>();
            FormUpdatedByNavigations = new HashSet<Form>();
            FormValueUpdatedByNavigations = new HashSet<FormValue>();
            FormValueCreatedByNavigations = new HashSet<FormValue>();

            EngineCreatedByNavigations = new HashSet<Engine>();
            EngineUpdatedByNavigations = new HashSet<Engine>();

            WorkFlowActionsCreatedByNavigations = new HashSet<WorkFlowActions>();
            WorkFlowActionsUpdatedByNavigations = new HashSet<WorkFlowActions>();

            EnginesActionsJobRoleCreatedByNavigations = new HashSet<EngineActionJobRole>();
            EnginesActionsJobRoleUpdatedByNavigations = new HashSet<EngineActionJobRole>();


            FormInputsActionsCreatedByNavigations = new HashSet<FormInputsActions>();
            FormInputsActionsUpdatedByNavigations = new HashSet<FormInputsActions>();

            FormValuesActionsCreatedByNavigations = new HashSet<FormValuesActions>();
            FromUserNavigations = new HashSet<FormValuesActions>();

            IdeaActions = new HashSet<IdeaAction>();
            CommentApprovedByNavigation = new HashSet<IdeaComment>();
            CommentCreatedByNavigation = new HashSet<IdeaComment>();
            CommentRepliedByNavigation = new HashSet<IdeaComment>();

            IdeaActivatedByNavigations = new HashSet<Idea>();
            IdeaCreatedByNavigations = new HashSet<Idea>();
            IdeaDeletedByNavigations = new HashSet<Idea>();
            IdeaUpdatedByNavigations = new HashSet<Idea>();

            RecommendationsCreatedByNavigations = new HashSet<Recommendations>();
            RecommendationsUpdatedByNavigations = new HashSet<Recommendations>();

            CronSettingsCreatedByNavigations = new HashSet<CronSettings>();
            CronSettingsUpdatedByNavigations = new HashSet<CronSettings>();

            FeedbacksCreatedByNavigations = new HashSet<Feedbacks>();
            FeedbacksDeletedByNavigations = new HashSet<Feedbacks>();
            FeedbacksUpdatedByNavigations = new HashSet<Feedbacks>();

            CourseActivatedByNavigations = new HashSet<Course>();
            CourseCreatedByNavigations = new HashSet<Course>();
            CourseDeletedByNavigations = new HashSet<Course>();
            CourseUpdatedByNavigations = new HashSet<Course>();

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmployeeId { get; set; }
        public string DomainUser { get; set; }
        public string IdCardNumber { get; set; }
        public int? ReferenceId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
        public int? RoleId { get; set; }
        public int? NationalityId { get; set; }
        public int? GenderId { get; set; }
        public int? CommunicateWayId { get; set; }
        public int? YearsOfExperienceId { get; set; }
        public int? CountryOfResidenceId { get; set; }
        public int? CityOfResidenceId { get; set; }
        public int? EducationalQualificationsId { get; set; }
        public int? WorkAreaId { get; set; }
        public bool? AcceptWorkOnSite { get; set; }
        public bool? WorkInGovSectors { get; set; }
        public string Details { get; set; }
        public string Cv { get; set; }
        public int? LoginWayId { get; set; }
        public int? JobRole { get; set; }

        public string OTP { get; set; }

        public DateTime? OTPDate { get; set; }


        public virtual Reference Reference { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Advertisement> AdvertisementActivatedByNavigations { get; set; }
        public virtual ICollection<Advertisement> AdvertisementCreatedByNavigations { get; set; }
        public virtual ICollection<Advertisement> AdvertisementDeletedByNavigations { get; set; }
        public virtual ICollection<Advertisement> AdvertisementUpdatedByNavigations { get; set; }
        public virtual ICollection<AmanahAward> AmanahAwardActivatedByNavigations { get; set; }
        public virtual ICollection<AmanahAward> AmanahAwardCreatedByNavigations { get; set; }
        public virtual ICollection<AmanahAward> AmanahAwardDeletedByNavigations { get; set; }
        public virtual ICollection<AmanahAward> AmanahAwardUpdatedByNavigations { get; set; }
        public virtual ICollection<Article> ArticleActivatedByNavigations { get; set; }
        public virtual ICollection<Article> ArticleCreatedByNavigations { get; set; }
        public virtual ICollection<Article> ArticleUpdatedByNavigations { get; set; }
        public virtual ICollection<ArticlesPublish> ArticlesPublishes { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<Beneficiary> BeneficiaryCreatedByNavigations { get; set; }
        public virtual ICollection<Beneficiary> BeneficiaryDeletedByNavigations { get; set; }
        public virtual ICollection<Comment> CommentApprovedByNavigations { get; set; }
        public virtual ICollection<Comment> CommentCreatedByNavigations { get; set; }
        public virtual ICollection<Comment> CommentRepliedByNavigations { get; set; }
        public virtual ICollection<ContactU> ContactUCreatedByNavigations { get; set; }
        public virtual ICollection<ContactU> ContactUModifiedByNavigations { get; set; }
        public virtual ICollection<Document> DocumentActivatedByNavigations { get; set; }
        public virtual ICollection<Document> DocumentCreatedByNavigations { get; set; }
        public virtual ICollection<Document> DocumentDeletedByNavigations { get; set; }
        public virtual ICollection<Document> DocumentUpdatedByNavigations { get; set; }
        public virtual ICollection<Eservice> EserviceActivatedByNavigations { get; set; }
        public virtual ICollection<Eservice> EserviceCreatedByNavigations { get; set; }
        public virtual ICollection<Eservice> EserviceDeletedByNavigations { get; set; }
        public virtual ICollection<Eservice> EserviceUpdatedByNavigations { get; set; }
        public virtual ICollection<ExternalSite> ExternalSiteActivatedByNavigations { get; set; }
        public virtual ICollection<ExternalSite> ExternalSiteCreatedByNavigations { get; set; }
        public virtual ICollection<ExternalSite> ExternalSiteDeletedByNavigations { get; set; }
        public virtual ICollection<ExternalSite> ExternalSiteUpdatedByNavigations { get; set; }
        public virtual ICollection<GovService> GovServiceActivatedByNavigations { get; set; }
        public virtual ICollection<GovService> GovServiceCreatedByNavigations { get; set; }
        public virtual ICollection<GovService> GovServiceDeletedByNavigations { get; set; }
        public virtual ICollection<GovService> GovServiceUpdatedByNavigations { get; set; }
        public virtual ICollection<InitiativesProject> InitiativesProjectActivatedByNavigations { get; set; }
        public virtual ICollection<InitiativesProject> InitiativesProjectCreatedByNavigations { get; set; }
        public virtual ICollection<InitiativesProject> InitiativesProjectDeletedByNavigations { get; set; }
        public virtual ICollection<InitiativesProject> InitiativesProjectUpdatedByNavigations { get; set; }
        public virtual ICollection<Investment> InvestmentActivatedByNavigations { get; set; }
        public virtual ICollection<Investment> InvestmentCreatedByNavigations { get; set; }
        public virtual ICollection<Investment> InvestmentDeletedByNavigations { get; set; }
        public virtual ICollection<Investment> InvestmentUpdatedByNavigations { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<MobileApplication> MobileApplicationActivatedByNavigations { get; set; }
        public virtual ICollection<MobileApplication> MobileApplicationCreatedByNavigations { get; set; }
        public virtual ICollection<MobileApplication> MobileApplicationDeletedByNavigations { get; set; }
        public virtual ICollection<MobileApplication> MobileApplicationUpdatedByNavigations { get; set; }
        public virtual ICollection<Multimedia> MultimediaActivatedByNavigations { get; set; }
        public virtual ICollection<Multimedia> MultimediaCreatedByNavigations { get; set; }
        public virtual ICollection<Multimedia> MultimediaDeletedByNavigations { get; set; }
        public virtual ICollection<Multimedia> MultimediaUpdatedByNavigations { get; set; }
        public virtual ICollection<News> NewsActivatedByNavigations { get; set; }
        public virtual ICollection<News> NewsCreatedByNavigations { get; set; }
        public virtual ICollection<News> NewsDeletedByNavigations { get; set; }
        public virtual ICollection<News> NewsUpdatedByNavigations { get; set; }
        public virtual ICollection<Official> OfficialActivatedByNavigations { get; set; }
        public virtual ICollection<Official> OfficialCreatedByNavigations { get; set; }
        public virtual ICollection<Official> OfficialDeletedByNavigations { get; set; }
        public virtual ICollection<Official> OfficialUpdatedByNavigations { get; set; }
        public virtual ICollection<Partner> PartnerActivatedByNavigations { get; set; }
        public virtual ICollection<Partner> PartnerCreatedByNavigations { get; set; }
        public virtual ICollection<Partner> PartnerDeletedByNavigations { get; set; }
        public virtual ICollection<Partner> PartnerUpdatedByNavigations { get; set; }
        public virtual ICollection<QuestionsAnswer> QuestionsAnswerCreatedByNavigations { get; set; }
        public virtual ICollection<QuestionsAnswer> QuestionsAnswerUpdatedByNavigations { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<Reference> ReferenceCreatedByNavigations { get; set; }
        public virtual ICollection<Reference> ReferenceUpdatedByNavigations { get; set; }
        public virtual ICollection<ReferencesMajor> ReferencesMajorCreatedByNavigations { get; set; }
        public virtual ICollection<ReferencesMajor> ReferencesMajorUpdatedByNavigations { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<TermsAndRegulation> TermsAndRegulationActivatedByNavigations { get; set; }
        public virtual ICollection<TermsAndRegulation> TermsAndRegulationCreatedByNavigations { get; set; }
        public virtual ICollection<TermsAndRegulation> TermsAndRegulationDeletedByNavigations { get; set; }
        public virtual ICollection<TermsAndRegulation> TermsAndRegulationUpdatedByNavigations { get; set; }
        public virtual ICollection<UsersEntity> UsersEntities { get; set; }
        public virtual ICollection<UsersEntityReference> UsersEntitiesReferences { get; set; }

        public virtual ICollection<UsersPermissionLevel> UsersPermissionLevels { get; set; }

        public virtual ICollection<JobAdvertisement> JobAdvertisementActivatedByNavigations { get; set; }
        public virtual ICollection<JobAdvertisement> JobAdvertisementCreatedByNavigations { get; set; }
        public virtual ICollection<JobAdvertisement> JobAdvertisementDeletedByNavigations { get; set; }
        public virtual ICollection<JobAdvertisement> JobAdvertisementUpdatedByNavigations { get; set; }

        public virtual ICollection<PermitsRequest> PermitsRequestActivatedByNavigations { get; set; }
        public virtual ICollection<PermitsRequest> PermitsRequestCreatedByNavigations { get; set; }
        public virtual ICollection<PermitsRequest> PermitsRequestDeletedByNavigations { get; set; }
        public virtual ICollection<PermitsRequest> PermitsRequestUpdatedByNavigations { get; set; }

        public virtual ICollection<PermitAction> PrintRequestCreatedByNavigations { get; set; }
        public virtual ICollection<PermitAction> PrintRequestUpdatedByNavigations { get; set; }
        public virtual ICollection<OpenData> OpenDataCreatedByNavigations { get; set; }
        public virtual ICollection<OpenData> OpenDataModifiedByNavigations { get; set; }

        public virtual ICollection<OpenDataTemp> OpenDataTempCreatedByNavigations { get; set; }
        public virtual ICollection<OpenDataTemp> OpenDataTempModifiedByNavigations { get; set; }
        public virtual ICollection<OpenDataRequest> OpenDataRequestCreatedByNavigations { get; set; }
        public virtual ICollection<OpenDataRequest> OpenDataRequestModifiedByNavigations { get; set; }
        public virtual ICollection<FAQ> FAQCreatedByNavigations { get; set; }
        public virtual ICollection<FAQ> FAQUpdatedByNavigations { get; set; }
        public virtual ICollection<Actions> ActionsCreatedByNavigations { get; set; }
        public virtual ICollection<Actions> ActionsFromUserNavigations { get; set; }


        public virtual ICollection<Survey> SurveyCreatedByNavigations { get; set; }
        public virtual ICollection<Survey> SurveyDeletedByNavigations { get; set; }
        public virtual ICollection<Survey> SurveyUpdatedByNavigations { get; set; }

        public virtual ICollection<SurveyQuestion> SurveyQuestionCreatedByNavigations { get; set; }
        public virtual ICollection<SurveyQuestion> SurveyQuestionUpdatedByNavigations { get; set; }

        public virtual ICollection<SurveyQuestion> SurveyQuestionDeletedByNavigations { get; set; }

        public virtual ICollection<SurveyDataSource> SurveyDataSourceCreatedByNavigations { get; set; }
        public virtual ICollection<SurveyDataSource> SurveyDataSourceDeletedByNavigations { get; set; }
        public virtual ICollection<SurveyDataSource> SurveyDataSourceUpdatedByNavigations { get; set; }

        public virtual ICollection<SurveyAnswerAction> SurveyAnswerActionCreatedByNavigations { get; set; }

        public virtual ICollection<ScientificLetters> ScientificLettersActivatedByNavigations { get; set; }
        public virtual ICollection<ScientificLetters> ScientificLettersCreatedByNavigations { get; set; }
        public virtual ICollection<ScientificLetters> ScientificLettersDeletedByNavigations { get; set; }
        public virtual ICollection<ScientificLetters> ScientificLettersUpdatedByNavigations { get; set; }
        public virtual ICollection<Order> OrderCreatedByNavigations { get; set; }
        public virtual ICollection<Order> OrderDeletedByNavigations { get; set; }
        public virtual ICollection<OrderActions> OrderActionsCreatedByNavigations { get; set; }

        public virtual ICollection<Exam> ExamCreatedByNavigations { get; set; }
        public virtual ICollection<Exam> ExamDeletedByNavigations { get; set; }
        public virtual ICollection<Exam> ExamUpdatedByNavigations { get; set; }

        public virtual ICollection<ExamQuestion> ExamQuestionCreatedByNavigations { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestionUpdatedByNavigations { get; set; }

        public virtual ICollection<ExamQuestion> ExamQuestionDeletedByNavigations { get; set; }

        public virtual ICollection<ExamDataSource> ExamDataSourceCreatedByNavigations { get; set; }
        public virtual ICollection<ExamDataSource> ExamDataSourceDeletedByNavigations { get; set; }
        public virtual ICollection<ExamDataSource> ExamDataSourceUpdatedByNavigations { get; set; }

        public virtual ICollection<ExamAnswerAction> ExamAnswerActionCreatedByNavigations { get; set; }

        public virtual ICollection<JobApplicationExams> JobApplicationExamCreatedByNavigations { get; set; }
        public virtual ICollection<JobApplicationExams> JobApplicationExamUpdatedByNavigations { get; set; }
        public virtual ICollection<JobApplicationExams> JobApplicationExamDeletedByNavigations { get; set; }
        public virtual ICollection<ProjectsUsers> ProjectsUsers { get; set; }
        public virtual ICollection<Project> ProjectsCreatedByNavigations { get; set; }
        public virtual ICollection<Project> ProjectsUpdatedByNavigations { get; set; }
        public virtual ICollection<Project> ProjectsDeletedByNavigations { get; set; }
        public virtual ICollection<PublishEntities> PublishEntityCreatedByNavigations { get; set; }
        public virtual ICollection<Form> FormCreatedByNavigations { get; set; }
        public virtual ICollection<Form> FormUpdatedByNavigations { get; set; }
        public virtual ICollection<FormValue> FormValueUpdatedByNavigations { get; set; }
        public virtual ICollection<FormValue> FormValueCreatedByNavigations { get; set; }

        public virtual ICollection<Engine> EngineCreatedByNavigations { get; set; }
        public virtual ICollection<Engine> EngineUpdatedByNavigations { get; set; }
        public virtual ICollection<WorkFlowActions> WorkFlowActionsCreatedByNavigations { get; set; }
        public virtual ICollection<WorkFlowActions> WorkFlowActionsUpdatedByNavigations { get; set; }
        public virtual ICollection<EngineActionJobRole> EnginesActionsJobRoleCreatedByNavigations { get; set; }
        public virtual ICollection<EngineActionJobRole> EnginesActionsJobRoleUpdatedByNavigations { get; set; }

        public virtual ICollection<FormInputsActions> FormInputsActionsCreatedByNavigations { get; set; }
        public virtual ICollection<FormInputsActions> FormInputsActionsUpdatedByNavigations { get; set; }
        public virtual ICollection<FormValuesActions> FormValuesActionsCreatedByNavigations { get; set; }
        public virtual ICollection<FormValuesActions> FromUserNavigations { get; set; }

        public virtual ICollection<IdeaAction> IdeaActions { get; set; }
        public virtual ICollection<Idea> IdeaActivatedByNavigations { get; set; }
        public virtual ICollection<Idea> IdeaCreatedByNavigations { get; set; }
        public virtual ICollection<Idea> IdeaDeletedByNavigations { get; set; }
        public virtual ICollection<Idea> IdeaUpdatedByNavigations { get; set; }

        public virtual ICollection<IdeaComment> CommentApprovedByNavigation { get; set; }
        public virtual ICollection<IdeaComment> CommentCreatedByNavigation { get; set; }
        public virtual ICollection<IdeaComment> CommentRepliedByNavigation { get; set; }

        public virtual ICollection<Recommendations> RecommendationsCreatedByNavigations { get; set; }
        public virtual ICollection<Recommendations> RecommendationsUpdatedByNavigations { get; set; }

        public virtual ICollection<CronSettings> CronSettingsCreatedByNavigations { get; set; }
        public virtual ICollection<CronSettings> CronSettingsUpdatedByNavigations { get; set; }

        public virtual ICollection<Feedbacks> FeedbacksCreatedByNavigations { get; set; }
        public virtual ICollection<Feedbacks> FeedbacksDeletedByNavigations { get; set; }
        public virtual ICollection<Feedbacks> FeedbacksUpdatedByNavigations { get; set; }

        public virtual ICollection<Course> CourseActivatedByNavigations { get; set; }
        public virtual ICollection<Course> CourseCreatedByNavigations { get; set; }
        public virtual ICollection<Course> CourseDeletedByNavigations { get; set; }
        public virtual ICollection<Course> CourseUpdatedByNavigations { get; set; }



    }
}
