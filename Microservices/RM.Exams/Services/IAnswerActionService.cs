using RM.Exams.Dtos;

namespace RM.Exams.Services
{
    public interface IAnswerActionService
    {
        Task<OperationOutput> SaveAnswerAction(ExamAnswerAction RequestedData);
        Task<OperationOutput> GetUserExamAnswers(ExamAnswerAction RequestedData);
    }
}
