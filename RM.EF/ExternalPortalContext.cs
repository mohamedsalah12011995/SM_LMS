
#nullable disable

using Microsoft.EntityFrameworkCore;
using RM.Courses.Models;
using RM.EF.Models.Course;
using RM.Models.Extensions;
using System.Diagnostics.Metrics;


#nullable disable

namespace RM.Models
{
    public class ExternalPortal_v2Context : DbContext
    {

        public ExternalPortal_v2Context(DbContextOptions<ExternalPortal_v2Context> options)
            : base(options)
        {
        }


        public virtual DbSet<AdminMenu> AdminMenus { get; set; }
        public virtual DbSet<Advertisement> Advertisements { get; set; }
        public virtual DbSet<AmanahAward> AmanahAwards { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ArticlesPublish> ArticlesPublishes { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Beneficiary> Beneficiaries { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ContactU> ContactUs { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentsType> DocumentsTypes { get; set; }
        public virtual DbSet<EntitiesType> EntitiesTypes { get; set; }
        public virtual DbSet<Entity> Entities { get; set; }
        public virtual DbSet<Eservice> Eservices { get; set; }
        public virtual DbSet<ExternalSite> ExternalSites { get; set; }
        public virtual DbSet<GovService> GovServices { get; set; }
        public virtual DbSet<InitiativesProject> InitiativesProjects { get; set; }
        public virtual DbSet<InitiativesProjectsBeneficiary> InitiativesProjectsBeneficiaries { get; set; }
        public virtual DbSet<InitiativesProjectsType> InitiativesProjectsTypes { get; set; }
        public virtual DbSet<InteractionStatistic> InteractionStatistics { get; set; }
        public virtual DbSet<InteractionStatisticsType> InteractionStatisticsTypes { get; set; }
        public virtual DbSet<Investment> Investments { get; set; }
        public virtual DbSet<InvestmentType> InvestmentTypes { get; set; }
        public virtual DbSet<JobRole> JobRoles { get; set; }
        public virtual DbSet<LoginWay> LoginWays { get; set; }
        public virtual DbSet<MajorLookup> MajorLookups { get; set; }
        public virtual DbSet<MajorLookupsType> MajorLookupsType { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuType> MenuTypes { get; set; }
        public virtual DbSet<MobileApplication> MobileApplications { get; set; }
        public virtual DbSet<Multimedia> Multimedias { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Official> Officials { get; set; }
        public virtual DbSet<Partner> Partners { get; set; }
        public virtual DbSet<PermissionLevel> PermissionLevels { get; set; }
        public virtual DbSet<QuestionsAnswer> QuestionsAnswers { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<Reference> References { get; set; }
        public virtual DbSet<ReferenceContent> ReferenceContents { get; set; }
        public virtual DbSet<ReferencesJobRole> ReferencesJobRoles { get; set; }
        public virtual DbSet<ReferencesMajor> ReferencesMajors { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolesPermissionLevel> RolesPermissionLevels { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TermsAndRegulation> TermsAndRegulations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersEntity> UsersEntities { get; set; }
        public virtual DbSet<UsersEntityReference> UsersEntitiesReferences { get; set; }

        public virtual DbSet<UsersPermissionLevel> UsersPermissionLevels { get; set; }
        public virtual DbSet<Volunteer> Volunteers { get; set; }
        public virtual DbSet<EntitiesLatestUpdate> EntitiesLatestUpdate { get; set; }
        public virtual DbSet<JobAdvertisement> JobAdvertisement { get; set; }
        public virtual DbSet<JobCareer> JobCareers { get; set; }
        public virtual DbSet<JobApplication> JobApplications { get; set; }
        public virtual DbSet<JobLookUp> JobLookUp { get; set; }
        public virtual DbSet<PermitsRequest> PermitsRequests { get; set; }
        public virtual DbSet<PermitAction> PermitActions { get; set; }
        public virtual DbSet<PermitWorkSite> PermitWorkSites { get; set; }
        public virtual DbSet<OpenDataTemp> OpenDataTemp { get; set; }
        public virtual DbSet<OpenData> OpenData { get; set; }
        public virtual DbSet<OpenDataRequest> OpenDataRequests { get; set; }
        public virtual DbSet<OpenDataStatistics> OpenDataStatistics { get; set; }

        public virtual DbSet<FAQ> FAQ { get; set; }
        public virtual DbSet<MajorStatus> MajorStatus { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Actions> Actions { get; set; }

        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public virtual DbSet<SurveyDataSource> SurveyDataSources { get; set; }
        public virtual DbSet<SurveyAnswerAction> SurveyAnswerActions { get; set; }
        public virtual DbSet<SurveyQuestionAnswer> SurveyQuestionAnswers { get; set; }
        public virtual DbSet<SurveyQuestionType> SurveyQuestionTypes { get; set; }
        public virtual DbSet<QuestionsRecommendations> QuestionsRecommendations { get; set; }
        public virtual DbSet<Recommendations> Recommendations { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<ScientificLetters> ScientificLetters { get; set; }

        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderActions> OrderActions { get; set; }

        public virtual DbSet<ActionFiles> ActionFiles { get; set; }
        public virtual DbSet<LogInformation> LogInformation { get; set; }


        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }
        public virtual DbSet<ExamDataSource> ExamDataSources { get; set; }
        public virtual DbSet<ExamAnswerAction> ExamAnswerActions { get; set; }
        public virtual DbSet<ExamQuestionAnswer> ExamQuestionAnswers { get; set; }
        public virtual DbSet<ExamQuestionType> ExamQuestionTypes { get; set; }

        public virtual DbSet<JobApplicationExams> JobApplicationExams { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectsUsers> ProjectsUsers { get; set; }
        public virtual DbSet<FlowStepper> FlowStepper { get; set; }
        public virtual DbSet<FlowStepperProjects> FlowStepperProjects { get; set; }
        public virtual DbSet<PublishEntities> PublishEntities { get; set; }
        public virtual DbSet<Form> Form { get; set; }
        public virtual DbSet<FormInput> FormInput { get; set; }
        public virtual DbSet<FormValue> FormValue { get; set; }
        public virtual DbSet<InputsType> InputsType { get; set; }
        public virtual DbSet<FormsDataSource> FormsDataSource { get; set; }
        public virtual DbSet<FormInputDataSource> FormInputDataSource { get; set; }
        public virtual DbSet<FormsEntity> FormsEntity { get; set; }
        public virtual DbSet<FormValueDetails> FormValueDetails { get; set; }
        public virtual DbSet<FormValueViewStatistic> FormValueViewStatistic { get; set; }

        // Courses 
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseLessonMaterial> CourseLessonMaterials { get; set; }
        public virtual DbSet<CourseLesson> CourseLessons { get; set; }
        public virtual DbSet<CourseInstructor> CourseInstructors { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<CourseLearningOutcome> CourseLearningOutcomes { get; set; }
        public virtual DbSet<CourseCategory> CourseCategorys { get; set; }
        public virtual DbSet<CoursePrerequisite> CoursePrerequisites { get; set; }
        public virtual DbSet<CourseTag> CourseTags { get; set; }
        public virtual DbSet<CourseTagMapping> CourseTagMappings { get; set; }
        public virtual DbSet<CourseTargetAudience> CourseTargetAudiences { get; set; }
        public virtual DbSet<CourseSection> CourseSections { get; set; }


        public virtual DbSet<Engine> Engine { get; set; }
        public virtual DbSet<WorkFlowActions> WorkFlowActions { get; set; }
        public virtual DbSet<EngineActionJobRole> EnginesActionsJobRole { get; set; }
        public virtual DbSet<FormInputsActions> FormInputsActions { get; set; }
        public virtual DbSet<FormValuesActions> FormValuesActions { get; set; }

        public virtual DbSet<Theme> Theme { get; set; }
        public virtual DbSet<APIDataSource> APIDataSources { get; set; }

        public virtual DbSet<Idea> Ideas { get; set; }
        public virtual DbSet<IdeaAction> IdeaActions { get; set; }
        public virtual DbSet<IdeasCompetentAuthority> IdeasCompetentAuthorities { get; set; }
        public virtual DbSet<IdeaComment> IdeaComments { get; set; }
        public virtual DbSet<SurveyTheme> SurveyTheme { get; set; }
        public virtual DbSet<CronSettings> CronSettings { get; set; }

        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<FeedbacksDataSource> FeedbackDataSources { get; set; }
        public virtual DbSet<FeedbacksAnswer> FeedbackAnswers { get; set; }
        public virtual DbSet<FeedbacksAnswerAction> FeedbackAnswerActions { get; set; }

        // Exam TrainingCourses

        public virtual DbSet<TrainingCourse> TrainingCourses { get; set; }
        public virtual DbSet<CourseAdvertisement> CourseAdvertisements { get; set; }
        public virtual DbSet<AdvertisementsCourses> AdvertisementsCourses { get; set; }
        public virtual DbSet<TrainingCourseSchedule> TrainingCourseSchedule { get; set; }
        public virtual DbSet<InternalCourseTrainees> InternalCourseTrainees { get; set; }
        public virtual DbSet<InternalCourseExams> InternalCourseExams { get; set; }
        public virtual DbSet<ExternalCourseExams> ExternalCourseExams { get; set; }
        public virtual DbSet<ExternalCourseTraniees> ExternalCourseTraniees { get; set; }
        public virtual DbSet<TrainingCourseType> TrainingCourseType { get; set; }
        public virtual DbSet<ExamExternalTranieesAnswerAction> ExamExternalTranieesAnswerAction { get; set; }
        public virtual DbSet<ExamExternalTranieesQuestionAnswer> ExamExternalTranieesQuestionAnswer { get; set; }

        public virtual DbSet<ExamInternalTranieesAnswerAction> ExamInternalTranieesAnswerAction { get; set; }
        public virtual DbSet<ExamInternalTranieesQuestionAnswer> ExamInternalTranieesQuestionAnswer { get; set; }
        public virtual DbSet<Certificate> Certificates { get; set; }
        public virtual DbSet<CertificateThemes> CertificateThemes { get; set; }
        public virtual DbSet<CertificateLog> CertificateLog { get; set; }

        // Exam Srandalone
        public virtual DbSet<UserApplicationExam> UserApplicationExam { get; set; }
        public virtual DbSet<ExamUserApplicationAnswerAction> ExamUserApplicationAnswerAction { get; set; }
        public virtual DbSet<ExamUserApplicationQuestionAnswer> ExamUserApplicationQuestionAnswer { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configure();


        }


    }
}
