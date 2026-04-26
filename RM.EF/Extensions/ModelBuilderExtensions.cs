
using Microsoft.EntityFrameworkCore;

namespace RM.Models.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Extention Method that calls Entities FluentAPI Extension Methods 
        /// </summary>
        /// <param name="builder"></param>
        public static void Configure(this ModelBuilder builder)
        {
            builder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            builder.ConfigureEntitiesToView();
            builder.AdminMenuConfiguration();
            builder.AdvertisementConfiguration();
            builder.AwardConfiguration();
            builder.ArticleConfiguration();
            builder.ArticlesPublishConfiguration();
            builder.AttachmentConfiguration();
            builder.BeneficiaryConfiguration();
            builder.CommentConfiguration();
            builder.ContactUsConfiguration();
            builder.DocumentConfiguration();
            builder.DocumentsTypeConfiguration();
            builder.EntitiesTypeConfiguration();
            builder.EntityConfiguration();
            builder.EserviceConfiguration();
            builder.ExternalSiteConfiguration();
            builder.GovServiceConfiguration();
            builder.InitiativesProjectConfiguration();
            builder.InitiativesProjectsBeneficiaryConfiguration();
            builder.InitiativesProjectsTypeConfiguration();
            builder.InteractionStatisticConfiguration();
            builder.InteractionStatisticsTypeConfiguration();
            builder.InvestmentConfiguration();
            builder.InvestmentTypeConfiguration();
            builder.JobRoleConfiguration();
            builder.LoginWayConfiguration();
            builder.MajorLookupConfiguration();
            builder.MenuConfiguration();
            builder.MenuTypeConfiguration();
            builder.MobileApplicationConfiguration();
            builder.MultimediaConfiguration();
            builder.NewsConfiguration();
            builder.OfficialConfiguration();
            builder.PartnerConfiguration();
            builder.PermissionLevelConfiguration();
            builder.QuestionsAnswerConfiguration();
            builder.RateConfiguration();
            builder.ReferenceConfiguration();
            builder.ReferenceContentConfiguration();
            builder.ReferencesJobRoleConfiguration();
            builder.ReferencesMajorConfiguration();
            builder.RoleConfiguration();
            builder.RolesPermissionLevelConfiguration();
            builder.SessionConfiguration();
            builder.TagConfiguration();
            builder.TermsAndRegulationConfiguration();
            builder.UserConfiguration();
            builder.UsersEntityConfiguration();
            builder.UsersPermissionLevelConfiguration();
            builder.VolunteerConfiguration();
            builder.JobAdvertisementConfiguration();
            builder.PermitsRequestConfiguration();
            builder.PermitActionConfiguration();
            builder.PermitWorkSiteConfiguration();
            builder.OpenDataConfiguration();
            builder.OpenDataTempConfiguration();
            builder.OpenDataRequestConfiguration();
            builder.OpenDataStatisticsConfiguration();
            builder.FAQConfiguration();
            builder.StatusConfiguration();
            builder.ActionsConfiguration();
            builder.SurveyConfiguration();
            builder.SurveyQuestionConfiguration();
            builder.SurveyDataSourceConfiguration();
            builder.SurveyAnswerActionConfiguration();
            builder.SurveyQuestionAnswerConfiguration();
            builder.FeedbackConfiguration();
            builder.ScientificLettersConfiguration();
            builder.OrderConfiguration();
            builder.OrderActionsConfiguration();
            builder.ActionFilesConfiguration();
            builder.LogInformationConfiguration();
            builder.ExamConfiguration();
            builder.ExamQuestionConfiguration();
            builder.ExamDataSourceConfiguration();
            builder.ExamAnswerActionConfiguration();
            builder.ExamQuestionAnswerConfiguration();
            builder.JobApplicationExamsConfiguration();
            builder.ProjectConfiguration();
            builder.PublishEntitiesConfiguration();
            builder.FormConfiguration();
            builder.FormInputConfiguration();
            builder.FormValueConfiguration();
            builder.FormInputDataSourceConfiguration();
            builder.FormValueDetailsConfiguration();
            builder.UsersEntityReferenceConfiguration();
            builder.EngineConfiguration();
            builder.WorkFlowActionsConfiguration();
            builder.EngineActionJobRoleConfiguration();
            builder.FormInputsActionsConfiguration();
            builder.FormValuesActionsConfiguration();
            builder.InnovationConfiguration();
            builder.QuestionsRecommendationsConfiguration();
            builder.RecommendationsConfiguration();
            builder.CronSettingsConfiguration();

            builder.FeedbacksConfiguration();
            builder.FeedbacksDataSourceConfiguration();
            builder.FeedbacksAnswerActionConfiguration();
            builder.FeedbacksAnswerConfiguration();


        }

    }
}