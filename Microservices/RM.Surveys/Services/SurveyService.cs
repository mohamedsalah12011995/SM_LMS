using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using RM.Core.CommonDtos;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Services;
using RM.Surveys.Dtos;
using RM.Surveys.Services;
using RM.Surveys.UnitOfWorks;
using System.Linq.Expressions;
using static RM.Surveys.Dtos.OperationOutput;


namespace Surveys.Services.Services
{
    public class SurveyService : BaseService, ISurveyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private static string imagesGetPath = string.Empty;
        public static string RequestLanguage = string.Empty;
        private static string HtmlSurveyReportUrl = string.Empty;
        public string FormSurveyReviewUrl;

        private readonly ILogger<SurveyService> _logger;
        public SurveyService(IUnitOfWork unitOfWork, ILogger<SurveyService> logger, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            imagesGetPath = Strings.HandleGetResourcesPath(IsLocal, GetPath, IntranetGetPath);
            FormSurveyReviewUrl = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("FormSurveyReviewUrl").Value;
            _logger = logger;
            //  RequestLanguage = _httpContextAccessor.HttpContext.Request.Headers["lang"].FirstOrDefault();
            _logger.LogError("Survey Service constractor RequestOwner is : {RequestOwner}", RequestOwner);

            try { HtmlSurveyReportUrl = _configuration.GetSection("AppSettings").GetSection("HtmlSurveyReportUrl").Value; }
            catch (Exception ex)
            {
                _logger.LogError("Survey Service constractor InnerException is : {Message}{InnerException}", ex.Message, ex.InnerException);

            }
        }



        #region SURVEY

