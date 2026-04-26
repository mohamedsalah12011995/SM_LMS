

using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Jobs.Dtos;
using RM.Jobs.UnitOfWorks;
using static RM.Jobs.Dtos.OperationOutput;


namespace RM.Jobs.Services
{
    public class JobApplicationExamsService : BaseService, IJobApplicationExamsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public JobApplicationExamsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> AddJobApplicationExamsList(JobApplicationExams RequestedData)
        {

            var DbItem = await _unitOfWork.JobApplication.GetAll().Where(x => x.IsDeleted != true
             && RequestedData.JobApplicationList.Select(v => v.Id).Contains(x.Id)).ToListAsync();

            var exam = RequestedData.JobApplicationExamsList.FirstOrDefault();
            foreach (var item in DbItem)
            {
                Models.JobApplicationExams JobApplicationExam = new Models.JobApplicationExams();
                JobApplicationExam.ExamId = exam.ExamId.Value;
                JobApplicationExam.JobApplicationId = item.Id;
                JobApplicationExam.FromDate = exam.FromDate;
                JobApplicationExam.ToDate = exam.ToDate;
                JobApplicationExam.CreatedBy = RequestOwner.Id;
                JobApplicationExam.CreatedDate = TransactionDate;

                _unitOfWork.JobApplicationExams.Add(JobApplicationExam);

                item.UpdatedBy = RequestOwner.Id;
                item.UpdatedDate = DateTime.Now;
                item.CurrentState = RequestedData.JobApplicationList.FirstOrDefault(x => x.Id == item.Id).NextState;
                _unitOfWork.JobApplication.Update(item);
            }
            _unitOfWork.Complete();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> GetJobAppExamInfo(Dtos.JobApplicationExam RequestedData)
        {
            int ExamTimeCounter = 0;
            var JobApplicationExam = _unitOfWork.JobApplicationExams.GetById(RequestedData.AppExamId.Value);

            if (JobApplicationExam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (JobApplicationExam.FromDate > TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateNotStarted);

            if (JobApplicationExam.ToDate < TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateOver);

            if (JobApplicationExam.EndAt.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.ApplyedBefore);

            var Exam = await _unitOfWork.Exams.GetAll()
                  .Include(s => s.ExamQuestions)
                  .ThenInclude(a => a.ExamDataSources)
                  .Include(s => s.ExamQuestions)
                  .ThenInclude(t => t.QuestionType)
                  .Where(x => x.Id == JobApplicationExam.ExamId)
                  .FirstOrDefaultAsync();

            if (Exam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ExamDto = Exam.Adapt<Dtos.Exam>(Dtos.Exam.SelectConfig());

            ExamTimeCounter = Exam.Duration.Value;
            if (JobApplicationExam.StartAt.HasValue)
            {
                if (JobApplicationExam.StartAt.Value.AddMinutes(Exam.Duration.Value) < TransactionDate)
                    return GetOperationOutput(header: Enums.ServiceMessages.ExamTimeFinished);

                var dd = (JobApplicationExam.ToDate.Value - TransactionDate).TotalMinutes;
                ExamTimeCounter = Exam.Duration.Value - (int)(TransactionDate - JobApplicationExam.StartAt.Value).TotalMinutes;
                ExamTimeCounter = (int)dd <= ExamTimeCounter ? (int)dd : ExamTimeCounter;
            }
            else
            {
                JobApplicationExam.StartAt = TransactionDate;
                _unitOfWork.JobApplicationExams.Update(JobApplicationExam);
                _unitOfWork.Complete();
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ExamsEntity, ExamDto),
                   new OutputDictionary(OperationOutputKeys.ExamTimeCounter, ExamTimeCounter));
        }

        public async Task<OperationOutput> GetJobAppExam(Dtos.JobApplicationExam RequestedData)
        {
            int ExamTimeCounter = 0;
            var JobApplicationExam = _unitOfWork.JobApplicationExams.GetById(RequestedData.AppExamId.Value);

            if (JobApplicationExam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (JobApplicationExam.FromDate > TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateNotStarted);

            if (JobApplicationExam.ToDate < TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateOver);

            if (JobApplicationExam.EndAt.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.ApplyedBefore);

            var Exam = await _unitOfWork.Exams.GetAll()
                  .Include(s => s.ExamQuestions)
                  .ThenInclude(a => a.ExamDataSources)
                  .Include(s => s.ExamQuestions)
                  .ThenInclude(t => t.QuestionType)
                  .Where(x => x.Id == JobApplicationExam.ExamId)
                  .FirstOrDefaultAsync();

            if (Exam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ExamDto = Exam.Adapt<Dtos.Exam>(Dtos.Exam.SelectConfig());
            ExamTimeCounter = Exam.Duration.Value;
            if (JobApplicationExam.StartAt.HasValue)
            {
                if (JobApplicationExam.StartAt.Value.AddMinutes(Exam.Duration.Value) < TransactionDate)
                    return GetOperationOutput(header: Enums.ServiceMessages.ExamTimeFinished);

                var dd = (JobApplicationExam.ToDate.Value - TransactionDate).TotalMinutes;
                ExamTimeCounter = Exam.Duration.Value - (int)(TransactionDate - JobApplicationExam.StartAt.Value).TotalMinutes;

                ExamTimeCounter = (int)dd <= ExamTimeCounter ? (int)dd : ExamTimeCounter;
            }
            else
            {
                JobApplicationExam.StartAt = TransactionDate;
                _unitOfWork.JobApplicationExams.Update(JobApplicationExam);
                _unitOfWork.Complete();
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ExamsEntity, ExamDto),
                   new OutputDictionary(OperationOutputKeys.ExamTimeCounter, ExamTimeCounter));
        }
        public async Task<OperationOutput> SaveJobAppExamAnswers(Dtos.ExamAnswerAction RequestedData)
        {
            Models.ExamAnswerAction DbItem = new Models.ExamAnswerAction();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var JobApplicationExam = _unitOfWork.JobApplicationExams.GetById(RequestedData.AppExamId.Value);

            if (JobApplicationExam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (JobApplicationExam.FromDate > TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateNotStarted);

            if (JobApplicationExam.ToDate < TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateOver);

            if (!JobApplicationExam.StartAt.HasValue || JobApplicationExam.EndAt.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowToApplyExam);

            var exam = _unitOfWork.Exams.GetById(JobApplicationExam.ExamId);

            if (exam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (JobApplicationExam.StartAt.HasValue && (JobApplicationExam.StartAt.Value.AddMinutes(exam.Duration.Value + 2) < TransactionDate))
            {
                JobApplicationExam.EndAt = TransactionDate;
                JobApplicationExam.IsSuccess = false;
                _unitOfWork.JobApplicationExams.Update(JobApplicationExam);
                _unitOfWork.Complete();
                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowToApplyExam);
            }

            JobApplicationExam.EndAt = TransactionDate;
            (JobApplicationExam.Result, JobApplicationExam.IsSuccess) = CalculatResult(RequestedData.ExamQuestionAnswers, exam);

            _unitOfWork.JobApplicationExams.Update(JobApplicationExam);

            DbItem.CreatedBy = RequestOwner.Id;
            DbItem.CreatedDate = TransactionDate;
            DbItem.ExamId = JobApplicationExam.ExamId;
            DbItem.ItemId = JobApplicationExam.Id;
            DbItem.Note = RequestedData.Note;
            foreach (var ans in RequestedData.ExamQuestionAnswers)
            {
                var DbAns = new Models.ExamQuestionAnswer();
                DbAns.Text = ans.Text;
                DbAns.Value = ans.Value;
                DbAns.DataSourceId = ans.DataSourceId;
                DbAns.QuestionId = ans.QuestionId;
                DbAns.ExamAnswerActionId = DbItem.Id;
                DbItem.ExamQuestionAnswers.Add(DbAns);
            }

            _unitOfWork.ExamAnswerActions.Add(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public (double, bool) CalculatResult(List<Dtos.ExamQuestionAnswer> QuestionsAnswers, Models.Exam exam)
        {
            double result = 0.0;
            foreach (var ans in QuestionsAnswers)
            {
                if (ans.QuestionId != null && ans.DataSourceId != null)
                {
                    var mark = _unitOfWork.ExamQuestions.GetById(ans.QuestionId.Value).Mark;

                    var isCorrect = _unitOfWork.ExamDataSources.GetById(ans.DataSourceId.Value).IsCorrect;
                    if (mark != null && isCorrect.Value == true)
                        result += mark.Value;
                }
            }
            return (result, result >= exam.SuccessMark);
        }

        public async Task<OperationOutput> UpdateJobApplicationExamsList(Dtos.JobApplicationExams RequestedData)
        {

            var jobApplicationExams = await _unitOfWork.JobApplicationExams.GetAll()
                .Include(c => c.JobApplication)
                .Where(c => !c.StartAt.HasValue)
                .Where(c => RequestedData.JobApplicationList.Any() ?
                    RequestedData.JobApplicationList.Select(v => v.Id).Contains(c.JobApplicationId)
                  : RequestedData.JobCareerId.HasValue ? c.JobApplication.JobCareerId == RequestedData.JobCareerId : true)
                .ToListAsync();

            if (jobApplicationExams.Any())
            {
                foreach (var jobExam in jobApplicationExams)
                {
                    jobExam.FromDate = RequestedData.FromDate.HasValue ? RequestedData.FromDate : jobExam.FromDate;
                    jobExam.ToDate = RequestedData.ToDate.HasValue ? RequestedData.ToDate : jobExam.ToDate;
                    jobExam.UpdatedBy = RequestOwner.Id;
                    jobExam.UpdatedDate = TransactionDate;
                    jobExam.ExamId = RequestedData.ExamId.HasValue ? RequestedData.ExamId.Value : jobExam.ExamId;
                    _unitOfWork.JobApplicationExams.Update(jobExam);
                }
                _unitOfWork.Complete();
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }
    }
}
