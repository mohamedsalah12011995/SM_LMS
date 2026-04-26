using RM.Surveys.Dtos;

namespace RM.Surveys.Services
{
    public interface IAnswerActionService
    {
        public Task<OperationOutput> SaveAnswerAction(SurveyAnswerAction RequestedData);

    }
}
