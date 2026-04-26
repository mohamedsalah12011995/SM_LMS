using RM.Core.Services;
using RM.Exams.Dtos;
using RM.Exams.Dtos.ExamStanalone;
using RM.Exams.Dtos.ExamStanalone.PortalDtos;
using RM.Exams.Records.ExamStandalone;

namespace RM.Exams.Services
{
    public interface IExamsService : IBaseService
    {
        #region Exam

        Task<OperationOutput> GetExamLookups(Exam RequestedData);
        Task<OperationOutput> GetExamsList(Dtos.Exam RequestedData);
        Task<OperationOutput> SaveExamAll(Dtos.Exam RequestedData);
        Task<OperationOutput> SaveExam(Dtos.Exam RequestedData);
        Task<OperationOutput> SaveQuestion(Dtos.ExamQuestion RequestedData);
        Task<OperationOutput> GetQuestionDetails(Dtos.ExamQuestion RequestedData);
        Task<OperationOutput> GetExamDetails(Dtos.Exam RequestedData);
        Task<OperationOutput> QuestionModelActions(Dtos.ExamQuestion RequestedData);
        Task<OperationOutput> ModelActions(Dtos.Exam RequestedData);

        #endregion

        #region Exam Standalone 
        Task<OperationOutput> GetLookupsForAssignUsersExam(ExamAssignLookup RequestedData);
        Task<OperationOutput> GetUserDepartmentsForAssignExam(UserDepartmentInput userDepartment);
        Task<OperationOutput> AssignExamToUserList(UserExamInput RequestedData);
        Task<OperationOutput> SendEmailToUsersExam(SendEmailToUsersExam RequestedData);
        Task<OperationOutput> GetDepartmentExamList();
        Task<OperationOutput> QueryUserApplicationExam(QueryUserAppExam RequestedData);
        Task<OperationOutput> GetExamInfoForUserApplication(GetUserExamDto RequestedData);
        Task<OperationOutput> SaveUserApplicationExamAnswers(UserApplicationExamAnswerActions RequestedData);

        Task<OperationOutput> GetDepartmentExamListForResults(ExamListForResultInput RequestedData);
        Task<OperationOutput> GetExamUserApplicationResultReportByYear(ExamUserApplicationResultByYear RequestedData);
        Task<OperationOutput> GetExamResultDetailByExamId(ExamResultDetailByExamIdInput RequestedData);
        Task<OperationOutput> GetUserApplicationExamAnswers(ExamUserApplicationAnswerActionInput RequestedData);
        #endregion

    }
}
