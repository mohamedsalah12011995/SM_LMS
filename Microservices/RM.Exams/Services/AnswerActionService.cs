

using Microsoft.EntityFrameworkCore;
using RM.Core.Services;
using RM.Exams.UnitOfWorks;
using RM.Exams.Dtos;
using static RM.Exams.Dtos.OperationOutput;
using RM.Core.Helpers;
using Mapster;

namespace RM.Exams.Services
{
    public class AnswerActionService:BaseService,IAnswerActionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AnswerActionService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            :base(httpContextAccessor,unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> SaveAnswerAction(ExamAnswerAction RequestedData)
        {
            Models.ExamAnswerAction DbItem = new Models.ExamAnswerAction();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var exam = _unitOfWork.Exams.GetById(RequestedData.ExamId.Value);

            if (exam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            DbItem.CreatedBy = RequestOwner.Id;
            DbItem.CreatedDate = TransactionDate;
            DbItem.ExamId=RequestedData.ExamId;
            DbItem.ItemId= RequestedData.ItemId;
            DbItem.Note=RequestedData.Note;
            foreach (var ans in RequestedData.ExamQuestionAnswers)
            {
                var DbAns = new Models.ExamQuestionAnswer();
                DbAns.Text=ans.Text;
                DbAns.Value = ans.Value;
                DbAns.DataSourceId=ans.DataSourceId;    
                DbAns.QuestionId=ans.QuestionId;
                DbAns.ExamAnswerActionId = DbItem.Id;
                DbItem.ExamQuestionAnswers.Add(DbAns);
            }

            _unitOfWork.ExamAnswerActions.Add(DbItem);
            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.Id, DbItem.Id));
        }

        public async Task<OperationOutput> GetUserExamAnswers(ExamAnswerAction RequestedData)
        {
            Dtos.Exam ExamDto = new Dtos.Exam();
            List<Dtos.ExamQuestionAnswer> UserAnswers;

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var examAnswer = await _unitOfWork.ExamAnswerActions.GetAll().Where(x => x.ItemId == RequestedData.ItemId.Value).OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            if (examAnswer == null)
                return GetOperationOutput(header: Enums.ServiceMessages.WrongeData);

            var Exam = await _unitOfWork.Exams.GetAll()
                .Include(s => s.ExamQuestions)
                .ThenInclude(a => a.ExamDataSources)
                .Include(s => s.ExamQuestions)
                .ThenInclude(t => t.QuestionType)
                .Where(x => x.Id == examAnswer.ExamId)
                .AsNoTracking().FirstOrDefaultAsync();

            if (Exam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Exam.Adapt(ExamDto, Dtos.Exam.SelectConfig(null));

            UserAnswers = _unitOfWork.ExamQuestionAnswers.GetAll()
                    .Where(d => d.ExamAnswerActionId == examAnswer.Id)
                    .ToList().Adapt<List<Dtos.ExamQuestionAnswer>>();

            var ExamResult = await _unitOfWork.JobApplicationExams.GetAll()
                .Where(x => x.Id == RequestedData.ItemId)
                .Select(r => new
                {
                    StartAt = r.StartAt != null ? r.StartAt.Value.ToString("yyyy-MM-dd") : null,
                    EndAt = r.EndAt != null ? r.EndAt.Value.ToString("yyyy-MM-dd") : null,
                    Result = r.Result.ToString(),
                    IsSuccess = r.IsSuccess.Value,
                    IsSuccessAr = r.IsSuccess.Value == true ? "ناجح" : "راسب",
                    IsSuccessEn = r.IsSuccess.Value == true ? "Pass" : "Fail"

                }).FirstOrDefaultAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ExamResult, ExamResult),
                   new OutputDictionary(OperationOutputKeys.ExamsEntity, ExamDto),
                   new OutputDictionary(OperationOutputKeys.UserAnswers, UserAnswers));
        }

    }
}