        public async Task<OperationOutput> GetSurveyQuestionLookups(Survey RequestedData)
        {

            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var surveys = await _unitOfWork.Surveys.FindAllWithAsNoTracking(s => s.ReferenceId == RequestedData.ReferenceId && s.IsDeleted != true)
                                           .Select(s => new RM.Models.Survey { Id = s.Id, TitleAr = s.TitleAr, TitleEn = s.TitleEn }).ToListAsync();

            var surveysDto = surveys.Adapt<List<Survey>>(Survey.SelectConfig(ImagesGetPath));

            var surveyQuestionTypes = await _unitOfWork.SurveyQuestionTypes.FindAllWithAsNoTracking().Select(s => new RM.Models.SurveyQuestionType { Id = s.Id, TextAr = s.TextAr, TextEn = s.TextEn, HasDataSource = s.HasDataSource, Icon = s.Icon }).ToListAsync();
            var surveyQuestionTypesDto = surveyQuestionTypes.Adapt<List<SurveyQuestionType>>();

            var surveyThemes = await _unitOfWork.SurveyThemes.GetAll(c => c.IsActive == true).AsNoTracking().ToListAsync();
            var surveyThemesDto = surveyThemes.Adapt<List<SurveyTheme>>();

            var surveyRecommendions = await _unitOfWork.SurveyRecommendations.GetAll().Where(x => x.EntityId == (int)Enums.Entities.Survey).AsNoTracking().ToListAsync();
            var surveyRecommendionsDto = surveyRecommendions.Adapt<List<SurveyRecommendations>>(SurveyRecommendations.SelectConfig());

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.SurveysList, surveysDto),
               new OutputDictionary(OperationOutputKeys.TypesList, surveyQuestionTypesDto),
               new OutputDictionary(OperationOutputKeys.SurveyThemes, surveyThemesDto),
               new OutputDictionary(OperationOutputKeys.SurveyRecommendions, surveyRecommendionsDto));
        }

        public async Task<OperationOutput> GetReferences()
        {
            var references = await _unitOfWork.References.FindAllWithAsNoTracking(c => c.IsPortal == true && c.ParentId == null)
                             .Select(c => new RM.Models.Reference { Id = c.Id, NameAr = c.NameAr, NameEn = c.NameEn }).ToListAsync();

            var referencesDto = references.Adapt<List<Reference>>(Reference.SelectConfig());

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.References, referencesDto));

        }

        public async Task<OperationOutput> GetSurveysList(Survey RequestedData)
        {

            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var publishedSurveys = _unitOfWork.PublishEntities.FindAll(c => c.EntityId == (int)Enums.Entities.Survey && c.ReferenceId == RequestedData.ReferenceId).ToList();

            var filteration = RequestedData.Filteration(publishedSurveys.Select(x => x.ItemId).ToList());

            var thenByList = new Expression<Func<RM.Models.Survey, object>>[]
            {    x=> x.FromDate ?? DateTime.MaxValue,
                 x => x.CreatedDate ?? x.CreatedDate
            };

            var surveys = await _unitOfWork.Surveys.FindAllByPaginationWithThenBy(filteration, RequestedData.Pagination, DefaultPaginationCount,
                                                  x => x.ToDate ?? DateTime.MaxValue, OrderBy.Descending,
                                                  thenByList, ThenBy.Descending,
                                                  c => c.CreatedByNavigation, u => u.UpdatedByNavigation,
                                                  a => a.SurveyAnswerActions);

            var surveysDto = surveys.Data.Adapt<List<Survey>>(Survey.SelectConfig(ImagesGetPath));

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.SurveysEntity, surveysDto),
               new OutputDictionary(OperationOutputKeys.EntityID, surveysDto.Count > 0 ? Accessor.Get<int?>(surveysDto[0].EntityId) : string.Empty),
               new OutputDictionary(OperationOutputKeys.Pagination, surveys.Pagination));

        }

        public async Task<OperationOutput> SaveSurveyAll(Survey RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            RM.Models.Survey Sur = null;
            RM.Models.SurveyQuestion Quest = null;
            RM.Models.SurveyDataSource QuestSource = null;
            List<int> QuestRemoved = new List<int>();
            List<int> Quests = new List<int>();
            List<int> QuestSourceRemoved = new List<int>();
            bool SurIsNew = true;

            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (RequestOwner is null)
                    {
                        Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoTokenRequested);
                        return Result;
                    }

                    //add or update Survey
                    if (!RequestedData.Id.HasValue)
                    {
                        Sur = new RM.Models.Survey();
                        Sur.ReferenceId = RequestedData.ReferenceId;
                        Sur.EntityId = RequestedData.EntityId;
                        Sur.CreatedBy = RequestOwner.Id.Value;
                        Sur.CreatedDate = TransactionDate;
                        Sur.Code = Strings.RandomDigits(10).ToString();
                        Sur.IsDeleted = false;
                    }
                    else
                    {
                        SurIsNew = false;
                        Sur = _unitOfWork.Surveys.GetAll().Include(c => c.SurveyQuestions).Include(s => s.CronSettings).Where(x => x.Id == RequestedData.Id).FirstOrDefault();
                        if (Sur == null)
                        {
                            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoDataReturned);
                            return Result;
                        }
                        Sur.UpdatedBy = RequestOwner.Id.Value;
                        Sur.UpdatedDate = TransactionDate;
                    }

                    Sur.TitleAr = RequestedData.TitleAr;
                    Sur.TitleEn = RequestedData.TitleEn;
                    Sur.DescriptionAr = RequestedData.DescriptionAr;
                    Sur.DescriptionEn = RequestedData.DescriptionEn;
                    Sur.ShowInHomePage = RequestedData.ShowInHomePage;
                    Sur.UseCapcha = RequestedData.UseCapcha;
                    Sur.InnerOnly = RequestedData.InnerOnly;
                    Sur.FromDate = RequestedData.FromDate;
                    Sur.ToDate = RequestedData.ToDate;
                    Sur.UpdatedBy = RequestOwner.Id.Value;
                    Sur.UpdatedDate = TransactionDate;
                    Sur.ThemeId = RequestedData.ThemeId;
                    if (!string.IsNullOrEmpty(RequestedData.ImageBase64))
                        Sur.Image = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.ImageBase64)
                            ? Images.SaveSingleImageOnServer(RequestedData.ImageBase64, null, ImagesSavePath, false) : null;



                    if (!SurIsNew)
                    {
                        // update all groups if not found in case survey is updated to isdeleted =true
                        DeletSurveyGroups(RequestedData, Sur);

                        foreach (var group in RequestedData.GroupQuestion)
                        {
                            //quest to remove
                            QuestRemoved = _unitOfWork.SurveyQuestions.GetAll()
                                .Where(x => x.SurveyId == RequestedData.Id && x.GroupId == group.GroupId).Select(x => x.Id)
                                .Where(x => !group.SurveyQuestion.Select(z => z.Id.HasValue ? z.Id.Value : 0).Contains(x)).ToList();
                            if (QuestRemoved.Any())
                            {
                                DeleteQuestionsInGroup(QuestRemoved);
                            }

                            //datasource to remove in all questions
                            foreach (var q in group.GroupDataSource)
                            {
                                //datasouce to remove
                                QuestSourceRemoved = _unitOfWork.SurveyDataSources.GetAll().Where(x => x.GroupId == group.GroupId && x.IsDeleted != true).Select(x => x.Id).Where(x => !group.GroupDataSource.Select(z => z.Id.HasValue ? z.Id.Value : 0).Contains(x)).ToList();

                                if (QuestSourceRemoved.Any())
                                {
                                    DeleteDataSourceInGroup(QuestSourceRemoved);
                                }
                            }

                        }
                    }

                    if (RequestedData.GroupQuestion != null)
                    {
                        //add or update quest
                        AddOrUpdateSurveyQutionsAndDataSource(RequestedData, Sur, ref Quest, ref QuestSource);
                    }

                    if (SurIsNew) _unitOfWork.Surveys.Add(Sur);
                    else _unitOfWork.Surveys.Update(Sur);

                    await _unitOfWork.CompleteAsync();

                    await AddOrUpdateCronSettingsAsync(RequestedData, Sur, SurIsNew);

                    dbContextTransaction.Commit();


                    #region Save Publish Entities
                    if (!SurIsNew)
                    {
                        var deleteReferences = _unitOfWork.PublishEntities.FindAll(c => c.ItemId == Sur.Id).ToList();
                        if (deleteReferences.Any()) { _unitOfWork.PublishEntities.DeleteRange(deleteReferences); }
                    }

                    if (RequestedData.PublishEntity.Any())
                    {
                        foreach (var item in RequestedData.PublishEntity)
                        {
                            var publish = new RM.Models.PublishEntities();
                            publish.EntityId = (int)Enums.Entities.Survey;
                            publish.ReferenceId = item._id.Value;
                            publish.ItemId = Sur.Id;
                            publish.CreatedBy = Sur.UpdatedBy;
                            publish.CreatedDate = TransactionDate;
                            _unitOfWork.PublishEntities.Add(publish);

                        }
                        _unitOfWork.Complete();

                    }

                    #endregion

                    RequestedData.Id = Sur.Id;
                    Result = await GetSurveyDetails(RequestedData);
                    return Result;

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionErorr);
                }

                return Result;
            }


        }

        private async Task AddOrUpdateCronSettingsAsync(Survey RequestedData, RM.Models.Survey Sur, bool SurIsNew)
        {
            foreach (var sett in RequestedData.CronSettings)
            {

                if (!sett.Id.HasValue)
                {
                    var NewItem = sett.Adapt(new RM.Models.CronSettings(), sett.AddConfig(Sur.Id));
                    await _unitOfWork.CronSettings.AddAsync(NewItem);
                }
                else
                {
                    var Item = sett.Adapt(Sur.CronSettings.Where(x => x.Id == sett.Id).FirstOrDefault(), sett.UpdateConfig());
                    await _unitOfWork.CronSettings.UpdateAsync(Sur.CronSettings.Where(x => x.Id == sett.Id).FirstOrDefault(), Item);
                }

            }
            await _unitOfWork.CompleteAsync();

        }


        #region HELPER METHODS TO SAVEALL
        private void AddOrUpdateSurveyQutionsAndDataSource(Survey RequestedData, RM.Models.Survey Sur, ref RM.Models.SurveyQuestion Quest, ref RM.Models.SurveyDataSource QuestSource)
        {
            foreach (var group in RequestedData.GroupQuestion)
            {
                foreach (var quest in group.SurveyQuestion)
                {
                    bool QuestIsNew = true;
                    if (!quest.Id.HasValue)
                    {
                        Quest = new RM.Models.SurveyQuestion();
                        Quest.SurveyId = Sur.Id;
                        Quest.CreatedBy = RequestOwner.Id;
                        Quest.CreatedDate = TransactionDate;

                        if (quest.QuestionsRecommendations != null && !quest.QuestionsRecommendations.IsNull())
                            Quest.QuestionsRecommendations = quest.QuestionsRecommendations.Adapt<RM.Models.QuestionsRecommendations>();
                    }
                    else
                    {
                        QuestIsNew = false;
                        Quest = _unitOfWork.SurveyQuestions.GetAll().Where(x => x.Id == quest.Id.Value).FirstOrDefault();
                        if (quest.QuestionsRecommendations != null && !quest.QuestionsRecommendations.IsNull())
                        {
                            var QRecomend = _unitOfWork.QuestionsRecommendations.Find(x => x.QuestionId == quest.Id);
                            if (QRecomend != null)
                            {
                                quest.QuestionsRecommendations.Adapt(QRecomend, quest.QuestionsRecommendations.UpdateConfig());
                                _unitOfWork.QuestionsRecommendations.Update(QRecomend);
                            }
                            else
                            {
                                QRecomend = new RM.Models.QuestionsRecommendations();
                                quest.QuestionsRecommendations.Adapt(QRecomend, quest.QuestionsRecommendations.AddConfig(Quest.Id));
                                _unitOfWork.QuestionsRecommendations.Add(QRecomend);
                            }
                        }
                    }
                    Quest.TextAr = quest.TextAr;
                    Quest.TextEn = quest.TextEn;
                    Quest.DescriptionAr = quest.DescriptionAr;
                    Quest.DescriptionEn = quest.DescriptionEn;
                    Quest.TypeId = group._typeId;
                    Quest.Mandatory = quest.Mandatory;
                    Quest.VerticalAnswersDirection = quest.VerticalAnswersDirection;
                    Quest.IsFiltration = quest.IsFiltration;
                    Quest.IsActive = quest.IsActive;

                    Quest.GroupId = group.GroupId;
                    Quest.UpdatedBy = RequestOwner.Id;
                    Quest.UpdatedDate = TransactionDate;
                    Quest.IsGlobal = quest.IsGlobal;
                    Quest.GroupOrder = quest.GroupOrder;
                    Quest.SubQuestionOrder = quest.SubQuestionOrder;

                    Quest.MinValue = quest.MinValue;
                    Quest.MaxValue = quest.MaxValue;
                    Quest.IsNoteRequired = quest.IsNoteRequired;
                    if (!string.IsNullOrEmpty(quest.ImageBase64))
                        Quest.Image = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(quest.ImageBase64)
                            ? Images.SaveSingleImageOnServer(quest.ImageBase64, null, ImagesSavePath, false) : null;


                    if (quest.IsDeleted == true)
                        Quest.IsDeleted = true;

                    if (QuestIsNew)
                        Sur.SurveyQuestions.Add(Quest);
                    else
                    {
                        _unitOfWork.SurveyQuestions.Update(Quest);
                    }

                    _unitOfWork.Complete();

                }


                var type = _unitOfWork.SurveyQuestionTypes.Find(x => x.Id == group._typeId.Value);

                if (type.HasDataSource == true)
                {
                    foreach (var datasource in group.GroupDataSource)
                    {

                        bool DataSourceIsNew = true;
                        if (!datasource.Id.HasValue)
                        {
                            QuestSource = new RM.Models.SurveyDataSource();
                            QuestSource.QuestionId = null;
                            QuestSource.CreatedBy = RequestOwner.Id;
                            QuestSource.CreatedDate = TransactionDate;
                        }
                        else
                        {
                            DataSourceIsNew = false;
                            QuestSource = _unitOfWork.SurveyDataSources.GetAll().Where(x => x.Id == datasource.Id.Value).FirstOrDefault();
                        }
                        QuestSource.TextAr = datasource.TextAr;
                        QuestSource.TextEn = datasource.TextEn;
                        QuestSource.DescriptionAr = datasource.DescriptionAr;
                        QuestSource.DescriptionEn = datasource.DescriptionEn;

                        QuestSource.IsActive = datasource.IsActive;
                        QuestSource.IsDeleted = datasource.IsDeleted;
                        QuestSource.GroupId = group.GroupId;
                        QuestSource.UpdatedBy = RequestOwner.Id;
                        QuestSource.UpdatedDate = TransactionDate;
                        QuestSource.Rate = datasource.Rate;
                        if (!string.IsNullOrEmpty(datasource.ImageBase64))
                            QuestSource.Image = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(datasource.ImageBase64)
                                ? Images.SaveSingleImageOnServer(datasource.ImageBase64, null, ImagesSavePath, false) : null;

                        if (DataSourceIsNew) _unitOfWork.SurveyDataSources.Add(QuestSource);
                        else
                        {
                            _unitOfWork.SurveyDataSources.Update(QuestSource);
                        }

                    }
                }
            }
        }

        private void DeleteDataSourceInGroup(List<int> QuestSourceRemoved)
        {
            foreach (var dsId in QuestSourceRemoved)
            {
                var souceToDelete = _unitOfWork.SurveyDataSources.GetAll().Where(x => x.Id == dsId).FirstOrDefault();
                if (souceToDelete != null)
                {
                    var dNotUsed = _unitOfWork.SurveyQuestionAnswers.GetAll().FirstOrDefault(x => x.DataSourceId == dsId) == null;
                    if (dNotUsed)
                    {
                        _unitOfWork.SurveyDataSources.Delete(souceToDelete);
                    }
                    else
                    {
                        souceToDelete.IsDeleted = true;
                        _unitOfWork.SurveyDataSources.Update(souceToDelete);
                    }
                }
            }
        }

        private void DeleteQuestionsInGroup(List<int> QuestRemoved)
        {
            foreach (var qId in QuestRemoved)
            {
                var qusToDelete = _unitOfWork.SurveyQuestions.GetAll().Where(x => x.Id == qId).FirstOrDefault();

                //datasouce to remove in removed question

                var qNotUsed = _unitOfWork.SurveyQuestionAnswers.GetAll().FirstOrDefault(x => x.QuestionId == qId) == null;
                if (qNotUsed)
                {
                    _unitOfWork.SurveyQuestions.Delete(qusToDelete);
                }
                else
                {
                    qusToDelete.IsDeleted = true;
                    _unitOfWork.SurveyQuestions.Update(qusToDelete);
                }
            }
        }

        private void DeletSurveyGroups(Survey RequestedData, RM.Models.Survey Sur)
        {
            var surveyGroups = Sur.SurveyQuestions.Where(c => c.IsDeleted != true).GroupBy(c => c.GroupId).Select(c => c.Key).ToList();
            var editGroups = RequestedData.GroupQuestion.GroupBy(c => c.GroupId).Select(c => c.Key).ToList();
            var removedGroups = surveyGroups.Except(editGroups).ToList();
            if (removedGroups.Any())
            {
                foreach (var group in removedGroups)
                {
                    var allDatasourceForGroupId = _unitOfWork.SurveyDataSources.FindAll(c => c.GroupId == group)
                        .Select(c => { c.IsDeleted = true; c.UpdatedBy = RequestOwner.Id.Value; c.UpdatedDate = TransactionDate; return c; }).ToList();

                    var allQuestionForGroupId = _unitOfWork.SurveyQuestions.FindAll(c => c.GroupId == group && c.SurveyId == Sur.Id)
                        .Select(c => { c.IsDeleted = true; c.UpdatedBy = RequestOwner.Id.Value; c.UpdatedDate = TransactionDate; return c; }).ToList();
                }

            }
        }

        #endregion 

        public async Task<OperationOutput> SaveSurvey(Survey RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            OperationOutput Result = new OperationOutput();
            RM.Models.Survey survey = RequestedData.ToEntity();
            if (RequestedData.Id.HasValue)
            {
                var model = _unitOfWork.Surveys.GetAll().Where(x => x.Id == RequestedData.Id).FirstOrDefault();
                if (model == null)
                    return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);
                survey.CreatedBy = model.CreatedBy;
                survey.CreatedDate = model.CreatedDate;
                survey.DeletedBy = model.DeletedBy;
                survey.DeletedDate = model.DeletedDate;
            }
            survey.UpdatedBy = RequestOwner.Id.Value;
            survey.ThemeId = RequestedData.ThemeId;
            if (!string.IsNullOrEmpty(RequestedData.ImageBase64))
                survey.Image = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.ImageBase64)
                    ? Images.SaveSingleImageOnServer(RequestedData.ImageBase64, null, ImagesSavePath, false) : null;


            if (!RequestedData.Id.HasValue)
            {
                survey.CreatedBy = RequestOwner.Id.Value;
                _unitOfWork.Surveys.Add(survey);
            }
            else _unitOfWork.Surveys.Update(survey);
            _unitOfWork.Complete();

            RequestedData.Id = survey.Id;
            Result = await GetSurveyDetails(RequestedData);
            return Result;

        }

        public async Task<OperationOutput> SaveQuestion(SurveyQuestion RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            RM.Models.SurveyQuestion Quest = null;
            RM.Models.SurveyDataSource QuestSource = null;
            List<int> QuestSourceRemoved = new List<int>();
            bool QuestIsNew = true;
            if (RequestOwner is null)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoTokenRequested);
                return Result;
            }


            if (!RequestedData.Id.HasValue)
            {
                Quest = new RM.Models.SurveyQuestion();
                Quest.SurveyId = RequestedData.SurveyId;
                Quest.CreatedBy = RequestOwner.Id;
                Quest.CreatedDate = TransactionDate;
            }
            else
            {
                QuestIsNew = false;
                Quest = await _unitOfWork.SurveyQuestions.GetAll().Where(x => x.Id == RequestedData.Id.Value).FirstOrDefaultAsync();
                Quest.UpdatedBy = RequestOwner.Id;
                Quest.UpdatedDate = TransactionDate;

                // datasouce to remove
                // from the front pass the question object with the data source has the same groupid
                QuestSourceRemoved = _unitOfWork.SurveyDataSources.GetAll().Where(x => x.GroupId == RequestedData.GroupId).Select(x => x.Id).Where(x => !RequestedData.SurveyDataSources.Select(z => z.Id.HasValue ? z.Id.Value : 0).Contains(x)).ToList();

                foreach (var qsId in QuestSourceRemoved)
                {
                    var souceToDelete = _unitOfWork.SurveyDataSources.GetAll().Where(x => x.Id == qsId).FirstOrDefault();
                    if (souceToDelete != null)
                    {
                        var dNotUsed = _unitOfWork.SurveyQuestionAnswers.Find(x => x.DataSourceId == qsId) == null;
                        if (dNotUsed)
                        {
                            _unitOfWork.SurveyDataSources.Delete(souceToDelete);
                        }
                        else
                        {
                            souceToDelete.IsDeleted = true;
                            _unitOfWork.SurveyDataSources.Update(souceToDelete);
                        }
                    }
                }
            }
            Quest.TextAr = RequestedData.TextAr;
            Quest.TextEn = RequestedData.TextEn;
            Quest.DescriptionAr = RequestedData.DescriptionAr;
            Quest.DescriptionEn = RequestedData.DescriptionEn;
            Quest.TypeId = RequestedData.TypeId;
            Quest.Mandatory = RequestedData.Mandatory;
            Quest.GroupId = RequestedData.GroupId;
            Quest.VerticalAnswersDirection = RequestedData.VerticalAnswersDirection;
            Quest.IsFiltration = RequestedData.IsFiltration;
            Quest.IsGlobal = RequestedData.IsGlobal;
            Quest.MinValue = RequestedData.MinValue;
            Quest.MaxValue = RequestedData.MaxValue;
            Quest.IsNoteRequired = RequestedData.IsNoteRequired;
            if (!string.IsNullOrEmpty(RequestedData.ImageBase64))
                Quest.Image = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.ImageBase64)
                    ? Images.SaveSingleImageOnServer(RequestedData.ImageBase64, null, ImagesSavePath, false) : null;

            //add or update datasource
            foreach (var datasource in RequestedData.SurveyDataSources)
            {
                bool DataSourceIsNew = true;
                if (!datasource.Id.HasValue)
                {
                    QuestSource = new RM.Models.SurveyDataSource();
                    QuestSource.QuestionId = Quest.Id;
                    QuestSource.CreatedBy = RequestOwner.Id;
                    QuestSource.CreatedDate = TransactionDate;
                    QuestSource.IsActive = true;
                }
                else
                {
                    DataSourceIsNew = false;
                    QuestSource = _unitOfWork.SurveyDataSources.GetAll().Where(x => x.Id == datasource.Id.Value).FirstOrDefault();
                    QuestSource.UpdatedBy = RequestOwner.Id;
                    QuestSource.UpdatedDate = TransactionDate;
                }
                QuestSource.TextAr = datasource.TextAr;
                QuestSource.TextEn = datasource.TextEn;
                QuestSource.DescriptionAr = datasource.DescriptionAr;
                QuestSource.DescriptionEn = datasource.DescriptionEn;
                QuestSource.IsActive = datasource.IsActive.HasValue ? datasource.IsActive : QuestSource.IsActive;
                QuestSource.GroupId = RequestedData.GroupId;
                QuestSource.Rate = datasource.Rate;
                if (!string.IsNullOrEmpty(datasource.ImageBase64))
                    QuestSource.Image = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(datasource.ImageBase64)
                        ? Images.SaveSingleImageOnServer(datasource.ImageBase64, null, ImagesSavePath, false) : null;


                if (DataSourceIsNew) Quest.SurveyDataSources.Add(QuestSource);
                else _unitOfWork.SurveyDataSources.Update(QuestSource);

            }

            if (QuestIsNew) _unitOfWork.SurveyQuestions.Add(Quest);
            else _unitOfWork.SurveyQuestions.Update(Quest);
            await _unitOfWork.CompleteAsync();

            RequestedData.Id = Quest.Id;
            RequestedData.IsActive = Quest.IsActive;
            return await GetQuestionDetails(RequestedData);

        }

        public async Task<OperationOutput> GetSurveyDetails(Survey RequestedData)
        {
            Survey surveyDto = null;
            List<ReferenceShareUrl> referenceShareUrls = new List<ReferenceShareUrl>();
            var publishReferences = _unitOfWork.PublishEntities.FindAll(x => x.ItemId == RequestedData.Id, c => c.Reference).ToList();
            List<RM.Models.SurveyDataSource> surveyDataSource = new List<RM.Models.SurveyDataSource>();

            var survey = await _unitOfWork.Surveys.GetAll()
                .Include(c => c.Theme)
                .Include(c => c.Reference)
                .Include(s => s.SurveyAnswerActions)
                .Include(s => s.CronSettings)
                .Include(s => s.SurveyQuestions.Where(c => c.IsDeleted != true))
                .ThenInclude(t => t.QuestionType)
                .Include(s => s.SurveyQuestions.Where(c => c.IsDeleted != true))
                .ThenInclude(t => t.QuestionsRecommendations)
                .Where(x => RequestedData.IsActive.HasValue ? x.IsActive == true : true).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == RequestedData.Id && x.IsDeleted != true);

            if (survey != null)
            {

                if (survey.ReferenceId != RequestedData.ReferenceId && publishReferences.FirstOrDefault(c => c.ReferenceId == RequestedData.ReferenceId.Value) == null)
                    return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.AccessDenied);

                GetSurveyReferenceShareUrl(referenceShareUrls, publishReferences, survey);

                if (survey.SurveyQuestions.Any())
                {
                    foreach (var question in survey.SurveyQuestions)
                    {
                        if (!surveyDataSource.Any(c => c.GroupId == question.GroupId))
                        {
                            var datasource = _unitOfWork.SurveyDataSources.FindAll(c => c.IsDeleted != true && c.GroupId.Trim() == question.GroupId.Trim()).ToList();
                            if (datasource.Any())
                                surveyDataSource.AddRange(datasource);
                        }
                    }
                }

                surveyDto = survey.Adapt<Survey>(Survey.SelectConfig(ImagesGetPath));
                surveyDto.ReferenceShareUrls = referenceShareUrls;
                surveyDto.PublishEntity = publishReferences.Any() ? publishReferences.Select(x => new Reference { _id = x.ReferenceId, }).ToList() : null;

                var surveyGroupIds = survey.SurveyQuestions.Where(x => x.IsDeleted != true).OrderBy(c => c.GroupOrder != null ? c.GroupOrder : c.Id).GroupBy(c => c.GroupId).Select(c => c.Key).ToList();

                foreach (var group in surveyGroupIds)
                {
                    GroupQuestion groupQuestion = FillGroupQuestion(RequestedData, surveyDataSource, survey, group, ImagesGetPath);

                    surveyDto.GroupQuestion.Add(groupQuestion);
                }

                foreach (var cron in surveyDto.CronSettings)
                {
                    var Users = _unitOfWork.Users.GetAll().Where(x => cron.Emails.Contains(x.Email)).ToList();
                    cron.Users = Users.Adapt<List<Users>>();
                }
            }

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.SurveysEntity, surveyDto),
             new OutputDictionary(OperationOutputKeys.EntityID, surveyDto != null ? Accessor.Get<int?>(surveyDto.EntityId) : string.Empty));
        }


        #region HELPER METHODS TO GetSurveyDetails
        private static GroupQuestion FillGroupQuestion(Survey RequestedData, List<RM.Models.SurveyDataSource> surveyDataSource, RM.Models.Survey survey, string group, string ImagesGetPath)
        {
            var groupQuestion = survey.SurveyQuestions.Where(x => x.IsDeleted != true && x.GroupId == group)
                .OrderBy(c => c.SubQuestionOrder != null ? c.SubQuestionOrder : (c.GroupOrder != null ? c.GroupOrder : c.Id))
                .ToList();
            var groupDatasource = surveyDataSource.Where(c => c.GroupId == group).OrderBy(c => c.Id).ToList();

            return new GroupQuestion
            {
                GroupId = group,
                _typeId = survey.SurveyQuestions.FirstOrDefault(x => x.IsDeleted != true && x.GroupId == group).TypeId,
                SurveyQuestion = groupQuestion.Adapt<List<SurveyQuestion>>(SurveyQuestion.SelectConfig(ImagesGetPath)),
                GroupDataSource = groupDatasource.Adapt<List<SurveyDataSource>>(SurveyDataSource.SelectConfig(ImagesGetPath))

            };
        }

        private void GetSurveyReferenceShareUrl(List<ReferenceShareUrl> referenceShareUrls, List<RM.Models.PublishEntities> publishReferences, RM.Models.Survey survey)
        {
            var userReference = new ReferenceShareUrl
            {
                _referenceId = survey.Reference != null ? survey.Reference.Id : null,
                NameAr = survey.Reference != null ? survey.Reference.NameAr : string.Empty,
                NameEn = survey.Reference != null ? survey.Reference.NameEn : string.Empty,
                ReviewUrlTitleAr = "رابط الاستعراض",
                ReviewUrlTitleEn = "Review Url",
                ShareUrl = survey.Reference != null ? $"{survey.Reference.Url}{RequestLanguage}/{FormSurveyUrl}/{Accessor.Get<int?>(survey.Id)}" : null,
                ShareReviewUrl = survey.Reference != null ? $"{survey.Reference.Url}{RequestLanguage}/{FormSurveyReviewUrl}/{Accessor.Get<int?>(survey.Id)}" : null

            };
            referenceShareUrls.Add(userReference);

            if (publishReferences.Any())
            {
                foreach (var p in publishReferences)
                {

                    var publishRefUrl = new ReferenceShareUrl
                    {
                        _referenceId = p.ReferenceId,
                        NameAr = p.Reference != null ? p.Reference.NameAr : string.Empty,
                        NameEn = p.Reference != null ? p.Reference.NameEn : string.Empty,
                        ReviewUrlTitleAr = "رابط الاستعراض",
                        ReviewUrlTitleEn = "Review Url",
                        ShareUrl = p.Reference != null ? $"{p.Reference.Url}/{FormSurveyUrl}/{Accessor.Get<int?>(survey.Id)}" : null,
                        ShareReviewUrl = p.Reference != null ? $"{p.Reference.Url}/{FormSurveyReviewUrl}/{Accessor.Get<int?>(survey.Id)}" : null
                    };
                    referenceShareUrls.Add(publishRefUrl);
                }
            }
        }

        #endregion

        public async Task<OperationOutput> GetSurveyQuestions(Survey RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<GroupQuestion> GroupQuestion = new List<GroupQuestion>();

            var surveyQuestions = await _unitOfWork.SurveyQuestions
                .GetAll().Include(c => c.QuestionType)
                .Where(c => c.SurveyId == RequestedData.Id && c.IsDeleted != true).AsNoTracking().ToListAsync();

            if (surveyQuestions.Count > 0)
            {
                var surveyGroupIds = surveyQuestions.OrderBy(c => c.GroupOrder != null ? c.GroupOrder : c.Id).GroupBy(c => c.GroupId).Select(c => c.Key).ToList();

                foreach (var group in surveyGroupIds)
                {
                    var firstQues = surveyQuestions.FirstOrDefault(x => x.GroupId == group);
                    GroupQuestion groupQuestion = FillGroupQuestionForSurveyDetail(surveyQuestions, group, firstQues, ImagesGetPath);
                    GroupQuestion.Add(groupQuestion);
                }
            }

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.GroupQuestion, GroupQuestion));
        }

        #region HELPER METHOD TO GetSurveyQuestions
        private static GroupQuestion FillGroupQuestionForSurveyDetail(List<RM.Models.SurveyQuestion> surveyQuestions, string group, RM.Models.SurveyQuestion firstQues, string ImagesGetPath)
        {
            var groupSurveyQuestions = surveyQuestions.Where(x => x.GroupId == group).OrderBy(c => c.SubQuestionOrder != null ? c.SubQuestionOrder : (c.GroupOrder != null ? c.GroupOrder : c.Id)).ToList();

            return new GroupQuestion
            {
                GroupId = group,
                _typeId = firstQues.TypeId,
                TextAr = firstQues.TextAr,
                TextEn = firstQues.TextEn,
                CreatedDateString = firstQues.CreatedDate != null ? firstQues.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty,
                UpdatedDateString = firstQues.UpdatedDate != null ? firstQues.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty,
                SurveyQuestion = groupSurveyQuestions.Adapt<List<SurveyQuestion>>(SurveyQuestion.SelectConfig(ImagesGetPath)),

            };
        }

        #endregion


        public async Task<OperationOutput> SurveyQuestionOrder(Survey RequestedData)
        {
            List<RM.Models.SurveyQuestion> DbItem = new List<RM.Models.SurveyQuestion>();

            var surveyQuestions = _unitOfWork.SurveyQuestions.FindAll(c => c.SurveyId == RequestedData.Id && c.IsDeleted != true).ToList();
            if (surveyQuestions.Any() && RequestedData.GroupQuestion.Any())
            {
                foreach (var group in RequestedData.GroupQuestion)
                {
                    foreach (var question in group.SurveyQuestion)
                    {
                        var item = surveyQuestions.FirstOrDefault(c => c.Id == question.Id);
                        if (item != null)
                        {
                            item.GroupOrder = question.GroupOrder;
                            item.SubQuestionOrder = question.SubQuestionOrder;
                        }
                    }
                }
                await _unitOfWork.CompleteAsync();
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
            }
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> SurveyClone(Survey RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            RM.Models.Survey Sur_Copy = new RM.Models.Survey();
            bool success = true;

            if (RequestOwner is null)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoTokenRequested);
                return Result;
            }

            var survey = _unitOfWork.Surveys.GetAll()
                .Include(s => s.SurveyQuestions)
                .ThenInclude(t => t.QuestionsRecommendations)
                .FirstOrDefault(x => x.Id == RequestedData.Id);


            if (survey != null)
            {
                using (var dbContextTransaction = _unitOfWork.BeginTransaction())
                {
                    try
                    {
                        Sur_Copy.ReferenceId = survey.ReferenceId;
                        Sur_Copy.EntityId = survey.EntityId;
                        Sur_Copy.CreatedBy = RequestOwner.Id.Value;
                        Sur_Copy.CreatedDate = TransactionDate;
                        Sur_Copy.Code = Strings.RandomDigits(10).ToString();
                        Sur_Copy.TitleAr = survey.TitleAr + "_نسخة";
                        Sur_Copy.TitleEn = survey.TitleEn + "_Copy";
                        Sur_Copy.DescriptionAr = survey.DescriptionAr;
                        Sur_Copy.DescriptionEn = survey.DescriptionEn;
                        Sur_Copy.ShowInHomePage = survey.ShowInHomePage;
                        Sur_Copy.UseCapcha = survey.UseCapcha;
                        Sur_Copy.InnerOnly = survey.InnerOnly;
                        Sur_Copy.FromDate = survey.FromDate;
                        Sur_Copy.ToDate = survey.ToDate;
                        Sur_Copy.IsActive = false;
                        Sur_Copy.Image = survey.Image;
                        Sur_Copy.ThemeId = survey.ThemeId;

                        if (survey.SurveyQuestions.Any())
                        {
                            var surveyGroupIds = survey.SurveyQuestions.Where(x => x.IsDeleted != true).OrderBy(c => c.GroupOrder != null ? c.GroupOrder : c.Id).GroupBy(c => c.GroupId).Select(c => c.Key).ToList();
                            foreach (var groupId in surveyGroupIds)
                            {
                                var new_qId = Guid.NewGuid().ToString();
                                foreach (var q in survey.SurveyQuestions.Where(x => x.GroupId == groupId))
                                {
                                    RM.Models.SurveyQuestion ques_copy = new RM.Models.SurveyQuestion();

                                    ques_copy.SurveyId = Sur_Copy.Id;
                                    ques_copy.TextAr = q.TextAr;
                                    ques_copy.TextEn = q.TextEn;
                                    ques_copy.DescriptionAr = q.DescriptionAr;
                                    ques_copy.DescriptionEn = q.DescriptionEn;
                                    ques_copy.TypeId = q.TypeId;
                                    ques_copy.Mandatory = q.Mandatory;
                                    ques_copy.VerticalAnswersDirection = q.VerticalAnswersDirection;
                                    ques_copy.IsFiltration = q.IsFiltration;
                                    ques_copy.IsActive = q.IsActive;
                                    ques_copy.CreatedDate = TransactionDate;
                                    ques_copy.CreatedBy = Sur_Copy.CreatedBy;
                                    ques_copy.GroupId = new_qId;
                                    ques_copy.GroupOrder = q.GroupOrder;
                                    ques_copy.SubQuestionOrder = q.SubQuestionOrder;
                                    ques_copy.MinValue = q.MinValue;
                                    ques_copy.MaxValue = q.MaxValue;
                                    ques_copy.Image = q.Image;
                                    ques_copy.IsNoteRequired = q.IsNoteRequired;
                                    ques_copy.QuestionsRecommendations = q.QuestionsRecommendations;
                                    ques_copy.QuestionsRecommendations.QuestionId = ques_copy.Id;
                                    Sur_Copy.SurveyQuestions.Add(ques_copy);

                                }
                                var qDataSources = _unitOfWork.SurveyDataSources.GetAll().Where(x => x.IsDeleted != true && x.GroupId.Trim() == groupId.Trim()).ToList();

                                foreach (var d in qDataSources)
                                {
                                    RM.Models.SurveyDataSource ds_copy = new RM.Models.SurveyDataSource();
                                    ds_copy.QuestionId = null;
                                    ds_copy.GroupId = new_qId;
                                    ds_copy.CreatedBy = Sur_Copy.CreatedBy;
                                    ds_copy.CreatedDate = TransactionDate;

                                    ds_copy.TextAr = d.TextAr;
                                    ds_copy.TextEn = d.TextEn;
                                    ds_copy.DescriptionAr = d.DescriptionAr;
                                    ds_copy.DescriptionEn = d.DescriptionEn;
                                    ds_copy.IsActive = d.IsActive;
                                    ds_copy.Image = d.Image;
                                    ds_copy.Rate = d.Rate;
                                    _unitOfWork.SurveyDataSources.Add(ds_copy);
                                    success = _unitOfWork.Complete() > 0;

                                }


                            }
                        }

                        _unitOfWork.Surveys.Add(Sur_Copy);
                        success = _unitOfWork.Complete() > 0;


                        var publishReferences = _unitOfWork.PublishEntities.FindAll(x => x.ItemId == RequestedData.Id, c => c.Reference).ToList();

                        foreach (var p in publishReferences)
                        {
                            RM.Models.PublishEntities p_copy = new RM.Models.PublishEntities();
                            p_copy.CreatedBy = RequestOwner.Id;
                            p_copy.CreatedDate = TransactionDate;
                            p_copy.EntityId = p.EntityId;
                            p_copy.ReferenceId = p.ReferenceId;
                            p_copy.ItemId = Sur_Copy.Id;

                            _unitOfWork.PublishEntities.Add(p_copy);
                            success = _unitOfWork.Complete() > 0;
                        }

                        if (success)
                        {
                            dbContextTransaction.Commit();
                            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.PleaseRedisplayData);
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionErorr);
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionErorr);
                    }
                }
            }

            return Result;

        }

        public async Task<OperationOutput> ModelActions(Survey RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (RequestedData.IsDeleted.HasValue)
            {
                await _unitOfWork.Surveys.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                       sett => sett.SetProperty(x => x.IsDeleted, RequestedData.IsDeleted.Value)
                       .SetProperty(d => d.DeletedBy, RequestOwner.Id)
                       .SetProperty(y => y.DeletedDate, TransactionDate));

                await _unitOfWork.SurveyQuestions.ExecuteUpdateAsync(x => x.SurveyId == RequestedData.Id,
                                     sett => sett.SetProperty(x => x.IsDeleted, RequestedData.IsDeleted.Value)
                                     .SetProperty(d => d.DeletedBy, RequestOwner.Id)
                                     .SetProperty(y => y.DeletedDate, TransactionDate));

            }
            if (RequestedData.IsActive.HasValue)
            {
                await _unitOfWork.Surveys.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                        sett => sett.SetProperty(x => x.IsActive, RequestedData.IsActive.Value)
                        .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                        .SetProperty(y => y.UpdatedDate, TransactionDate));
            }


            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        public async Task<OperationOutput> GetSurveysStatistics(Survey RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var NotExpiredServeys = await _unitOfWork.Surveys.CountAsync(x => x.ReferenceId == RequestedData.ReferenceId && x.ToDate.Value >= TransactionDate && x.FromDate.Value < TransactionDate && x.IsActive == true && x.IsDeleted != true);
            var ExpiredServeys = await _unitOfWork.Surveys.CountAsync(x => x.ReferenceId == RequestedData.ReferenceId && x.ToDate.Value < TransactionDate || x.IsActive != true && x.IsDeleted != true);
            var AnswersCount = await _unitOfWork.SurveyAnswerActions.GetAll().Include(x => x.Survey).CountAsync(x => x.Survey.ReferenceId == RequestedData.ReferenceId);

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.NotExpiredServeys, NotExpiredServeys),
             new OutputDictionary(OperationOutputKeys.ExpiredServeys, ExpiredServeys),
             new OutputDictionary(OperationOutputKeys.AnswersCount, AnswersCount));
        }


        public async Task<OperationOutput> GetSurveyAnswersStatistics(SurveyResult RequestedData)
        {
            OperationOutput Result = null;
            List<SurveyResult> SurveyStatistics = null;
            List<SurveyResult> SurveyAnswers = null;
            int surveyActionCount = 0;
            //if (RequestedData.DropDownIds.Count > 1)
            //    Result = _memoryCache.Get<OperationOutput>(RequestedData.SurveyId + RequestedData.DropDownIds[1]);
            //else
            //    Result = _memoryCache.Get<OperationOutput>(RequestedData.SurveyId);
            //if (Result != null)
            //    return Result;
            List<int> dropDown = new List<int>();
            if (RequestedData.DropDownIds.Count > 1)
                dropDown = RequestedData.DropDownIds.Select(x => Accessor.Set(x).Value).ToList();

            string ProcedureStatistics = "sp_getSurveyStatistics";
            string ProcedureAnswers = "sp_getSurveyAnswers";

            List<Tuple<string, object>> Params = new List<Tuple<string, object>>();
            Params.Add(new Tuple<string, object>("surveyId", RequestedData._surveyId.HasValue ? RequestedData._surveyId : DBNull.Value));
            Params.Add(new Tuple<string, object>("surveyActionTableDynamicParams", RequestedData.DropDownIds != null && dropDown.Count > 1 ? Strings.ConvertListToString(dropDown.Select(x => x.ToString()).ToList(), "|") : DBNull.Value));
            Params.Add(new Tuple<string, object>("FromDate", RequestedData.FromDate.HasValue ? RequestedData.FromDate : new DateTime(2000, 1, 1)));
            Params.Add(new Tuple<string, object>("ToDate", RequestedData.ToDate.HasValue ? RequestedData.ToDate : TransactionDate));

            var survey = await _unitOfWork.Surveys.GetAll().Include(x => x.SurveyQuestions).AsNoTracking().FirstOrDefaultAsync(x => x.Id == RequestedData._surveyId && x.IsDeleted != true);

            var surveyDto = survey.Adapt<Survey>(Survey.SelectConfig(imagesGetPath));

            var questIds = survey.SurveyQuestions.Where(x => x.IsDeleted != true).Select(x => x.Id).ToList();
            var QuestionsRecomendations = _unitOfWork.QuestionsRecommendations.GetAll()
                .Include(x => x.LessAverage).Include(x => x.Average).Include(x => x.AboveAverage).Where(x => questIds.Contains(x.QuestionId)).AsNoTracking().ToList();

            SurveyStatistics = _unitOfWork.SurveyResult.QueryProcedure(ProcedureStatistics, Params).ToList()
            .Select(x => new SurveyResult()
            {
                _surveyId = x.SurveyId,
                SurveyTitle = x.SurveyTitle,
                SurveyTitleEn = x.SurveyTitleEn,
                _questionId = x.QuestionId,
                _questionTypeId = x.QuestionTypeId,
                Question = x.Question,
                QuestionEn = x.QuestionEn,
                Count = x.Count,
                _dataSourceId = x.DataSourceId,
                Choice = x.Choice,
                ChoiceEn = x.ChoiceEn,
                GroupId = x.GroupId,
                Rate = x.Rate,
                //  CreatedDate = x.CreatedDate

            }).ToList();


            SurveyAnswers = _unitOfWork.SurveyResult.QueryProcedure(ProcedureAnswers, Params).ToList()
                .Select(x => new SurveyResult()
                {
                    _surveyId = x.SurveyId,
                    SurveyTitle = x.SurveyTitle,
                    SurveyTitleEn = x.SurveyTitleEn,
                    _questionId = x.QuestionId,
                    _questionTypeId = x.QuestionTypeId,
                    Question = x.Question,
                    QuestionEn = x.QuestionEn,
                    AnswerText = x.AnswerText,
                    AnswerValue = x.AnswerValue,
                    GroupId = x.GroupId,
                    Rate = x.Rate
                }).ToList();

            if (dropDown.Count > 1)
                surveyActionCount = SurveyStatistics.Where(x => x._questionId == dropDown[0] && x._dataSourceId == dropDown[1]).FirstOrDefault() != null ? SurveyStatistics.Where(x => x._questionId == dropDown[0] && x._dataSourceId == dropDown[1]).FirstOrDefault().Count.Value : 0;
            else
                surveyActionCount = SurveyStatistics.Max(x => x.Count.Value);


            var groupResult = SurveyStatistics.Where(x => dropDown.Count > 0 ? x._questionId != dropDown[0] : true).ToList().GroupBy(x => new { x.QuestionId }, (key, group) => new
            {
                QuestionId = key.QuestionId,
                Question = group.First().Question,
                QuestionEn = group.First().QuestionEn,
                Sum = group.Sum(s => s.Count),
                Rate = group.Sum(s => s.Count) > 0 ? group.Sum(s => s.Rate * s.Count) / group.Sum(s => s.Count) : 0,
                QuestionRecommendationAr = QuestionRecommendationAr(group.Sum(s => s.Count) > 0 ? group.Sum(s => s.Rate * s.Count) / group.Sum(s => s.Count) : 0, group.First()._questionId, QuestionsRecomendations),
                QuestionRecommendationEn = QuestionRecommendationEn(group.Sum(s => s.Count) > 0 ? group.Sum(s => s.Rate * s.Count) / group.Sum(s => s.Count) : 0, group.First()._questionId, QuestionsRecomendations),
                Result = group.Select(r => new SurveyResult()
                {
                    _surveyId = r._surveyId,
                    SurveyTitle = r.SurveyTitle,
                    SurveyTitleEn = r.SurveyTitleEn,
                    _questionId = r._questionId,
                    _questionTypeId = r._questionId,
                    Question = r.Question,
                    QuestionEn = r.QuestionEn,
                    Count = r.Count,
                    _dataSourceId = r._dataSourceId,
                    Choice = r.Choice,
                    ChoiceEn = r.ChoiceEn,
                    AnswerText = r.AnswerText,
                    AnswerValue = r.AnswerValue,
                    Sum = group.Sum(s => s.Count),
                    Avg = group.Sum(s => s.Count) > 0 ? (double)(r.Count * 100) / (double)group.Sum(s => s.Count) : 0.0,

                }).ToList()
            });

            var qroupSurveyAnswers = SurveyAnswers.GroupBy(x => new { x.QuestionTypeId }, (key, group) => new
            {
                QuestionTypeId = key.QuestionTypeId,
                Question = group.First().Question,
                QuestionEn = group.First().QuestionEn,
                Questions = group.ToList(),
            });

            var RatingType = _unitOfWork.SurveyQuestionTypes.GetAll().FirstOrDefault(x => x.Type == SurveyInputTypes.RatingType);
            var RatingQuestsResult = SurveyAnswers.Where(x => x._questionTypeId == RatingType.Id).GroupBy(x => new { x.QuestionId }, (key, group) => new
            {
                QuestionId = key.QuestionId,
                Question = group.First().Question,
                QuestionEn = group.First().QuestionEn,
                Count = group.Count(),
                Sum = group.Count(),
                Result = group.GroupBy(x => new { x.AnswerValue }, (key, group2) => new SurveyResult()
                {
                    AnswerValue = key.AnswerValue,
                    Choice = key.AnswerValue.ToString(),
                    ChoiceEn = key.AnswerValue.ToString(),
                    Count = group2.Count(),
                    Sum = group2.Count(),
                    Avg = group2.Count() > 0 ? (double)(group2.Count() * 100) / (double)group.Count() : 0.0
                }).ToList()
                .OrderByDescending(x => x.AnswerValue)
            });

            var RangeType = _unitOfWork.SurveyQuestionTypes.GetAll().FirstOrDefault(x => x.Type == SurveyInputTypes.RangeType);
            var RangeQuestsResult = SurveyAnswers.Where(x => x._questionTypeId == RangeType.Id).GroupBy(x => new { x.QuestionId }, (key, group) => new
            {
                QuestionId = key.QuestionId,
                Question = group.First().Question,
                QuestionEn = group.First().QuestionEn,
                Count = group.Count(),
                Sum = group.Count(),
                Result = group.GroupBy(x => new { x.AnswerText }, (key, group2) => new
                {
                    AnswerValue = JsonConvert.DeserializeObject<List<int>>(key.AnswerText),
                    Choice = key.AnswerText.ToString(),
                    ChoiceEn = key.AnswerText.ToString(),
                    Count = group2.Count(),
                    Sum = group2.Count(),
                    Avg = group2.Count() > 0 ? (double)(group2.Count() * 100) / (double)group.Count() : 0.0
                }).ToList()
            });

            var SurveyStatisticRates = await GetSurveyStatisticRates(RequestedData);

            var ResultLookUps = await _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.SurveyResult).ToListAsync();
            var ResultLookUpsDto = ResultLookUps.Adapt<List<MajorLookups>>(MajorLookups.SelectConfig());

            SurveyDataSource dataSource = null;
            if (RequestedData.DropDownIds.Count > 1)
            {
                var dataSourceDb = _unitOfWork.SurveyDataSources.Find(x => x.Id == Accessor.Set(RequestedData.DropDownIds[1]).Value);
                dataSource = dataSourceDb.Adapt<SurveyDataSource>(SurveyDataSource.SelectConfig(imagesGetPath));
            }

            Result = new OperationOutput();
            Result.Output = new Dictionary<string, object>()
            {
                { OperationOutput.OperationOutputKeys.SurveysEntity,surveyDto},
                { OperationOutput.OperationOutputKeys.SurveyStatistics,SurveyStatistics.Where(x => dropDown.Count > 0 ? x._questionId != dropDown[0] : true).ToList()},
                { OperationOutput.OperationOutputKeys.SurveyAnswers,SurveyAnswers},
                { OperationOutput.OperationOutputKeys.SurveyGroupResult,groupResult},
                { OperationOutput.OperationOutputKeys.GroupSurveyAnswers,qroupSurveyAnswers},
                { OperationOutput.OperationOutputKeys.SurveyAnswerCount,surveyActionCount},
                { OperationOutput.OperationOutputKeys.RatingQuestsResult,RatingQuestsResult},
                { OperationOutput.OperationOutputKeys.RangeQuestsResult,RangeQuestsResult},
                { OperationOutput.OperationOutputKeys.SurveyStatisticRates,SurveyStatisticRates},
                { OperationOutput.OperationOutputKeys.ResultLookUps,ResultLookUpsDto},
                { OperationOutput.OperationOutputKeys.SurveyService,dataSource}

            };

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }


        public async Task<OperationOutput> GetSurveyAnswersStatisticsReport(SurveyResult RequestedData)
        {
            OperationOutput Result = null;
            List<SurveyResult> SurveyStatistics = null;
            int surveyActionCount = 0;
            if (RequestedData.DropDownIds.Count > 1)
                Result = _unitOfWork.MemoryCache.Get<OperationOutput>(RequestedData.SurveyId + RequestedData.DropDownIds[1]);
            else
                Result = _unitOfWork.MemoryCache.Get<OperationOutput>(RequestedData.SurveyId);
            if (Result != null)
                return Result;

            List<int> dropDown = new List<int>();
            if (RequestedData.DropDownIds.Count > 1)
                dropDown = RequestedData.DropDownIds.Select(x => Accessor.Set(x).Value).ToList();

            string ProcedureStatistics = "sp_getSurveyStatistics";

            List<Tuple<string, object>> Params = new List<Tuple<string, object>>();
            Params.Add(new Tuple<string, object>("surveyId", RequestedData._surveyId.HasValue ? RequestedData._surveyId : DBNull.Value));
            Params.Add(new Tuple<string, object>("surveyActionTableDynamicParams", RequestedData.DropDownIds != null && dropDown.Count > 1 ? Strings.ConvertListToString(dropDown.Select(x => x.ToString()).ToList(), "|") : DBNull.Value));
            Params.Add(new Tuple<string, object>("FromDate", RequestedData.FromDate.HasValue ? RequestedData.FromDate : new DateTime(2000, 1, 1)));
            Params.Add(new Tuple<string, object>("ToDate", RequestedData.ToDate.HasValue ? RequestedData.ToDate : TransactionDate));

            var survey = await _unitOfWork.Surveys.GetAll().Include(x => x.SurveyQuestions).AsNoTracking().FirstOrDefaultAsync(x => x.Id == RequestedData._surveyId && x.IsDeleted != true);

            var surveyDto = survey.Adapt<Survey>(Survey.SelectConfig(imagesGetPath));

            var questIds = survey.SurveyQuestions.Where(x => x.IsDeleted != true).Select(x => x.Id).ToList();
            var QuestionsRecomendations = _unitOfWork.QuestionsRecommendations.GetAll()
                .Include(x => x.LessAverage).Include(x => x.Average).Include(x => x.AboveAverage).Where(x => questIds.Contains(x.QuestionId)).AsNoTracking().ToList();

            SurveyStatistics = _unitOfWork.SurveyResult.QueryProcedure(ProcedureStatistics, Params).ToList()
            .Select(x => new SurveyResult()
            {
                _surveyId = x.SurveyId,
                SurveyTitle = x.SurveyTitle,
                SurveyTitleEn = x.SurveyTitleEn,
                _questionId = x.QuestionId,
                _questionTypeId = x.QuestionTypeId,
                Question = x.Question,
                QuestionEn = x.QuestionEn,
                Count = x.Count,
                _dataSourceId = x.DataSourceId,
                Choice = x.Choice,
                ChoiceEn = x.ChoiceEn,
                GroupId = x.GroupId,
                Rate = x.Rate,
                //  CreatedDate = x.CreatedDate
            }).ToList();


            if (dropDown.Count > 1)
                surveyActionCount = SurveyStatistics.Where(x => x._questionId == dropDown[0] && x._dataSourceId == dropDown[1]).FirstOrDefault() != null ? SurveyStatistics.Where(x => x._questionId == dropDown[0] && x._dataSourceId == dropDown[1]).FirstOrDefault().Count.Value : 0;
            else
                surveyActionCount = SurveyStatistics.Max(x => x.Count.Value);


            var groupResult = SurveyStatistics.Where(x => dropDown.Count > 0 ? x._questionId != dropDown[0] : true).ToList().GroupBy(x => new { x.QuestionId }, (key, group) => new
            {
                QuestionId = key.QuestionId,
                Question = group.First().Question,
                QuestionEn = group.First().QuestionEn,
                Sum = group.Sum(s => s.Count),
                Rate = group.Sum(s => s.Count) > 0 ? group.Sum(s => s.Rate * s.Count) / group.Sum(s => s.Count) : 0,
                QuestionRecommendationAr = QuestionRecommendationAr(group.Sum(s => s.Count) > 0 ? group.Sum(s => s.Rate * s.Count) / group.Sum(s => s.Count) : 0, group.First()._questionId, QuestionsRecomendations),
                QuestionRecommendationEn = QuestionRecommendationEn(group.Sum(s => s.Count) > 0 ? group.Sum(s => s.Rate * s.Count) / group.Sum(s => s.Count) : 0, group.First()._questionId, QuestionsRecomendations),
                Result = group.Select(r => new SurveyResult()
                {
                    _surveyId = r._surveyId,
                    SurveyTitle = r.SurveyTitle,
                    SurveyTitleEn = r.SurveyTitleEn,
                    _questionId = r._questionId,
                    _questionTypeId = r._questionId,
                    Question = r.Question,
                    QuestionEn = r.QuestionEn,
                    Count = r.Count,
                    _dataSourceId = r._dataSourceId,
                    Choice = r.Choice,
                    ChoiceEn = r.ChoiceEn,
                    AnswerText = r.AnswerText,
                    AnswerValue = r.AnswerValue,
                    Sum = group.Sum(s => s.Count),
                    Avg = group.Sum(s => s.Count) > 0 ? (double)(r.Count * 100) / (double)group.Sum(s => s.Count) : 0.0,

                }).ToList()
            });



            var SurveyStatisticRates = await GetSurveyStatisticRates(RequestedData);

            var ResultLookUps = await _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.SurveyResult).ToListAsync();
            var ResultLookUpsDto = ResultLookUps.Adapt<List<MajorLookups>>(MajorLookups.SelectConfig());

            SurveyDataSource dataSource = null;
            if (RequestedData.DropDownIds.Count > 1)
            {
                var dataSourceDb = _unitOfWork.SurveyDataSources.Find(x => x.Id == Accessor.Set(RequestedData.DropDownIds[1]).Value);
                dataSource = dataSourceDb.Adapt<SurveyDataSource>(SurveyDataSource.SelectConfig(imagesGetPath));
            }

            Result = new OperationOutput();
            Result.Output = new Dictionary<string, object>()
            {
                { OperationOutput.OperationOutputKeys.SurveysEntity,surveyDto},
                { OperationOutput.OperationOutputKeys.SurveyStatistics,SurveyStatistics.Where(x => dropDown.Count > 0 ? x._questionId != dropDown[0] : true).ToList()},
                { OperationOutput.OperationOutputKeys.SurveyGroupResult,groupResult},
                { OperationOutput.OperationOutputKeys.SurveyAnswerCount,surveyActionCount},
                { OperationOutput.OperationOutputKeys.SurveyStatisticRates,SurveyStatisticRates},
                { OperationOutput.OperationOutputKeys.ResultLookUps,ResultLookUpsDto},
                { OperationOutput.OperationOutputKeys.SurveyService,dataSource}

            };

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }


        private SurveyRecommendations QuestionRecommendationAr(int? rate, int? questId, List<RM.Models.QuestionsRecommendations> QuestionsRecomendations)
        {
            if (rate < 5)
                return QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId) != null ? QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId).LessAverage != null ? QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId).LessAverage.Adapt<SurveyRecommendations>(SurveyRecommendations.SelectConfig()) : new SurveyRecommendations() : new SurveyRecommendations();
            else if (rate == 5)
                return QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId) != null ? QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId).Average != null ? QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId).Average.Adapt<SurveyRecommendations>(SurveyRecommendations.SelectConfig()) : new SurveyRecommendations() : new SurveyRecommendations();
            else if (rate > 5)
                return QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId) != null ? QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId).AboveAverage != null ? QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId).AboveAverage.Adapt<SurveyRecommendations>(SurveyRecommendations.SelectConfig()) : new SurveyRecommendations() : new SurveyRecommendations();
            else return new SurveyRecommendations();
        }
        private SurveyRecommendations QuestionRecommendationEn(int? rate, int? questId, List<RM.Models.QuestionsRecommendations> QuestionsRecomendations)
        {
            if (rate < 5)
                return QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId) != null ? QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId).LessAverage != null ? QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId).LessAverage.Adapt<SurveyRecommendations>(SurveyRecommendations.SelectConfig()) : new SurveyRecommendations() : new SurveyRecommendations();
            else if (rate == 5)
                return QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId) != null ? QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId).Average != null ? QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId).Average.Adapt<SurveyRecommendations>(SurveyRecommendations.SelectConfig()) : new SurveyRecommendations() : new SurveyRecommendations();
            else if (rate > 5)
                return QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId) != null ? QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId).AboveAverage != null ? QuestionsRecomendations.FirstOrDefault(x => x.QuestionId == questId).AboveAverage.Adapt<SurveyRecommendations>(SurveyRecommendations.SelectConfig()) : new SurveyRecommendations() : new SurveyRecommendations();
            else return new SurveyRecommendations();
        }

        public async Task<OperationOutput> GetSurveysWithStatisticsList(Survey RequestedData)
        {
            OperationOutput Result = null;
            var SurveyStatistics = new List<SurveyResultStatistices>();
            if (RequestOwner is null)
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoTokenRequested);
                return Result;
            }
            var publishedSurveys = _unitOfWork.PublishEntities.FindAll(c => c.EntityId == (int)Enums.Entities.Survey && c.ReferenceId == RequestedData.ReferenceId).ToList();

            var filteration = RequestedData.Filteration(publishedSurveys.Select(x => x.ItemId).ToList());

            var surveys = await _unitOfWork.Surveys.GetAll().Include(a => a.SurveyAnswerActions)
                .Where(filteration).OrderByDescending(x => x.ToDate ?? DateTime.MaxValue).TakePagginationAsync(RequestedData.Pagination, DefaultPaginationCount);

            foreach (var survey in surveys.Data)
            {
                if (survey.SurveyAnswerActions != null)
                {
                    var Item = survey.SurveyAnswerActions
                        .GroupBy(x => new { x.CreatedDate.Value.Date.Year }, (key, group) => new
                        {
                            Year = key.Year,
                            YearCount = group.Count(),
                            Months = group.GroupBy(x => new { x.CreatedDate.Value.Date.Month }, (key, group2) => new
                            {
                                Month = key.Month,
                                MonthCount = group2.Count(),
                                Days = group2.GroupBy(x => new { x.CreatedDate.Value.Date.Day }, (key, group3) => new
                                {
                                    Day = key.Day,
                                    DayCount = group3.Count()
                                }).ToList()
                            })
                        });

                    var surveyStatistic = new SurveyResultStatistices
                    {
                        SurveyId = survey.Id,
                        TitleAr = survey.TitleAr,
                        TitleEn = survey.TitleEn,
                        Years = new List<YearResult>(),
                        Day = 0,
                        Week = 0,
                        Month = 0,
                        Year = 0,
                        Total = 0
                    };

                    for (int i = 0; i < 5; i++)
                    {
                        var Year = Item.Where(x => x.Year == TransactionDate.Year - i).FirstOrDefault();
                        var year = new YearResult { Year = TransactionDate.Year - i, Monthes = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

                        if (Year != null)
                        {
                            year.Monthes = new List<int>();
                            for (int j = 1; j <= 12; j++)
                            {
                                var cym = Year.Months.FirstOrDefault(x => x.Month == j);
                                year.Monthes.Add(cym != null ? cym.MonthCount : 0);
                                year.YearCount = Year.YearCount;
                            }

                            if (i == 0)
                            {
                                var startWeekDate = TransactionDate.StartOfWeek(DayOfWeek.Saturday);
                                var endWeekDate = startWeekDate.AddDays(6);

                                var month = Year.Months.Where(x => x.Month == TransactionDate.Month).FirstOrDefault();
                                var day = month != null ? month.Days.Where(x => x.Day == TransactionDate.Day).FirstOrDefault() : null;
                                surveyStatistic.Day = day != null ? day.DayCount : 0;
                                //surveyStatistic.Week = currentYear.Months.Where(x => x.Month == TransactionDate.Month).FirstOrDefault().Days.Where(x => x.Day + 7 <= TransactionDate.Day).Sum(x => x.DayCount);
                                surveyStatistic.Week = month != null ? month.Days.Where(x => x.Day >= startWeekDate.Day && x.Day <= endWeekDate.Day).Sum(x => x.DayCount) : 0;
                                surveyStatistic.Month = month != null ? month.MonthCount : 0;
                                surveyStatistic.Year = Year.YearCount;
                                surveyStatistic.Total = Item.Sum(x => x.YearCount);
                            }
                        }
                        surveyStatistic.Years.Add(year);

                    }

                    SurveyStatistics.Add(surveyStatistic);
                }
            }

            Result = new OperationOutput();
            Result.Output = new Dictionary<string, object>()
            {
                { OperationOutput.OperationOutputKeys.SurveyStatistics,SurveyStatistics},
                { OperationOutput.OperationOutputKeys.Pagination,surveys.Pagination}
            };

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }

        private async Task<List<YearResult>> GetSurveyStatisticRates(SurveyResult RequestedData)
        {
            List<RM.Models.SurveyQuestionAnswer> surveyResult = new List<RM.Models.SurveyQuestionAnswer>();
            List<YearResult> Years = new List<YearResult>();

            var RadioType = _unitOfWork.SurveyQuestionTypes.GetAll().FirstOrDefault(x => x.Type == SurveyInputTypes.RadioType);
            var gridRadioType = _unitOfWork.SurveyQuestionTypes.GetAll().FirstOrDefault(x => x.Type == SurveyInputTypes.GridRadioType);
            List<int> DropDownIds = RequestedData.DropDownIds != null ? RequestedData.DropDownIds.Select(x => Accessor.Set(x).Value).ToList() : new List<int>();

            if (DropDownIds.Count == 2)
            {
                var SurveyAnswerActions = await _unitOfWork.SurveyQuestionAnswers.GetAll()
                    .Where(x => x.QuestionId == DropDownIds[0] && x.DataSourceId == DropDownIds[1])
                    //.Where(x => RequestedData.FromDate.HasValue ? x.SurveyAnswerAction.CreatedDate.Value.Date >= RequestedData.FromDate.Value.Date : true)
                    //.Where(x => RequestedData.ToDate.HasValue ? x.SurveyAnswerAction.CreatedDate.Value.Date <= RequestedData.ToDate.Value.Date : true)
                    .Select(x => x.SurveyAnswerActionId).ToListAsync();

                surveyResult = await _unitOfWork.SurveyQuestionAnswers.GetAll()
                   .Where(x => SurveyAnswerActions.Contains(x.SurveyAnswerActionId.Value))
                   .Where(x => x.SurveyAnswerAction.SurveyId == RequestedData._surveyId)
                   .Where(x => x.Question.TypeId == RadioType.Id || x.Question.TypeId == gridRadioType.Id)
                   .Where(x => x.DataSourceId != null)
                   .Where(x => x.Question.IsDeleted != true)
                   .Include(x => x.DataSource)
                   .Include(x => x.SurveyAnswerAction)
                   .ToListAsync();
            }
            else
            {
                surveyResult = await _unitOfWork.SurveyQuestionAnswers.GetAll()
                       .Where(x => x.SurveyAnswerAction.SurveyId == RequestedData._surveyId)
                       .Where(x => x.Question.TypeId == RadioType.Id || x.Question.TypeId == gridRadioType.Id)
                       .Where(x => x.DataSourceId != null)
                       .Where(x => x.Question.IsDeleted != true)
                       //.Where(x => RequestedData.FromDate.HasValue ? x.SurveyAnswerAction.CreatedDate.Value.Date >= RequestedData.FromDate.Value.Date : true)
                       //.Where(x => RequestedData.ToDate.HasValue ? x.SurveyAnswerAction.CreatedDate.Value.Date <= RequestedData.ToDate.Value.Date : true)
                       .Include(x => x.DataSource)
                       .Include(x => x.SurveyAnswerAction)
                       .ToListAsync();
            }

            if (surveyResult != null)
            {
                var Item = surveyResult
                    .GroupBy(x => new { x.SurveyAnswerAction.CreatedDate.Value.Date.Year }, (key, group) => new
                    {
                        Year = key.Year,
                        YearRate = group.Sum(x => x.DataSource.Rate) / group.Count(),
                        YearCount = group.Select(x => x.SurveyAnswerActionId).Distinct().Count(),
                        Months = group.GroupBy(x => new { x.SurveyAnswerAction.CreatedDate.Value.Date.Month }, (key, group2) => new
                        {
                            Month = key.Month,
                            MonthRate = group2.Sum(x => x.DataSource.Rate) / group2.Count(),
                            MonthCount = group2.Select(x => x.SurveyAnswerActionId).Distinct().Count(),
                            MonthAvg = group2.Count() > 0 ? (double)(group2.Count() * 100) / (double)group.Count() : 0.0
                            //Days = group.GroupBy(x => new { x.SurveyAnswerAction.CreatedDate.Value.Date.Day }, (key, group3) => new
                            //{
                            //    Day = key.Day,
                            //    DayRate = group3.Sum(x => x.DataSource.Rate) / group3.Count()
                            //}).ToList()
                        })
                    });

                for (int i = 0; i < 10; i++)
                {
                    var Year = Item.Where(x => x.Year == TransactionDate.Year - i).FirstOrDefault();

                    if (Year != null)
                    {
                        var year = new YearResult { Year = Year.Year, Monthes = new List<int>(), MonthesAvg = new List<double>(), MonthesCount = new List<int>() };
                        for (int j = 1; j <= 12; j++)
                        {
                            var cym = Year.Months.FirstOrDefault(x => x.Month == j);
                            year.Year = Year.Year;
                            year.Monthes.Add(cym != null ? cym.MonthRate : 0);
                            year.MonthesCount.Add(cym != null ? cym.MonthCount : 0);
                            year.MonthesAvg.Add(cym != null ? cym.MonthAvg : 0.0);
                            year.YearRate = Year.YearRate; year.YearCount = Year.YearCount;
                        }
                        Years.Add(year);
                    }
                }
            }

            return Years;
        }




        #endregion

        #region  QUESTIONS
        public async Task<OperationOutput> GetQuestionDetails(SurveyQuestion RequestedData)
        {
            var surveyQuestion = await _unitOfWork.SurveyQuestions.GetAll()
                .Include(s => s.SurveyDataSources)
                .Where(x => x.Id == RequestedData.Id).FirstOrDefaultAsync();

            var surveyQuestionDto = surveyQuestion.Adapt<SurveyQuestion>(SurveyQuestion.SelectConfig(ImagesGetPath));

            var surveyDataSources = _unitOfWork.SurveyDataSources.GetAll()
                .Where(x => x.IsDeleted != true && x.GroupId.Trim() == surveyQuestionDto.GroupId.Trim())
                .Where(x => RequestedData.IsActive.HasValue ? x.IsActive == true : true).AsNoTracking().ToList();

            surveyQuestionDto.SurveyDataSources = surveyDataSources.Adapt<List<SurveyDataSource>>(SurveyDataSource.SelectConfig(ImagesGetPath));

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.SurveyQuestionEntity, surveyQuestionDto));

        }

        public async Task<OperationOutput> QuestionModelActions(SurveyQuestion RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var dbItem = await _unitOfWork.SurveyQuestions.GetByIdAsync(RequestedData.Id.Value);
            if (dbItem == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            dbItem.IsActive = RequestedData.IsActive.HasValue ? RequestedData.IsActive.Value : dbItem.IsActive;
            dbItem.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : dbItem.IsDeleted;
            if (RequestedData.IsDeleted.HasValue && RequestedData.IsDeleted.Value == true)
            {
                dbItem.DeletedBy = RequestOwner.Id;
                dbItem.DeletedDate = TransactionDate;
            }

            dbItem.UpdatedBy = RequestOwner.Id;
            dbItem.UpdatedDate = TransactionDate;

            _unitOfWork.SurveyQuestions.Update(dbItem);
            _unitOfWork.Complete();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> GetAllQuestionsList(SurveyQuestion RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<GroupQuestion> questionList = new List<GroupQuestion>();
            List<GroupQuestion> filterdQuestionList = new List<GroupQuestion>();

            var surveyQuestions = await _unitOfWork.SurveyQuestions.GetAll().Include(t => t.QuestionType)
                .Where(r => r.Survey.ReferenceId == RequestedData.ReferenceId)
                .Where(t => RequestedData.TypeId != null ? t.TypeId == RequestedData.TypeId : true)
                .Where(g => g.IsGlobal == true)
                .ToListAsync();

            var surveyDataSource = _unitOfWork.SurveyDataSources.FindAll(c => c.IsDeleted != true).ToList();

            if (surveyQuestions.Any())
            {
                var surveyGroupIds = surveyQuestions.Where(x => x.IsDeleted != true).GroupBy(c => c.GroupId).Select(c => c.Key).ToList();
                foreach (var group in surveyGroupIds)
                {
                    var new_qId = Guid.NewGuid().ToString();
                    GroupQuestion groupQuestion = FillGroupQuestionForAllQuestions(surveyQuestions, surveyDataSource, group, new_qId);
                    questionList.Add(groupQuestion);
                }
            }

            foreach (var question in questionList)
            {
                var repet = filterdQuestionList.Find(x => x.SurveyQuestion.FirstOrDefault().TextAr.Trim() == question.SurveyQuestion.FirstOrDefault().TextAr.Trim());
                if (repet == null)
                    filterdQuestionList.Add(question);
            }

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.SurveysEntity, filterdQuestionList));
        }

        #region HELPER METHOD
        private static GroupQuestion FillGroupQuestionForAllQuestions(List<RM.Models.SurveyQuestion> surveyQuestions, List<RM.Models.SurveyDataSource> surveyDataSource, string group, string new_qId)
        {
            var surveyQuestionModel = surveyQuestions.Where(x => x.IsDeleted != true && x.GroupId == group)
               .OrderBy(c => c.SubQuestionOrder != null ? c.SubQuestionOrder
               : (c.GroupOrder != null ? c.GroupOrder : c.Id)).ToList();

            var groupDataSourceModel = surveyDataSource.Where(c => c.GroupId == group).OrderBy(c => c.Id)
                                      .Select(c => { c.GroupId = new_qId; return c; }).ToList();

            return new GroupQuestion
            {
                GroupId = new_qId,
                _typeId = surveyQuestions.FirstOrDefault(x => x.IsDeleted != true && x.GroupId == group).TypeId,
                SurveyQuestion = surveyQuestionModel.Adapt<List<SurveyQuestion>>(SurveyQuestion.SelectConfig(imagesGetPath)),
                GroupDataSource = groupDataSourceModel.Adapt<List<SurveyDataSource>>(SurveyDataSource.SelectConfig(imagesGetPath))
            };
        }

        #endregion
        public async Task<OperationOutput> QuestionsGlobalList(List<SurveyQuestion> RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var groups = RequestedData.Select(x => x.GroupId).ToList();
            var questionsInGroups = await _unitOfWork.SurveyQuestions.GetAll().Where(x => x.IsDeleted != true && groups.Contains(x.GroupId)).AsNoTracking().ToListAsync();

            foreach (var question in questionsInGroups)
            {
                question.IsGlobal = RequestedData.Where(x => x.GroupId == question.GroupId).FirstOrDefault().IsGlobal;
                _unitOfWork.SurveyQuestions.Update(question);
            }
            await _unitOfWork.CompleteAsync();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> QuestionsDeleteList(List<SurveyQuestion> RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var groups = RequestedData.Select(x => x.GroupId).ToList();
            var questionsInGroups = await _unitOfWork.SurveyQuestions.GetAll().Where(x => x.IsDeleted != true && groups.Contains(x.GroupId)).ToListAsync();
            var dsInGroups = await _unitOfWork.SurveyDataSources.GetAll().Where(x => x.IsDeleted != true && groups.Contains(x.GroupId)).ToListAsync();
            foreach (var question in questionsInGroups)
            {
                question.IsDeleted = true;
                question.DeletedBy = RequestOwner.Id;
                question.DeletedDate = TransactionDate;
                _unitOfWork.SurveyQuestions.Update(question);
            }

            foreach (var ds in dsInGroups)
            {
                ds.IsDeleted = true;
                ds.DeletedBy = RequestOwner.Id;
                ds.DeletedDate = TransactionDate;
                _unitOfWork.SurveyDataSources.Update(ds);
            }
            await _unitOfWork.CompleteAsync();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        public async Task<OperationOutput> QuestionsActivationList(List<SurveyQuestion> RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var groups = RequestedData.Select(x => x.GroupId).ToList();
            var questionsInGroups = await _unitOfWork.SurveyQuestions.GetAll().Where(x => x.IsDeleted != true && groups.Contains(x.GroupId)).ToListAsync();
            var dsInGroups = await _unitOfWork.SurveyDataSources.GetAll().Where(x => x.IsDeleted != true && groups.Contains(x.GroupId)).ToListAsync();
            foreach (var question in questionsInGroups)
            {
                question.IsActive = RequestedData.Where(x => x.GroupId == question.GroupId).FirstOrDefault().IsActive ?? !question.IsActive;
                _unitOfWork.SurveyQuestions.Update(question);
            }

            foreach (var ds in dsInGroups)
            {
                ds.IsActive = RequestedData.Where(x => x.GroupId == ds.GroupId).FirstOrDefault().IsActive ?? !ds.IsActive;
                _unitOfWork.SurveyDataSources.Update(ds);
            }
            await _unitOfWork.CompleteAsync();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
        #endregion



        #region SendEmail

        public async Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord cron)
        {
            OperationOutput Result = new OperationOutput();
            List<SurveyResult> Surveys = new List<SurveyResult>();
            List<SurveyResult> SurveysEntity = new List<SurveyResult>();
            if (cron.CronTypeId == (int)Enums.CronType.WhenFinishToDate)
            {
                Surveys = _unitOfWork.CronSettings.GetAll().Include(s => s.Survey).ThenInclude(x => x.CronSettings)
                    .Where(x => x.IsActive == true && x.CronTypeId == cron.CronTypeId)
                    .Where(x => x.Survey.IsDeleted != true && x.Survey.IsActive == true && x.Survey.ToDate.Value.Date == DateTime.Now.Date)
                    .AsNoTracking().Select(s => s.Survey).ToList().Adapt<List<SurveyResult>>(SurveyResult.SelectWithSettingsConfig(cron.CronTypeId));

                var Surveys2 = _unitOfWork.Surveys.GetAll()
                                          .Where(x => x.IsDeleted != true && x.IsActive == true && x.ToDate.Value.Date == DateTime.Now.Date)
                                          .AsNoTracking().ToList();

                var CronSettings = _unitOfWork.CronSettings.GetAll().Where(x => x.IsActive == true && x.CronTypeId == cron.CronTypeId && x.EntityId == (int)Enums.Entities.Survey).AsNoTracking().ToList();

                foreach (var SE in Surveys2)
                    SE.CronSettings = CronSettings;

                SurveysEntity = Surveys2.Adapt<List<SurveyResult>>(SurveyResult.SelectWithSettingsConfig(cron.CronTypeId));
            }
            else
            {
                Surveys = _unitOfWork.CronSettings.GetAll().Include(s => s.Survey).ThenInclude(x => x.CronSettings)
                    .Where(x => x.IsActive == true && x.CronTypeId == cron.CronTypeId)
                    .Where(x => x.Survey.IsDeleted != true && x.Survey.IsActive == true && x.Survey.ToDate.Value.Date >= DateTime.Now.Date)
                    .AsNoTracking().Select(s => s.Survey).ToList().Adapt<List<SurveyResult>>(SurveyResult.SelectWithSettingsConfig(cron.CronTypeId));

                var Surveys2 = _unitOfWork.Surveys.GetAll()
                                          .Where(x => x.IsDeleted != true && x.IsActive == true && x.ToDate.Value.Date >= DateTime.Now.Date)
                                          .AsNoTracking().ToList();

                var CronSettings = _unitOfWork.CronSettings.GetAll().Where(x => x.IsActive == true && x.CronTypeId == cron.CronTypeId && x.EntityId == (int)Enums.Entities.Survey).AsNoTracking().ToList();

                foreach (var SE in Surveys2)
                    SE.CronSettings = CronSettings;

                SurveysEntity = Surveys2.Adapt<List<SurveyResult>>(SurveyResult.SelectWithSettingsConfig(cron.CronTypeId));
            }

            foreach (var survey in Surveys)
                await SendEmailSurveysStatistics(survey);

            foreach (var survey in SurveysEntity)
                await SendEmailSurveysStatistics(survey);

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }

        public async Task<OperationOutput> SendEmailSurveysStatistics(SurveyResult RequestedData)
        {
            OperationOutput Result = new OperationOutput();

            var type = _unitOfWork.SurveyQuestionTypes.Find(x => x.Type == SurveyInputTypes.DropdownType);

            var survey = _unitOfWork.Surveys.GetAll().Include(x => x.SurveyQuestions).Where(x => x.Id == RequestedData._surveyId && x.IsDeleted != true).AsNoTracking().FirstOrDefault();

            if (survey != null)
            {
                var dropdown = survey.SurveyQuestions.FirstOrDefault(x => x.TypeId == type.Id && x.IsDeleted != true && x.IsFiltration == true);
                if (dropdown != null)
                {
                    List<(byte[], string)> filesAr = new List<(byte[], string)>();
                    List<(byte[], string)> filesEn = new List<(byte[], string)>();
                    var qDataSources = _unitOfWork.SurveyDataSources.GetAll().Where(x => x.IsDeleted != true && x.GroupId.Trim() == dropdown.GroupId.Trim()).ToList();

                    foreach (var ds in qDataSources)
                    {
                        RequestedData.DropDownIds = new List<string>() { Accessor.Get(dropdown.Id), Accessor.Get(ds.Id) };
                        var data = await GetSurveyAnswersStatisticsReport(RequestedData);
                        _unitOfWork.MemoryCache.Set(RequestedData.SurveyId + RequestedData.DropDownIds[1], data, DateTimeOffset.Now.AddSeconds(30));

                        var urlAr = HtmlSurveyReportUrl + "/ar/" + RequestedData.SurveyId + "?token=" + Token.Replace("bearer ", "") + "&q=" + RequestedData.DropDownIds[0] + "&ds=" + RequestedData.DropDownIds[1];
                        var urlEn = HtmlSurveyReportUrl + "/en/" + RequestedData.SurveyId + "?token=" + Token.Replace("bearer ", "") + "&q=" + RequestedData.DropDownIds[0] + "&ds=" + RequestedData.DropDownIds[1];

                        var pdfOption = new PdfOptions
                        {
                            Format = PaperFormat.Tabloid,
                            PrintBackground = true,
                            Landscape = true
                        };
                        var pdffFileAr = await PDF.GeneratePdfFromUrlAsync(urlAr, PDFServiceUrl, pdfOption, Token);
                        var pdffFileEn = await PDF.GeneratePdfFromUrlAsync(urlEn, PDFServiceUrl, pdfOption, Token);

                        filesAr.Add((pdffFileAr, ds.TextAr.Replace(" ", "_") + ".pdf"));
                        filesEn.Add((pdffFileEn, ds.TextEn.Replace(" ", "_") + ".pdf"));

                    }

                    var compressAr = Files.CompressToZip(filesAr);
                    var compressEn = Files.CompressToZip(filesEn);

                    foreach (var email in RequestedData.Emails)
                    {
                        if (compressAr.Length < 7000000)
                        {
                            await Email.SendEmailAsync(email, RequestedData.Subject, RequestedData.Body, EmailServiceUrl, Token, null, new List<EmailAttachment> { new EmailAttachment { FileName = RequestedData.FileName + "_Ar.zip", FileBytes = compressAr }, new EmailAttachment { FileName = RequestedData.FileName + "_En.zip", FileBytes = compressEn } });
                            Thread.Sleep(500);
                        }
                        else
                        {
                            await Email.SendEmailAsync(email, RequestedData.Subject, RequestedData.Body, EmailServiceUrl, Token, null, new List<EmailAttachment> { new EmailAttachment { FileName = RequestedData.FileName + "_Ar.zip", FileBytes = compressAr } });
                            Thread.Sleep(500);

                            await Email.SendEmailAsync(email, RequestedData.Subject, RequestedData.Body, EmailServiceUrl, Token, null, new List<EmailAttachment> { new EmailAttachment { FileName = RequestedData.FileName + "_En.zip", FileBytes = compressEn } });
                            Thread.Sleep(500);
                        }
                    }
                }
                else
                {
                    var data = await GetSurveyAnswersStatisticsReport(RequestedData);
                    _unitOfWork.MemoryCache.Set(RequestedData.SurveyId, data, DateTimeOffset.Now.AddSeconds(30));

                    var urlAr = HtmlSurveyReportUrl + "/ar/" + RequestedData.SurveyId + "?token=" + Token.Replace("bearer ", "");
                    var urlEn = HtmlSurveyReportUrl + "/en/" + RequestedData.SurveyId + "?token=" + Token.Replace("bearer ", "");

                    var pdfOption = new PdfOptions
                    {
                        Format = PaperFormat.Tabloid,
                        PrintBackground = true,
                        Landscape = true
                    };
                    var pdffFileAr = await PDF.GeneratePdfFromUrlAsync(urlAr, PDFServiceUrl, pdfOption, Token);
                    var pdffFileEn = await PDF.GeneratePdfFromUrlAsync(urlEn, PDFServiceUrl, pdfOption, Token);

                    foreach (var email in RequestedData.Emails)
                    {
                        await Email.SendEmailAsync(email, RequestedData.Subject, RequestedData.Body, EmailServiceUrl, Token, null, new List<EmailAttachment> { new EmailAttachment { FileName = RequestedData.FileName + "_Ar.pdf", FileBytes = pdffFileAr }, new EmailAttachment { FileName = RequestedData.FileName + "_En.pdf", FileBytes = pdffFileEn } });
                        Thread.Sleep(500);
                    }
                }
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
                return Result;
            }
            else
            {
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionErorr);
                return Result;
            }
        }

        public async Task<OperationOutput> GetSurveyResultLookUps()
        {
            OperationOutput Result = new OperationOutput();

            var ResultLookUps = await _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.SurveyResult).ToListAsync();

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            Result.Output = new System.Collections.Generic.Dictionary<string, object>()
            {
                { OperationOutput.OperationOutputKeys.ResultLookUps,ResultLookUps.Adapt<List<MajorLookups>>()},
            };
            return Result;

        }

        public async Task<OperationOutput> UpdateSurveyResultLookUps(List<MajorLookups> RequestedData)
        {
            var ResultLookUps = await _unitOfWork.MajorLookups.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.SurveyResult).ToListAsync();

            foreach (var lookup in ResultLookUps)
            {
                RequestedData.FirstOrDefault(x => x.Id == lookup.Id).Adapt(lookup);
                _unitOfWork.MajorLookups.Update(lookup);
            }

            _unitOfWork.Complete();
            return await GetSurveyResultLookUps();
        }

        #endregion


    }

}
