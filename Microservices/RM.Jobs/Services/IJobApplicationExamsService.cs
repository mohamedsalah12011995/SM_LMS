
using RM.Core.Services;
using RM.Jobs.Dtos;
namespace RM.Jobs.Services
{
    public interface IJobApplicationExamsService:IBaseService
    {
        Task<OperationOutput> AddJobApplicationExamsList(Dtos.JobApplicationExams RequestedData);
        Task<OperationOutput> GetJobAppExamInfo(Dtos.JobApplicationExam RequestedData);
        Task<OperationOutput> GetJobAppExam(Dtos.JobApplicationExam RequestedData);
        Task<OperationOutput> SaveJobAppExamAnswers(Dtos.ExamAnswerAction RequestedData);
        Task<OperationOutput> UpdateJobApplicationExamsList(Dtos.JobApplicationExams RequestedData);

    }
}
