using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Core.Dtos;
using RabbitMQ.Core.Services;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Exams.Dtos;
using RM.Exams.Dtos.Exams.Certifications;
using RM.Exams.Dtos.ExamStanalone;
using RM.Exams.Dtos.ExamStanalone.PortalDtos;
using RM.Exams.Dtos.ExamTrainingCourses.PortalDtos;
using RM.Exams.Dtos.ExamTrainingCourses.TrainingCourseSchedule;
using RM.Exams.Records.Emails;
using RM.Exams.Records.ExamStandalone;
using RM.Exams.UnitOfWorks;
using RM.Models;
using System.Data;
using static RM.Exams.Dtos.OperationOutput;

namespace RM.Exams.Services
{
    public class ExamsService : BaseService, IExamsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private int DepartmentRefernceId = 0;
        public IRabbitMqService _rabbitMqService { get; }
        private readonly RabbitMQConfiguration _configuration;

        public ExamsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
              IOptions<RabbitMQConfiguration> options
            , IRabbitMqService rabbitMqService)
        : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            DepartmentRefernceId = int.Parse(_unitOfWork.Configuration.GetSection("AppSettings").GetSection("Departments").Value!);
            // TransactionDate = Dates.GetCurrentDateTimeInCulture();
            _rabbitMqService = rabbitMqService;
            _configuration = options.Value;
        }

        #region Exam 

        public async Task<OperationOutput> GetExamLookups(Dtos.Exam RequestedData)
        {

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var ExamsList = _unitOfWork.Exams.GetAll().Where(x => x.ReferenceId == RequestedData.ReferenceId
                             && x.EntityId == RequestedData.EntityId && x.IsDeleted != true)

                        .AsNoTracking().ToList().Adapt<List<Dtos.Exam>>();

            var TypesList = _unitOfWork.ExamQuestionTypes.GetAll()
                        .AsNoTracking().ToList().Adapt<List<Dtos.ExamQuestionType>>();

            var certificates = await _unitOfWork.Certificates.GetAll(c => c.IsDeleted == false && c.IsActive == true)
                  .AsNoTracking().Select(c => new ExamLookup
                  {
                      Id = c.Id,
                      TitleAr = c.TitleAr,
                      TitleEn = c.TitleEn,
                  }).ToListAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                        new OutputDictionary(OperationOutputKeys.ExamsList, ExamsList),
                        new OutputDictionary(OperationOutputKeys.TypesList, TypesList),
                        new OutputDictionary(OperationOutputKeys.Certificates, certificates));
        }

        public async Task<OperationOutput> GetExamsList(Dtos.Exam RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var filteration = RequestedData.Filteration();

            var Exams = await _unitOfWork.Exams.FindAllByPaginationAsync(filteration, RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending,
                 c => c.CreatedByNavigation, u => u.UpdatedByNavigation, a => a.ExamAnswerActions);

            var ExamDto = Exams.Data.Adapt<List<Dtos.Exam>>(Dtos.Exam.SelectConfig(null));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ExamsEntity, ExamDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, ExamDto.Any() ? ExamDto[0].entityId : string.Empty),
                   new OutputDictionary(OperationOutputKeys.Pagination, Exams.Pagination));
        }


        public async Task<OperationOutput> SaveExamAll(Dtos.Exam RequestedData)
        {
            Models.Exam Exam = new Models.Exam();
            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (RequestOwner == null)
                        return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

                    if (RequestedData.Id.HasValue)
                    {
                        Exam = _unitOfWork.Exams.GetAll().Where(x => x.Id == RequestedData.Id)
                                                .Include(x => x.ExamQuestions)
                                                .ThenInclude(x => x.ExamDataSources)
                                                .FirstOrDefault();

                        if (Exam == null)
                            return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                        RequestedData.Adapt(Exam, RequestedData.UpdateConfig(RequestOwner.Id.Value, TransactionDate));
                        await UpdateQuestionsAndDataSource(RequestedData, Exam);
                        _unitOfWork.Exams.Update(Exam);

                    }
                    else
                    {
                        RequestedData.Adapt(Exam, RequestedData.AddConfig(RequestOwner.Id.Value, TransactionDate));
                        AddQuestionsAndDataSource(RequestedData, Exam);
                        _unitOfWork.Exams.Add(Exam);
                    }

                    await _unitOfWork.CompleteAsync();
                    dbContextTransaction.Commit();
                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                        new OutputDictionary(OperationOutputKeys.Id, Accessor.Get<int?>(Exam.Id)));
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
                }
            }

        }

        private void AddQuestionsAndDataSource(Dtos.Exam RequestedData, Models.Exam Exam)
        {
            foreach (var Question in RequestedData.ExamQuesions)
            {
                var DbItemQuestion = new Models.ExamQuestion();
                Question.Adapt(DbItemQuestion, Question.AddConfig(RequestOwner.Id.Value, TransactionDate));
                DbItemQuestion.ExamId = RequestedData.Id;

                foreach (var dataSource in Question.ExamDataSources)
                {
                    var DbItemDataSource = new Models.ExamDataSource();
                    dataSource.Adapt(DbItemDataSource, dataSource.AddConfig(RequestOwner.Id.Value, TransactionDate));
                    DbItemDataSource.QuestionId = Question.Id;
                    DbItemQuestion.ExamDataSources.Add(DbItemDataSource);
                }
                Exam.ExamQuestions.Add(DbItemQuestion);

            }
        }

        private async Task UpdateQuestionsAndDataSource(Dtos.Exam RequestedData, Models.Exam Exam)
        {
            var MustRemovedQuestions = Exam.ExamQuestions.Where(x => !RequestedData.ExamQuesions.Select(v => v.Id ?? 0).Contains(x.Id)).ToList();
            var NewQuestions = RequestedData.ExamQuesions.Where(x => !Exam.ExamQuestions.Select(v => v.Id).Contains(x.Id ?? 0)).ToList();
            var MustEditedQuestions = RequestedData.ExamQuesions.Where(x => Exam.ExamQuestions.Select(v => v.Id).Contains(x.Id ?? 0)).ToList();

            if (MustRemovedQuestions.Count > 0)
            {
                await _unitOfWork.ExamQuestions.ExecuteUpdateAsync(x => MustRemovedQuestions.Select(x => x.Id).ToList().Contains(x.Id), sett => sett.SetProperty(x => x.IsDeleted, true).SetProperty(d => d.DeletedBy, RequestOwner.Id).SetProperty(y => y.DeletedDate, TransactionDate));
                await _unitOfWork.ExamDataSources.ExecuteUpdateAsync(x => MustRemovedQuestions.Select(x => x.Id).ToList().Contains(x.QuestionId.Value), sett => sett.SetProperty(x => x.IsDeleted, true).SetProperty(d => d.DeletedBy, RequestOwner.Id).SetProperty(y => y.DeletedDate, TransactionDate));
            }
            if (MustEditedQuestions.Count > 0)
            {
                foreach (var Question in MustEditedQuestions)
                {
                    var DbItemQuestion = Exam.ExamQuestions.Where(x => x.Id == Question.Id).FirstOrDefault();
                    Question.Adapt(DbItemQuestion, Question.UpdateConfig(RequestOwner.Id.Value, TransactionDate));
                    _unitOfWork.ExamQuestions.Update(DbItemQuestion);

                    await UpdateDataSource(Question, DbItemQuestion);
                }
            }

            if (NewQuestions.Count > 0)
            {
                foreach (var Question in NewQuestions)
                {
                    var DbItemQuestion = new Models.ExamQuestion();
                    Question.Adapt(DbItemQuestion, Question.AddConfig(RequestOwner.Id.Value, TransactionDate));
                    DbItemQuestion.ExamId = RequestedData.Id;

                    foreach (var dataSource in Question.ExamDataSources)
                    {
                        var DbItemDataSource = new Models.ExamDataSource();
                        dataSource.Adapt(DbItemDataSource, dataSource.AddConfig(RequestOwner.Id.Value, TransactionDate));
                        DbItemDataSource.QuestionId = DbItemQuestion.Id;
                        DbItemQuestion.ExamDataSources.Add(DbItemDataSource);
                    }
                    _unitOfWork.ExamQuestions.Add(DbItemQuestion);

                }
            }
        }

        private async Task UpdateDataSource(Dtos.ExamQuestion Question, Models.ExamQuestion DbItemQuestion)
        {
            var MustRemovedDataSources = DbItemQuestion.ExamDataSources.Where(x => !Question.ExamDataSources.Select(v => v.Id ?? 0).Contains(x.Id)).ToList();
            var NewDataSources = Question.ExamDataSources.Where(x => !DbItemQuestion.ExamDataSources.Select(v => v.Id).Contains(x.Id ?? 0)).ToList();
            var MustEditedDataSources = Question.ExamDataSources.Where(x => DbItemQuestion.ExamDataSources.Select(v => v.Id).Contains(x.Id ?? 0)).ToList();

            if (MustRemovedDataSources.Count > 0)
            {
                await _unitOfWork.ExamDataSources.ExecuteUpdateAsync(x => MustRemovedDataSources.Select(x => x.Id).ToList().Contains(x.Id), sett => sett.SetProperty(x => x.IsDeleted, true).SetProperty(d => d.DeletedBy, RequestOwner.Id).SetProperty(y => y.DeletedDate, TransactionDate));
            }
            if (MustEditedDataSources.Count > 0)
            {
                foreach (var dataSource in MustEditedDataSources)
                {
                    var DbItemDataSource = DbItemQuestion.ExamDataSources.Where(x => x.Id == dataSource.Id).FirstOrDefault();
                    dataSource.Adapt(DbItemDataSource, dataSource.UpdateConfig(RequestOwner.Id.Value, TransactionDate));
                    _unitOfWork.ExamDataSources.Update(DbItemDataSource);
                }
            }

            if (NewDataSources.Count > 0)
            {
                foreach (var dataSource in NewDataSources)
                {
                    var DbItemDataSource = new Models.ExamDataSource();
                    dataSource.Adapt(DbItemDataSource, dataSource.AddConfig(RequestOwner.Id.Value, TransactionDate));
                    DbItemDataSource.QuestionId = Question.Id;
                    _unitOfWork.ExamDataSources.Add(DbItemDataSource);
                }
            }
        }

        public async Task<OperationOutput> SaveExam(Dtos.Exam RequestedData)
        {
            Models.Exam Exam = new Models.Exam();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.Id.HasValue)
            {
                RequestedData.Adapt(Exam, RequestedData.AddConfig(RequestOwner.Id.Value, TransactionDate));
                _unitOfWork.Exams.Add(Exam);
            }
            else
            {
                Exam = _unitOfWork.Exams.GetAll().Where(x => x.Id == RequestedData.Id).FirstOrDefault();
                if (Exam == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(Exam, RequestedData.UpdateConfig(RequestOwner.Id.Value, TransactionDate));
                _unitOfWork.Exams.Update(Exam);
            }

            _unitOfWork.Complete();
            return await GetExamDetail(Exam.Id);
        }


        public async Task<OperationOutput> SaveQuestion(Dtos.ExamQuestion RequestedData)
        {
            Models.ExamQuestion Quest = null;
            Models.ExamDataSource QuestSource = null;
            List<int> QuestRemovedDataSource = new List<int>();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.Id.HasValue)
            {
                Quest = new Models.ExamQuestion();
                RequestedData.Adapt(Quest, RequestedData.AddConfig(RequestOwner.Id.Value, TransactionDate));
                _unitOfWork.ExamQuestions.Add(Quest);
            }
            else
            {
                Quest = _unitOfWork.ExamQuestions.GetAll().Where(x => x.Id == RequestedData.Id.Value).FirstOrDefault();
                RequestedData.Adapt(Quest, RequestedData.UpdateConfig(RequestOwner.Id.Value, TransactionDate));
                _unitOfWork.ExamQuestions.Update(Quest);

                //datasouce to remove
                QuestRemovedDataSource = _unitOfWork.ExamDataSources.GetAll().Where(x => x.QuestionId == RequestedData.Id).Select(x => x.Id).Where(x => !RequestedData.ExamDataSources.Select(z => z.Id.HasValue ? z.Id.Value : 0).Contains(x)).ToList();
                var QuestRemovedDataSourceNotUsed = _unitOfWork.ExamQuestionAnswers.GetAll().Where(x => x.DataSourceId != null && !QuestRemovedDataSource.Contains(x.DataSourceId.Value)).Select(x => x.DataSourceId).ToList();

                await _unitOfWork.ExamDataSources.ExecuteUpdateAsync(x => QuestRemovedDataSource.Contains(x.Id), sett => sett.SetProperty(c => c.IsDeleted, true).SetProperty(d => d.DeletedBy, RequestOwner.Id).SetProperty(y => y.DeletedDate, TransactionDate));
                await _unitOfWork.ExamDataSources.ExecuteDeleteAsync(x => QuestRemovedDataSourceNotUsed.Contains(x.Id));
            }

            //add or update datasource
            foreach (var datasource in RequestedData.ExamDataSources.Where(x => !QuestRemovedDataSource.Contains(x.Id.Value)).Select(x => x).ToList())
            {
                if (!datasource.Id.HasValue)
                {
                    QuestSource = new Models.ExamDataSource();
                    QuestSource.QuestionId = RequestedData.Id;
                    datasource.Adapt(QuestSource, datasource.AddConfig(RequestOwner.Id.Value, TransactionDate));
                    _unitOfWork.ExamDataSources.Add(QuestSource);
                }
                else
                {
                    QuestSource = _unitOfWork.ExamDataSources.GetAll().Where(x => x.Id == datasource.Id.Value).FirstOrDefault();
                    datasource.Adapt(QuestSource, datasource.UpdateConfig(RequestOwner.Id.Value, TransactionDate));
                    _unitOfWork.ExamDataSources.Update(QuestSource);
                }
            }

            _unitOfWork.Complete();
            return await GetQuestionDetail(Quest.Id);

        }

        public async Task<OperationOutput> GetExamDetails(Dtos.Exam RequestedData)
        {
            return await GetExamDetail(RequestedData.Id.Value, RequestedData.IsActive);
        }

        public async Task<OperationOutput> GetExamDetail(int Id, bool? IsActive = null)
        {
            Dtos.Exam ItemDto = new Dtos.Exam();
            var Item = await _unitOfWork.Exams.GetAll()
                 .Include(s => s.ExamQuestions)
                 .ThenInclude(c => c.CreatedByNavigation)
                 .Include(s => s.ExamQuestions)
                 .ThenInclude(c => c.UpdatedByNavigation)
                 .Include(s => s.ExamQuestions)
                 .ThenInclude(a => a.ExamDataSources)
                 .Include(s => s.ExamQuestions)
                 .ThenInclude(t => t.QuestionType)
                 .AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted != true);

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Item.Adapt(ItemDto, Dtos.Exam.SelectConfig(IsActive));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.ExamsEntity, ItemDto),
                    new OutputDictionary(OperationOutputKeys.EntityID, ItemDto.entityId));

        }

        public async Task<OperationOutput> GetQuestionDetails(Dtos.ExamQuestion RequestedData)
        {
            return await GetQuestionDetail(RequestedData.Id.Value, RequestedData.IsActive);
        }

        public async Task<OperationOutput> GetQuestionDetail(int Id, bool? IsActive = null)
        {
            Dtos.ExamQuestion ItemDto = new Dtos.ExamQuestion();
            var Item = await _unitOfWork.ExamQuestions.GetAll()
                .Include(s => s.ExamDataSources)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted != true);

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Item.Adapt(ItemDto, Dtos.ExamQuestion.SelectConfig(IsActive));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.ExamQuesionEntity, ItemDto));
        }

        public async Task<OperationOutput> QuestionModelActions(Dtos.ExamQuestion RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = await _unitOfWork.ExamQuestions.GetByIdAsync(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            DbItem.IsActive = RequestedData.IsActive.HasValue ? RequestedData.IsActive.Value : DbItem.IsActive;
            DbItem.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : DbItem.IsDeleted;
            if (RequestedData.IsDeleted.HasValue && RequestedData.IsDeleted.Value == true)
            {
                DbItem.DeletedBy = RequestOwner.Id;
                DbItem.DeletedDate = TransactionDate;
            }

            DbItem.UpdatedBy = RequestOwner.Id;
            DbItem.UpdatedDate = TransactionDate;

            _unitOfWork.ExamQuestions.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> ModelActions(Dtos.Exam RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = await _unitOfWork.Exams.GetByIdAsync(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            DbItem.IsActive = RequestedData.IsActive.HasValue ? RequestedData.IsActive.Value : DbItem.IsActive;
            DbItem.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : DbItem.IsDeleted;

            if (RequestedData.IsDeleted.HasValue && RequestedData.IsDeleted.Value == true)
            {
                DbItem.DeletedBy = RequestOwner.Id;
                DbItem.DeletedDate = TransactionDate;
            }

            DbItem.UpdatedBy = RequestOwner.Id;
            DbItem.UpdatedDate = TransactionDate;

            _unitOfWork.Exams.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        #endregion

        #region Exam Stanalone

        public async Task<OperationOutput> GetLookupsForAssignUsersExam(ExamAssignLookup RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var examsList = await GetExamListWithValidDates(RequestedData);

            List<ReferencesTree> departmentReferencesTree = await GetDepartmentReferences();

            var certificateThemes = await GetCertificationThemeLookup();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.ExamsList, examsList),
                     new OutputDictionary(OperationOutputKeys.CertificationThemes, certificateThemes),
                     new OutputDictionary(OperationOutputKeys.DepartmentReferences, departmentReferencesTree));
        }

        #region HELPER METHODS GetLookupsForAssignUsersExam
        private async Task<List<ReferencesTree>> GetDepartmentReferences()
        {
            var departmentReferences = await _unitOfWork.References
                .GetAll(x => x.IsDeleted != true && x.ParentId != null).AsNoTracking().ToListAsync();

            var departmentReferencesTree = GenerateReferenceTree(departmentReferences, DepartmentRefernceId).Adapt<List<ReferencesTree>>(ReferencesTree.SelectConfig());
            return departmentReferencesTree;
        }

        private IEnumerable<ReferencesTree> GenerateReferenceTree(List<Models.Reference> references, int? parentId)
        {
            foreach (var r in references.Where(r => r.ParentId == parentId))
            {
                var t = r.Adapt<ReferencesTree>(ReferencesTree.SelectConfig());
                t.Children = GenerateReferenceTree(references, r.Id);
                yield return t;
            }
        }

        private async Task<List<ExamWithUserApplicationValidDateList>> GetExamListWithValidDates(ExamAssignLookup requestedData)
        {
            var examList = await (from exam in _unitOfWork.Exams
                                    .GetAll(c => c.ReferenceId == requestedData.ReferenceId
                                         && c.EntityId == requestedData.EntityId && c.IsDeleted != true)

                                  join certificate in _unitOfWork.Certificates.GetAll()
                                  on exam.CertificateId equals certificate.Id into examCertification
                                  from exmCert in examCertification.DefaultIfEmpty()

                                  join examUser in _unitOfWork.UserApplicationExam
                                  .GetAll(c => c.ToDate >= TransactionDate)

                                  on exam.Id equals examUser.ExamId into userExams
                                  from userExm in userExams.DefaultIfEmpty()
                                  group new { exam, userExm, exmCert } by new { exam.Id } into grpUserExm
                                  select new ExamWithUserApplicationValidDateList
                                  {
                                      Id = grpUserExm.Key.Id,
                                      TitleAr = grpUserExm.Select(g => g.exam.TitleAr).First(),
                                      TitleEn = grpUserExm.Select(g => g.exam.TitleEn).First(),
                                      CertificateTitleAr = grpUserExm.Select(g => g.exmCert != null ? g.exmCert.TitleAr : string.Empty).First(),
                                      CertificateTitleEn = grpUserExm.Select(g => g.exmCert != null ? g.exmCert.TitleEn : string.Empty).First(),
                                      DateList = grpUserExm.Where(g => g.userExm != null)
                                              .Select(g => new UserApplicationValidDates
                                              {
                                                  Id = g.userExm.Id,
                                                  FromDate = g.userExm.FromDate.Value,
                                                  ToDate = g.userExm.ToDate.Value,
                                                  FromDateString = g.userExm.FromDate.Value.ToString("yyyy-MM-dd hh:mm"),
                                                  ToDateString = g.userExm.ToDate.Value.ToString("yyyy-MM-dd hh:mm"),

                                              }).OrderBy(c => c.FromDate).GroupBy(d => new { d.FromDate, d.ToDate }).Select(d => d.First())
                                             .ToList()
                                  }).ToListAsync();
            return examList;
        }

        private async Task<List<ThemesLookup>> GetCertificationThemeLookup()
        {
            return await _unitOfWork.CertificateThemes.GetAll(c => c.IsActive)
                  .AsNoTracking().Select(c => new ThemesLookup
                  {
                      Id = c.Id,
                      TitleAr = c.TitleAr,
                      TitleEn = c.TitleEn,
                      ThumbnailPicture = $"{ImagesGetPath}/{c.ThumbnailPicture}"
                  }).ToListAsync();
        }

        #endregion

        public async Task<OperationOutput> GetUserDepartmentsForAssignExam(UserDepartmentInput RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var userDepartments = await GetUserDepartments(RequestedData);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.UserDepartments, userDepartments));
        }


        #region HELPER METHOD GetUserDepartmentsForAssignExam
        private async Task<List<UserDepartmentsOutput>> GetUserDepartments(UserDepartmentInput userDepartment)
        {
            var userDepartments = await (from user in _unitOfWork.User.GetAll(c => !c.IsDeleted && !c.IsBlocked)
                                         join reference in _unitOfWork.References.GetAll() on user.ReferenceId equals reference.Id
                                         join examUser in _unitOfWork.UserApplicationExam
                                         .GetAll(c => c.IsDeleted != true
                                         && c.FromDate == userDepartment.FromDate && c.ToDate == userDepartment.ToDate)

                                         on user.Id equals examUser.UserId into userExams

                                         where userDepartment.ReferenceId.HasValue ?
                                         user.ReferenceId == userDepartment.ReferenceId :
                                         reference.ParentId == DepartmentRefernceId

                                         from userExm in userExams.DefaultIfEmpty()
                                         select new UserDepartmentsOutput
                                         {
                                             UserId = user.Id,
                                             Name = user.Name,
                                             DepartmentReferenceNameAr = reference.NameAr,
                                             DepartmentReferenceNameEn = reference.NameEn,
                                             Phone = user.Phone,
                                             IdCardNumber = user.IdCardNumber,
                                             EmployeeID = user.EmployeeId,
                                             Email = user.Email,
                                             IsSelected = userExm != null && userExm.ExamId == userDepartment.ExamId && userExm.FromDate == userDepartment.FromDate && userExm.ToDate == userDepartment.ToDate
                                         }).ToListAsync();


            return userDepartments;

        }

        #endregion


        public async Task<OperationOutput> AssignExamToUserList(UserExamInput RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.ExamId.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var examData = await _unitOfWork.Exams.GetByIdAsync(RequestedData.ExamId.Value);

            if (examData != null && examData.CertificateId.HasValue && !RequestedData.CertificateThemeId.HasValue)
                await SetDefaultCertificateTheme(RequestedData);

            var userApplicationExam = await _unitOfWork.UserApplicationExam
                .GetAll(c => c.ExamId == RequestedData.ExamId
                && c.FromDate == RequestedData.FromDate
                && c.ToDate == RequestedData.ToDate
                && c.StartAt == null && c.EndAt == null && c.IsDeleted != true)
               .ToListAsync();

            var removedUsersExams = userApplicationExam.Where(x => !RequestedData.Users.Select(v => v.Id ?? 0).Contains(x.UserId)).ToList();

            if (removedUsersExams.Count > 0)
                RemoveUserApplicationExams(removedUsersExams);

            var newExamUsers = RequestedData.Users.Where(x => !userApplicationExam.Select(v => v.UserId).Contains(x.Id ?? 0)).ToList();
            var editedExamUsers = RequestedData.Users.Where(x => userApplicationExam.Select(v => v.UserId).Contains(x.Id ?? 0)).ToList();

            if (newExamUsers.Count > 0)
                await InsertUseApplivationExams(RequestedData, newExamUsers);

            if (editedExamUsers.Count > 0)
                await UpdateUserApplicationExams(RequestedData, userApplicationExam, editedExamUsers);

            await _unitOfWork.CompleteAsync();

            var examsList = await GetExamListWithValidDates(new ExamAssignLookup
            {
                EntityId = RequestedData.EntityId,
                ReferenceId = RequestedData.ReferenceId
            });
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.ExamsList, examsList));
        }

        #region HELPER METHOD AssignExamToUserList
        private async Task SetDefaultCertificateTheme(UserExamInput RequestedData)
        {
            var theme = await _unitOfWork.CertificateThemes.GetAll().AsNoTracking().FirstOrDefaultAsync();
            if (theme != null)
                RequestedData.CertificateThemeId = theme.Id;
        }

        private void RemoveUserApplicationExams(List<Models.UserApplicationExam> removedUsersExams)
        {
            foreach (var user in removedUsersExams)
            {
                _unitOfWork.UserApplicationExam.Delete(user);
            }
        }

        private async Task UpdateUserApplicationExams(UserExamInput RequestedData, List<Models.UserApplicationExam> userApplicationExam, List<UserData> editedExamUsers)
        {
            var updatedExamList = new List<Models.UserApplicationExam>();

            foreach (var item in editedExamUsers)
            {
                var updateditem = userApplicationExam.FirstOrDefault(c => c.UserId == item.Id);
                if (updateditem is not null)
                {
                    updateditem.FromDate = RequestedData.FromDate;
                    updateditem.ToDate = RequestedData.ToDate;
                    updatedExamList.Add(updateditem);
                }
            }

            await _unitOfWork.UserApplicationExam.BulkUpdateAsync(updatedExamList);

        }

        private async Task InsertUseApplivationExams(UserExamInput RequestedData, List<UserData> newExamUsers)
        {
            var internalExams = new List<Models.UserApplicationExam>();

            foreach (var item in newExamUsers)
            {
                Models.UserApplicationExam internalExam = new();
                RequestedData.Adapt(internalExam, RequestedData.AddConfig(RequestOwner.Id.Value, item.Id.Value, TransactionDate));
                internalExams.Add(internalExam);
            }

            await _unitOfWork.UserApplicationExam.BulkInsertAsync(internalExams);
        }

        #endregion

        public async Task<OperationOutput> SendEmailToUsersExam(SendEmailToUsersExam RequestedData)
        {

            var usersExamAssignedCount = await _unitOfWork.UserApplicationExam.GetAll()
                .AsNoTracking()
                .Where(c => c.ExamId == RequestedData.ExamId
                && RequestedData.Users.Select(u => u.Id).Contains(c.UserId)
                && c.IsSuccess == null && c.FromDate == DateTime.Parse(RequestedData.FromDate)
                && c.ToDate == DateTime.Parse(RequestedData.ToDate)).CountAsync();

            if (usersExamAssignedCount != RequestedData.Users.Count)
                return GetOperationOutput(header: Enums.ServiceMessages.MustAssignAllSelectedUsersToExam);

            var emailAddress = await _unitOfWork.User.GetAll(c => RequestedData.Users.Select(u => u.Id).Contains(c.Id))
                .AsNoTracking().Select(c => c.Email.Trim()).ToListAsync();


            var exam = await _unitOfWork.Exams.GetAll().AsNoTracking().Select(c => new { c.Id, c.TitleAr })
                .FirstOrDefaultAsync(c => c.Id == RequestedData.ExamId.Value);

            if (emailAddress.Count > 0 && exam is not null)
            {
                return await SendEmailToUsersForExam(exam.TitleAr, RequestedData.FromDate, RequestedData.ToDate, emailAddress);
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

        }

        #region HELPER METHOD SendEmailToUsersExam
        private async Task<OperationOutput> SendEmailToUsersForExam(string examName, string fromDate, string toDate, List<string> emailAddress)
        {
            string Body = string.Empty;
            string Subject = $" {TransactionDate.ToString("yyy-MM-dd hh:mm ")}  -  ادارة التدريب والتطوير ";


            string path = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
            using (StreamReader reader = new StreamReader(Path.Combine(path, "ExamStandaloneUserEmail.html")))
            {
                Body = reader.ReadToEnd();
            }
            Body = Body.Replace("@currentDateHijri", Dates.ConvertFromGerogianToHijriDateString(TransactionDate));
            Body = Body.Replace("@currentDate", TransactionDate.ToString("yyyy-MM-dd"));
            Body = Body.Replace("@examName", examName);
            Body = Body.Replace("@fromDate", fromDate);
            Body = Body.Replace("@toDate", toDate);

            #region RabbitMQ

            DateTime endDateExam;
            var isValid = DateTime.TryParse(toDate, out endDateExam);

            if (isValid && endDateExam > DateTime.Now)
            {
                var result = SendEmailMessageByRabbitMQ(toDate, emailAddress, Body, Subject);

                if (result.IsValid)
                {
                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
                }
                else return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

            }

            return GetOperationOutput(header: Enums.ServiceMessages.ExamDateEnded);

            #endregion

            // await Email.SendEmailAsync(emailAddress.First(), Subject, Body, EmailServiceUrl, Token, null, null, emailAddress);
        }


        #region HELPER METHOD SendEmailToUsersExam
        private ResultDto SendEmailMessageByRabbitMQ(string toDate, List<string> emailAddress, string Body, string Subject)
        {
            var emailInfo = new EmailInfoRecord
            {
                ToEmail = emailAddress.First(),
                Subject = Subject,
                Body = Body,
                BCCEmailAddresses = emailAddress
            };


            (_, var model) = _rabbitMqService.CreateConnection();
            _rabbitMqService.DeclareQueue_Quorum(model, _configuration.Queue, _configuration.Exchange, string.Empty, ExchangeType.Fanout);

            var expirationTime = DateTime.Parse(toDate).Subtract(DateTime.Now);

            return _rabbitMqService.SendMessage(model, _configuration.Exchange, emailInfo, string.Empty, expirationTime);
        }

        #endregion

        #endregion

        #region CP Exam Results - Report

        public async Task<OperationOutput> GetDepartmentExamListForResults(ExamListForResultInput RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var examsList = await _unitOfWork.Exams.GetAll().Where(x => x.ReferenceId == RequestedData.ReferenceId
                            && x.EntityId == RequestedData.EntityId && x.IsDeleted != true)
                           .AsNoTracking().ToListAsync();

            var examsListDto = examsList.Adapt<List<Exams.Dtos.ExamStanalone.PortalDtos.ExamListLookup>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ExamsList, examsListDto));
        }


        public async Task<OperationOutput> GetExamUserApplicationResultReportByYear(ExamUserApplicationResultByYear RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var userApplicationExams = await _unitOfWork.UserApplicationExam
                 .GetAll(c => c.IsDeleted != true && c.FromDate.Value.Year >= RequestedData.Year)
                 .Where(c => RequestedData.ExamId.HasValue ? c.ExamId == RequestedData.ExamId : true)

                 .AsNoTrackingWithIdentityResolution()
                 .Include(c => c.Exam)
                 .GroupBy(tc => tc.ExamId)
                 .Select(g => new
                 {
                     ExamId = Accessor.Get(g.Key),
                     ExamTitleAr = g.Select(c => c.Exam.TitleAr).First(),
                     ExamTitleEn = g.Select(c => c.Exam.TitleEn).First(),
                     TotalUserExam = g.Count(),
                     TotalUserExamPass = g.Count(tc => tc.IsSuccess == true),
                     SuccessRate = g.Count() > 0 && g.Count(tc => tc.IsSuccess == true) > 0 ? Math.Round((g.Count(tc => tc.IsSuccess == true) * 100.0) / g.Count(), 2) : 0.0
                 }).ToListAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ExamResult, userApplicationExams));
        }

        public async Task<OperationOutput> GetExamResultDetailByExamId(ExamResultDetailByExamIdInput RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var userApplicationExams = await _unitOfWork.UserApplicationExam.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Ascending, c => c.Exam, c => c.User, r => r.User.Reference);

            var userApplicationExamsDto = userApplicationExams.Data.ToList().Adapt<List<UserExamsDetailsOutput>>(UserExamsDetailsOutput.SelectConfig());


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ExamResult, userApplicationExamsDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, userApplicationExams.Pagination));
        }

        public async Task<OperationOutput> GetUserApplicationExamAnswers(ExamUserApplicationAnswerActionInput RequestedData)
        {
            Dtos.Exam ExamDto = new Dtos.Exam();
            List<Dtos.ExamQuestionAnswer> UserAnswers;

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var examAnswer = await _unitOfWork.ExamUserApplicationAnswerAction.GetAll().Where(x => x.UserApplicationExamId == RequestedData.UserApplicationExamId.Value).OrderByDescending(x => x.Id).FirstOrDefaultAsync();

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

            UserAnswers = _unitOfWork.ExamUserApplicationQuestionAnswer.GetAll()
                    .Where(d => d.ExamUserApplicationAnswerActionId == examAnswer.Id)
                    .ToList().Adapt<List<Dtos.ExamQuestionAnswer>>();

            var ExamResult = await _unitOfWork.UserApplicationExam
                .GetAll(x => x.Id == RequestedData.UserApplicationExamId)
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

        #endregion

        #region PORTAL API

        public async Task<OperationOutput> GetDepartmentExamList()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var examList = await _unitOfWork.UserApplicationExam
                 .GetAll(c => c.ToDate >= TransactionDate && c.IsDeleted != true)
                .AsNoTrackingWithIdentityResolution()
                .Include(c => c.Exam)
                .GroupBy(c => new { c.ExamId, c.FromDate, c.ToDate })
                .Select(g => new DepartmentExamListOutput
                {
                    ExamId = g.Key.ExamId,
                    TitleAr = g.Select(c => c.Exam.TitleAr).First(),
                    TitleEn = g.Select(c => c.Exam.TitleEn).First(),
                    DescriptionAr = g.Select(c => c.Exam.DescriptionAr).First(),
                    DescriptionEn = g.Select(c => c.Exam.DescriptionEn).First(),
                    FromDate = g.Key.FromDate.Value,
                    ToDate = g.Key.ToDate.Value,
                    FromDateString = g.Key.FromDate.Value.ToString("yyyy-MM-dd"),
                    ToDateString = g.Key.ToDate.Value.ToString("yyyy-MM-dd"),
                    RemainTime = g.Key.ToDate.Value.Subtract(TransactionDate).TotalMinutes.ToString()

                }).ToListAsync();


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                             new OutputDictionary(OperationOutputKeys.ExamsList, examList));

        }

        public async Task<OperationOutput> QueryUserApplicationExam(QueryUserAppExam RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (string.IsNullOrEmpty(RequestedData.IdCardNumber))
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var userApplication = await _unitOfWork.UserApplicationExam.GetAll()
                .AsNoTrackingWithIdentityResolution()
                .Include(c => c.Exam)
                .Include(c => c.User)
                .ThenInclude(c => c.Reference)
                .FirstOrDefaultAsync(c => c.ExamId == RequestedData.ExamId
                 && c.User.IdCardNumber.Trim() == RequestedData.IdCardNumber.Trim()
                 && c.FromDate == RequestedData.FromDate
                 && c.ToDate == RequestedData.ToDate
                 && c.IsDeleted != true);


            if (userApplication is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var userApplicationDto = userApplication.Adapt<QueryUserApplicationOutputDto>(QueryUserApplicationOutputDto.SelectConfig());

            if (userApplication.IsSuccess == null && userApplication.FromDate.Value <= TransactionDate
                && userApplication.ToDate.Value > TransactionDate)
            {
                FillUserApplicationDtoByExamData(userApplicationDto, userApplication);
            }
            else
            {
                FillUserApplicationExamNotAvailable(userApplicationDto, userApplication);
            }

            if (userApplicationDto.IsSuccess == true && userApplication.Exam.CertificateId.HasValue)
            {
                await GetCertificationDataAndSaveLogToUserExam(userApplication, userApplicationDto);

            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                            new OutputDictionary(OperationOutputKeys.Traniee, userApplicationDto));
        }

        #region HELPER METHOD QueryUserApplicationExam

        private void FillUserApplicationDtoByExamData(QueryUserApplicationOutputDto userApplicationDto, Models.UserApplicationExam userExam)
        {
            userApplicationDto.ExamDateStringAr = $"موعد الاختبار : {userExam.FromDate.Value.ToString("yyyy-MM-dd hh:mm")}: {userExam.ToDate.Value.ToString("yyyy-MM-dd hh:mm")}";
            userApplicationDto.ExamDateStringEn = $"Exam date : {userExam.FromDate.Value.ToString("yyyy-MM-dd hh:mm")}: {userExam.ToDate.Value.ToString("yyyy-MM-dd hh:mm")}";
            userApplicationDto.IsExamAvailable = true;
            userApplicationDto.ExamResultStringAr = userExam.IsSuccess == true ? "ناجح" : userExam.IsSuccess == false ? "راسب" : "لم يتم بدء موعد الاختبار ";
            userApplicationDto.ExamResultStringEn = userExam.IsSuccess == true ? "Passed" : userExam.IsSuccess == false ? "Failed" : "The exam date has not started.";
            userApplicationDto.ExamTitleAr = userExam.Exam != null ? userExam.Exam.TitleAr : string.Empty;
            userApplicationDto.ExamTitleEn = userExam.Exam != null ? userExam.Exam.TitleEn : string.Empty;
            userApplicationDto.IsSuccess = userExam.IsSuccess;

        }

        private void FillUserApplicationExamNotAvailable(QueryUserApplicationOutputDto userApplicationDto, Models.UserApplicationExam userExam)
        {
            if (userExam.FromDate > TransactionDate)
            {
                userApplicationDto.ExamDateStringAr = $"موعد الاختبار : {userExam.FromDate.Value.ToString("yyyy-MM-dd hh:mm")}: {userExam.ToDate.Value.ToString("yyyy-MM-dd hh:mm")}";
                userApplicationDto.ExamDateStringEn = $"Exam date : {userExam.FromDate.Value.ToString("yyyy-MM-dd hh:mm")}: {userExam.ToDate.Value.ToString("yyyy-MM-dd hh:mm")}";
                userApplicationDto.IsExamAvailable = false;
            }
            else if (userExam.ToDate > TransactionDate)
            {
                userApplicationDto.ExamDateStringAr = "موعد الاختبار: انتهى";
                userApplicationDto.ExamDateStringEn = "Exam date: Ended";
                userApplicationDto.IsExamAvailable = false;
            }
            userApplicationDto.ExamResultStringAr = userExam.IsSuccess == true ? "ناجح" : userExam.IsSuccess == false ? "راسب" : "لم يتم بدء موعد الاختبار ";
            userApplicationDto.ExamResultStringEn = userExam.IsSuccess == true ? "Passed" : userExam.IsSuccess == false ? "Failed" : "The exam date has not started.";
            userApplicationDto.ExamTitleAr = userExam.Exam != null ? userExam.Exam.TitleAr : string.Empty;
            userApplicationDto.ExamTitleEn = userExam.Exam != null ? userExam.Exam.TitleEn : string.Empty;
            userApplicationDto.IsSuccess = userExam.IsSuccess;
        }

        private async Task GetCertificationDataAndSaveLogToUserExam(Models.UserApplicationExam userApplication, QueryUserApplicationOutputDto userApplicationDto)
        {
            CertificateData certificate = new();

            var certificateModel = await _unitOfWork.Certificates.GetAll()
                .AsNoTracking().FirstOrDefaultAsync(c => c.Id == userApplication.Exam.CertificateId);

            certificate = certificateModel.Adapt<CertificateData>();
            certificate.CertificateId = certificateModel.Id;

            if (certificateModel.UpdatedDate > userApplication.EndAt)
            {
                var certificateLog = await _unitOfWork.CertificateLog.GetAll().AsNoTracking()
                      .FirstOrDefaultAsync(c => c.UserApplicationExamId == userApplication.Id);

                certificate = certificateLog.Adapt<CertificateData>();
            }

            var certificateTheme = await _unitOfWork.CertificateThemes.GetAll()
               .AsNoTracking().Select(c => new { c.Id, c.Name })
               .FirstOrDefaultAsync(c => c.Id == userApplication.CertificateThemeId);

            if (certificate != null)
                userApplicationDto.TraineeCertificate = certificate.Adapt<TraineeCertificate>(TraineeCertificate.SelectConfig(ImagesGetPath, userApplication.User.Name, certificateTheme != null ? certificateTheme.Name : string.Empty, userApplication.EndAt, userApplication.CertificateNumber));

            if (string.IsNullOrEmpty(userApplication.CertificateNumber) && userApplicationDto.TraineeCertificate != null)
            {
                userApplication.CertificateNumber = userApplicationDto.TraineeCertificate.CertificateNumber;

                CertificateLog certificateLog = certificate.Adapt<CertificateLog>();

                certificateLog.UserApplicationExamId = userApplication.Id;
                certificateLog.CreatedDate = TransactionDate;

                _unitOfWork.CertificateLog.Add(certificateLog);

                await _unitOfWork.CompleteAsync();
            }
        }

        #endregion


        //like as GetJobAppExam

        public async Task<OperationOutput> GetExamInfoForUserApplication(GetUserExamDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.UserApplicationExamId.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            return await GetExamInfoForUserApplicationExam(RequestedData);

        }

        #region HELPER METHODS GetExamInfoForUserApplication

        private async Task<OperationOutput> GetExamInfoForUserApplicationExam(GetUserExamDto RequestedData)
        {
            int examTimeCounter = 0;
            var userApplicationExam = _unitOfWork.UserApplicationExam.GetById(RequestedData.UserApplicationExamId.Value);

            if (userApplicationExam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (userApplicationExam.FromDate > TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateNotStarted);

            if (userApplicationExam.ToDate < TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateOver);

            if (userApplicationExam.EndAt.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.ApplyedBefore);

            var exam = await _unitOfWork.Exams.GetAll()
                  .Include(s => s.ExamQuestions)
                  .ThenInclude(a => a.ExamDataSources)
                  .Include(s => s.ExamQuestions)
                  .ThenInclude(t => t.QuestionType)
                  .Where(x => x.Id == userApplicationExam.ExamId)
                  .FirstOrDefaultAsync();

            if (exam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var examDto = exam.Adapt<Dtos.Exam>(Dtos.Exam.SelectConfig(true));

            examTimeCounter = exam.Duration.Value;

            if (userApplicationExam.StartAt.HasValue)
            {
                if (userApplicationExam.StartAt.Value.AddMinutes(exam.Duration.Value) < TransactionDate)
                    return GetOperationOutput(header: Enums.ServiceMessages.ExamTimeFinished);

                var totalMinutes = (userApplicationExam.ToDate.Value - TransactionDate).TotalMinutes;
                examTimeCounter = exam.Duration.Value - (int)(TransactionDate - userApplicationExam.StartAt.Value).TotalMinutes;

                examTimeCounter = (int)totalMinutes <= examTimeCounter ? (int)totalMinutes : examTimeCounter;
            }
            else
            {
                userApplicationExam.StartAt = TransactionDate;
                _unitOfWork.UserApplicationExam.Update(userApplicationExam);
                _unitOfWork.Complete();
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                              new OutputDictionary(OperationOutputKeys.ExamsEntity, examDto),
                              new OutputDictionary(OperationOutputKeys.ExamTimeCounter, examTimeCounter));
        }


        #endregion

        //like as SaveJobAppExamAnswers

        public async Task<OperationOutput> SaveUserApplicationExamAnswers(UserApplicationExamAnswerActions RequestedData)
        {

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            return await SaveUserApplicationExamAnswer(RequestedData);
        }


        #region HELPER METHODS SaveUserApplicationExamAnswers
        private async Task<OperationOutput> SaveUserApplicationExamAnswer(UserApplicationExamAnswerActions RequestedData)
        {
            Models.ExamUserApplicationAnswerAction examAnswerAction = new();

            var userApplicationExam = await _unitOfWork.UserApplicationExam.GetByIdAsync(RequestedData.Id.Value);

            if (userApplicationExam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (userApplicationExam.FromDate > TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateNotStarted);

            if (userApplicationExam.ToDate < TransactionDate)
                return GetOperationOutput(header: Enums.ServiceMessages.ExamDateOver);

            if (!userApplicationExam.StartAt.HasValue || userApplicationExam.EndAt.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowToApplyExam);

            var exam = _unitOfWork.Exams.GetById(userApplicationExam.ExamId);

            if (exam == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (userApplicationExam.StartAt.HasValue && (userApplicationExam.StartAt.Value.AddMinutes(exam.Duration.Value + 2) < TransactionDate))
            {
                userApplicationExam.EndAt = TransactionDate;
                userApplicationExam.IsSuccess = false;
                userApplicationExam.AnswerTotalTime = Math.Round(userApplicationExam.EndAt.Value.Subtract(userApplicationExam.StartAt.Value).TotalMinutes, 2);

                _unitOfWork.UserApplicationExam.Update(userApplicationExam);
                _unitOfWork.Complete();
                return GetOperationOutput(header: Enums.ServiceMessages.NotAllowToApplyExam);
            }

            userApplicationExam.EndAt = TransactionDate;
            (userApplicationExam.Result, userApplicationExam.IsSuccess) = CalculatResult(RequestedData.ExamQuestionAnswers, exam);

            userApplicationExam.AnswerTotalTime = Math.Round(userApplicationExam.EndAt.Value.Subtract(userApplicationExam.StartAt.Value).TotalMinutes, 2);

            _unitOfWork.UserApplicationExam.Update(userApplicationExam);

            examAnswerAction.CreatedBy = RequestOwner.Id;
            examAnswerAction.CreatedDate = TransactionDate;
            examAnswerAction.ExamId = userApplicationExam.ExamId;
            examAnswerAction.UserApplicationExamId = userApplicationExam.Id;
            examAnswerAction.Note = RequestedData.Note;
            foreach (var answer in RequestedData.ExamQuestionAnswers)
            {
                var dbAns = new Models.ExamUserApplicationQuestionAnswer();
                dbAns.Text = answer.Text;
                dbAns.Value = answer.Value;
                dbAns.DataSourceId = answer.DataSourceId;
                dbAns.QuestionId = answer.QuestionId;
                dbAns.ExamUserApplicationAnswerActionId = examAnswerAction.Id;
                examAnswerAction.ExamQuestionAnswer.Add(dbAns);
            }

            _unitOfWork.ExamUserApplicationAnswerAction.Add(examAnswerAction);

            await _unitOfWork.CompleteAsync();

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


        #endregion



        #endregion

        #endregion


    }
}
