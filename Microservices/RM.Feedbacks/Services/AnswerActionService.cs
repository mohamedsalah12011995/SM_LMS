using Mapster;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Feedbacks.Dtos;
using RM.Feedbacks.UnitOfWorks;
using System.Text.Json;
using static RM.Feedbacks.Dtos.OperationOutput;
using Microsoft.EntityFrameworkCore;
using RM.Core.CommonDtos;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using RM.Core.Integrations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.IO.Compression;
using Serilog.Sinks.File;
using System.Linq;

namespace RM.Feedbacks.Services
{

    public class AnswerActionService : BaseService, IAnswerActionService
    {
        JsonSerializerOptions jsonOptions = null;
        private readonly IUnitOfWork _unitOfWork;
        private static string imagesGetPath = string.Empty;
        private static string filesGetPath = string.Empty;
        private static string HtmlFeedbackReportUrl = string.Empty;

        public AnswerActionService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            try { HtmlFeedbackReportUrl = _configuration.GetSection("AppSettings").GetSection("HtmlFeedbackReportUrl").Value; } catch { }

        }

        public async Task<OperationOutput> SaveAnswerAction(FeedbacksAnswerAction RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var NewItem = RequestedData.Adapt(new RM.Models.FeedbacksAnswerAction(), RequestedData.AddConfig());
            await _unitOfWork.FeedbacksAnswerActions.AddAsync(NewItem);

            await _unitOfWork.CompleteAsync();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> GetFeedbacksEntityItems(Dtos.Feedbacks RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<EntityItems> EntitiesItems = await GetEntities(RequestedData.Id.Value);

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKey.EntityItems, EntitiesItems));
        }

        private async Task<List<EntityItems>> GetEntities(int FeedbackId)
        {
            List<EntityItems> EntitiesItems = new List<EntityItems>();

            var Entitys = await _unitOfWork.FeedbacksAnswerActions.GetAll().Include(x => x.Entity)
                                           .Where(x => x.FeedbacksId == FeedbackId)
                                           .AsNoTracking().ToListAsync();

            var Entities = Entitys.GroupBy(x => new { x.EntityId }, (key, group) => new EntityItems
            {
                Id = key.EntityId,
                NameAr = group.First().Entity.NameAr,
                NameEn = group.First().Entity.NameEn,
                Items = group.Select(x => new EntityItems { Id = x.ItemId }).ToList()
            });

            foreach (var entityItem in Entities)
                EntitiesItems.Add(getItemDetails(entityItem));
            return EntitiesItems;
        }

        private async Task<EntityItems> GetEntityItems(int EntityId,int FeedbackId)
        {
            var Entitys = await _unitOfWork.FeedbacksAnswerActions.GetAll().Include(x => x.Entity)
                                           .Where(x => x.FeedbacksId == FeedbackId && x.EntityId == EntityId)
                                           .AsNoTracking().ToListAsync();

            var entityItem = Entitys.GroupBy(x => new { x.EntityId }, (key, group) => new EntityItems
            {
                Id = key.EntityId,
                NameAr = group.First().Entity.NameAr,
                NameEn = group.First().Entity.NameEn,
                Items = group.Select(x => new EntityItems { Id = x.ItemId }).ToList()
            }).FirstOrDefault();

            return getItemDetails(entityItem);
        }

        private EntityItems getItemDetails(EntityItems item)
        {
            var itemsIds = item.Items.Select(x => x.Id).ToList();
            if (itemsIds.Any())
            {
                if (item.Id == (int)Enums.Entities.Advertisments)
                    item.Items = _unitOfWork.Advertisements.GetAll().Where(x => itemsIds.Contains(x.Id) && x.IsDeleted != true)
                                  .Select(x => new EntityItems { Id = x.Id, NameAr = x.TitleAr, NameEn = x.TitleEn }).AsNoTracking().ToList();
                else if (item.Id == (int)Enums.Entities.Articles)
                    item.Items = _unitOfWork.Articles.GetAll().Where(x => itemsIds.Contains(x.Id) && x.IsDeleted != true)
                                 .Select(x => new EntityItems { Id = x.Id, NameAr = x.NameAr, NameEn = x.NameEn }).AsNoTracking().ToList();
                else if (item.Id == (int)Enums.Entities.News)
                    item.Items = _unitOfWork.News.GetAll().Where(x => itemsIds.Contains(x.Id) && x.IsDeleted != true)
                                 .Select(x => new EntityItems { Id = x.Id, NameAr = x.TitleAr, NameEn = x.TitleEn }).AsNoTracking().ToList();
                else if (item.Id == (int)Enums.Entities.EServices)
                    item.Items = _unitOfWork.Eservices.GetAll().Where(x => itemsIds.Contains(x.Id) && x.IsDeleted != true)
                                 .Select(x => new EntityItems { Id = x.Id, NameAr = x.NameAr, NameEn = x.NameEn }).AsNoTracking().ToList();
                else if (item.Id == (int)Enums.Entities.GovServices)
                    item.Items = _unitOfWork.GovServices.GetAll().Where(x => itemsIds.Contains(x.Id) && x.IsDeleted != true)
                                 .Select(x => new EntityItems { Id = x.Id, NameAr = x.NameAr, NameEn = x.NameEn }).AsNoTracking().ToList();

                else if (item.Id == (int)Enums.Entities.ScientificLetters)
                    item.Items = _unitOfWork.ScientificLetters.GetAll().Where(x => itemsIds.Contains(x.Id) && x.IsDeleted != true)
                                 .Select(x => new EntityItems { Id = x.Id, NameAr = x.TitleAr, NameEn = x.TitleEn }).AsNoTracking().ToList();

                else if (item.Id == (int)Enums.Entities.Partners)
                    item.Items = _unitOfWork.Partners.GetAll().Where(x => itemsIds.Contains(x.Id) && x.IsDeleted != true)
                                 .Select(x => new EntityItems { Id = x.Id, NameAr = x.TitleAr, NameEn = x.TitleEn }).AsNoTracking().ToList();

                else if (item.Id == (int)Enums.Entities.RmOfficials)
                    item.Items = _unitOfWork.Officials.GetAll().Where(x => itemsIds.Contains(x.Id) && x.IsDeleted != true)
                                 .Select(x => new EntityItems { Id = x.Id, NameAr = x.NameAr, NameEn = x.NameEn }).AsNoTracking().ToList();

                else if (item.Id == (int)Enums.Entities.ImageGallery || item.Id == (int)Enums.Entities.VideoGallery)
                    item.Items = _unitOfWork.Multimedia.GetAll().Where(x => itemsIds.Contains(x.Id) && x.IsDeleted != true)
                                 .Select(x => new EntityItems { Id = x.Id, NameAr = x.NameAr, NameEn = x.NameEn }).AsNoTracking().ToList();

                else if (item.Id == (int)Enums.Entities.JobAdvertisement)
                    item.Items = _unitOfWork.JobAdvertisement.GetAll().Where(x => itemsIds.Contains(x.Id) && x.IsDeleted != true)
                                 .Select(x => new EntityItems { Id = x.Id, NameAr = x.TitleAr, NameEn = x.TitleEn }).AsNoTracking().ToList();

                else if (item.Id == (int)Enums.Entities.JobCareer)
                    item.Items = _unitOfWork.JobCareers.GetAll().Where(x => itemsIds.Contains(x.Id) && x.IsDeleted != true)
                                 .Select(x => new EntityItems { Id = x.Id, NameAr = x.TitleAr, NameEn = x.TitleEn }).AsNoTracking().ToList();

                else if (item.Id == (int)Enums.Entities.FAQ)
                    item.Items = _unitOfWork.FAQs.GetAll().Where(x => itemsIds.Contains(x.Id) && x.IsDeleted != true)
                                 .Select(x => new EntityItems { Id = x.Id, NameAr = x.AnswerAr, NameEn = x.AnswerEn }).AsNoTracking().ToList();

                else if (item.Id == (int)Enums.Entities.Magazine)
                    item.Items = _unitOfWork.Documents.GetAll().Where(x => itemsIds.Contains(x.Id) && x.IsDeleted != true)
                                 .Select(x => new EntityItems { Id = x.Id, NameAr = x.NameAr, NameEn = x.NameEn }).AsNoTracking().ToList();

                else item.Items = [];
            }
            return item;
        }

        public async Task<OperationOutput> GetFeedbacksAnswersStatistics(Dtos.FeedbacksAnswerAction RequestedData)
        {
            FeedbacksStatisticsResult YearsStatistics=null;
            int HelpfulCount=0, NotHelpfulCount=0, TotalRate = 0;

            EntityItems Entity = await GetEntityItems(RequestedData.EntityId.Value, RequestedData.FeedbacksId.Value);

            var result = await _unitOfWork.FeedbacksAnswerActions.GetAll().Include(x => x.Feedbacks)
                .Include(x => x.FeedbacksAnswers).ThenInclude(x => x.FeedbacksDataSource)
                .Where(x => x.FeedbacksId == RequestedData.FeedbacksId && x.EntityId == RequestedData.EntityId)
                .Where(x => RequestedData.ItemId.HasValue ? x.ItemId == RequestedData.ItemId : true)
                .Where(x => RequestedData.Year.HasValue ? x.CreatedDate.Value.Year == RequestedData.Year : true)
                .Where(x => RequestedData.Quarter.HasValue ?
                  RequestedData.Quarter == 1 ? x.CreatedDate.Value.Month <= 3
                : RequestedData.Quarter == 2 ? x.CreatedDate.Value.Month >= 4 && x.CreatedDate.Value.Month <= 6
                : RequestedData.Quarter == 3 ? x.CreatedDate.Value.Month >= 7 && x.CreatedDate.Value.Month <= 9
                : RequestedData.Quarter == 4 ? x.CreatedDate.Value.Month >= 10 : true : true)
                .ToListAsync();

            var Recomendations = _unitOfWork.Recommendations.GetAll().Where(x => x.EntityId == (int)Enums.Entities.Feedbacks).AsNoTracking().ToList();
            var dataSources = _unitOfWork.FeedbacksDataSources.GetAll().Where(x => x.FeedbacksId == RequestedData.FeedbacksId && x.IsDeleted != true).AsNoTracking().ToList();

            if (result.Any())
            {
                YearsStatistics = new FeedbacksStatisticsResult
                {
                    FeedbacksId = result.FirstOrDefault().Feedbacks.Id,
                    TitleAr = result.FirstOrDefault().Feedbacks.TitleAr,
                    TitleEn = result.FirstOrDefault().Feedbacks.TitleEn,
                    ThisDay = result.Count(x => x.CreatedDate.Value.Day == TransactionDate.Day),
                    ThisWeek = result.Count(x => x.CreatedDate.Value.Year == TransactionDate.Year && x.CreatedDate.Value.Month == TransactionDate.Month && x.CreatedDate.Value.Day >= StartWeekDate.Day && x.CreatedDate.Value.Day <= EndWeekDate.Day),
                    ThisMonth = result.Count(x => x.CreatedDate.Value.Month == TransactionDate.Month),
                    ThisYear = result.Count(x => x.CreatedDate.Value.Year == TransactionDate.Year),
                    Total = result.Count,
                    Years = result.GroupBy(x => new { x.CreatedDate.Value.Year }, (key, group) =>
                           new YearResult
                           {
                               Year = key.Year,
                               YearRate = group.Count() > 0 ? group.Count(x => x.IsHelpful == true) * 10 / group.Count() : 0,
                               YearCount = group.Count(),
                               Quarters = new List<QuarterResult>
                                   {
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(dataSources,Recomendations,1,group.Where(x=>x.CreatedDate.Value.Month <= 3).ToList(),group.Count(),1,group.Where(x=>x.CreatedDate.Value.Month == 1).ToList(),2,group.Where(x=>x.CreatedDate.Value.Month == 2).ToList(),3,group.Where(x=>x.CreatedDate.Value.Month == 3).ToList())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(dataSources,Recomendations,2,group.Where(x=>x.CreatedDate.Value.Month >= 4 && x.CreatedDate.Value.Month <=6).ToList(),group.Count(),4,group.Where(x=>x.CreatedDate.Value.Month == 4).ToList(),5,group.Where(x=>x.CreatedDate.Value.Month == 5).ToList(),6,group.Where(x=>x.CreatedDate.Value.Month == 6).ToList())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(dataSources,Recomendations,3,group.Where(x=>x.CreatedDate.Value.Month >= 7 && x.CreatedDate.Value.Month <=9).ToList(),group.Count(),7,group.Where(x=>x.CreatedDate.Value.Month == 7).ToList(),8,group.Where(x=>x.CreatedDate.Value.Month == 8).ToList(),9,group.Where(x=>x.CreatedDate.Value.Month == 9).ToList())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(dataSources,Recomendations,4,group.Where(x=>x.CreatedDate.Value.Month >= 10).ToList(),group.Count(),10,group.Where(x=>x.CreatedDate.Value.Month == 10).ToList(),11,group.Where(x=>x.CreatedDate.Value.Month == 11).ToList(),12,group.Where(x=>x.CreatedDate.Value.Month == 12).ToList()))
                                   }
                           }).ToList()
                };

                HelpfulCount = result.Count(x => x.IsHelpful == true);
                NotHelpfulCount = result.Count - HelpfulCount;
                TotalRate = result.Count > 0 ? HelpfulCount * 10 / result.Count : 0;
            }
            else {
                YearsStatistics = new FeedbacksStatisticsResult
                {
                    FeedbacksId = RequestedData.FeedbacksId,
                    TitleAr = "",
                    TitleEn = "",
                    ThisDay = 0,
                    ThisWeek = 0,
                    ThisMonth = 0,
                    ThisYear = 0,
                    Total = 0,
                    Years = new List<YearResult> {
                           new YearResult
                           {
                               Year = RequestedData.Year.Value,
                               YearRate = 0,
                               YearCount = 0,
                               Quarters = new List<QuarterResult>
                                   {
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(dataSources,Recomendations,1,new List<Models.FeedbacksAnswerAction>(),0,1,new List<Models.FeedbacksAnswerAction>(),2,new List<Models.FeedbacksAnswerAction>(),3,new List<Models.FeedbacksAnswerAction>())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(dataSources,Recomendations,2,new List<Models.FeedbacksAnswerAction>(),0,4,new List<Models.FeedbacksAnswerAction>(),5,new List<Models.FeedbacksAnswerAction>(),6,new List<Models.FeedbacksAnswerAction>())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(dataSources,Recomendations,3,new List<Models.FeedbacksAnswerAction>(),0,7,new List<Models.FeedbacksAnswerAction>(),8,new List<Models.FeedbacksAnswerAction>(),9,new List<Models.FeedbacksAnswerAction>())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(dataSources,Recomendations,4,new List<Models.FeedbacksAnswerAction>(),0,10,new List<Models.FeedbacksAnswerAction>(),11,new List<Models.FeedbacksAnswerAction>(),12,new List<Models.FeedbacksAnswerAction>()))
                                   }
                           }
                    }
                };
            }


            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKey.TotalCount, result.Count),
                     new OutputDictionary(OperationOutputKey.TotalRate, TotalRate),
                     new OutputDictionary(OperationOutputKey.HelpfulCount, HelpfulCount),
                     new OutputDictionary(OperationOutputKey.NotHelpfulCount, NotHelpfulCount),
                     new OutputDictionary(OperationOutputKey.YearsStatistics, YearsStatistics),
                     new OutputDictionary("EntityObj", Entity),
                     new OutputDictionary("ItemObj", Entity.Items.FirstOrDefault(x=>x.Id == RequestedData.ItemId)));
        }



        #region SendEmail

        public async Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord cron)
        {
            OperationOutput Result = new OperationOutput();

            var Feedbacks = _unitOfWork.Feedbacks.GetAll().Where(x => x.IsDeleted != true).AsNoTracking().ToList();
            var CronSettings = _unitOfWork.CronSettings.GetAll().Where(x => x.IsActive == true && x.CronTypeId == cron.CronTypeId && x.EntityId == (int)Enums.Entities.Feedbacks).AsNoTracking().ToList();

            foreach (var feedback in Feedbacks) 
            {
                var EntitiesItems = await GetEntities(feedback.Id);

                foreach (var entity in EntitiesItems)
                {
                    var emails = CronSettings.Where(x => x.SubEntityId == null || x.SubEntityId == entity.Id && x.IsActive == true).SelectMany(x => Strings.ConvertStringToList(x.Emails, "$")).Distinct().ToList();
                    if (emails.Any())
                    {
                        if (entity.Items.Any())
                        {
                            foreach (var item in entity.Items)
                            {
                                await SendEmailFeedbacksStatistics(new FeedbacksAnswerAction() { FeedbacksId = feedback.Id, EntityId = entity.Id, ItemId = item.Id, Emails = emails, Subject = entity.NameAr + " - " + item.NameAr, FileName = item.NameEn });
                            }
                        }
                        else
                            await SendEmailFeedbacksStatistics(new FeedbacksAnswerAction() { FeedbacksId = feedback.Id, EntityId = entity.Id, Emails = emails, Subject = entity.NameAr, FileName = entity.NameEn });
                    }
                }
            }

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }

        public async Task<OperationOutput> GetFeedbacksAnswersStatisticsReport(Dtos.FeedbacksAnswerAction RequestedData)
        {
            OperationOutput Result = null;
            if (RequestedData.ItemId.HasValue)
                Result = _unitOfWork.MemoryCache.Get<OperationOutput>(RequestedData.feedbacksId + RequestedData.entityId + RequestedData.itemId);
            else
                Result = _unitOfWork.MemoryCache.Get<OperationOutput>(RequestedData.feedbacksId + RequestedData.entityId);
            if (Result != null)
                return Result;

          return  await GetFeedbacksAnswersStatistics(RequestedData);
        }
        public async Task<OperationOutput> SendEmailFeedbacksStatistics(Dtos.FeedbacksAnswerAction RequestedData)
        {
            OperationOutput Result = new OperationOutput();

            var feedback = _unitOfWork.Feedbacks.GetAll().Where(x => x.Id == RequestedData.FeedbacksId && x.IsDeleted != true).AsNoTracking().FirstOrDefault();

            if (feedback != null)
            {
                EntityItems Entity = await GetEntityItems(RequestedData.EntityId.Value, RequestedData.FeedbacksId.Value);

                if (Entity != null && Entity.Items.Any())
                {
                    List<(byte[], string)> filesAr = new List<(byte[], string)>();
                    List<(byte[], string)> filesEn = new List<(byte[], string)>();

                    foreach (var item in Entity.Items.Where(x => RequestedData.ItemId.HasValue ? x.Id == RequestedData.ItemId : true))
                    {
                        var data = await GetFeedbacksAnswersStatistics(RequestedData);

                        _unitOfWork.MemoryCache.Set(RequestedData.feedbacksId + RequestedData.entityId + item.ID, data, DateTimeOffset.Now.AddSeconds(30));

                        var urlAr = HtmlFeedbackReportUrl + "/ar/" + RequestedData.feedbacksId + "?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&itemId=" + item.ID + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
                        var urlEn = HtmlFeedbackReportUrl + "/en/" + RequestedData.feedbacksId + "?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&itemId=" + item.ID + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

                        var pdfOption = new PdfOptions
                        {
                            Format = PaperFormat.Tabloid,
                            PrintBackground = true,
                            Landscape = true
                        };
                        var pdffFileAr = await PDF.GeneratePdfFromUrlAsync(urlAr, PDFServiceUrl, pdfOption, Token);
                        var pdffFileEn = await PDF.GeneratePdfFromUrlAsync(urlEn, PDFServiceUrl, pdfOption, Token);

                        filesAr.Add((pdffFileAr, item.NameAr.Replace(" ", "_") + ".pdf"));
                        filesEn.Add((pdffFileEn, item.NameEn.Replace(" ", "_") + ".pdf"));

                    }

                    if (!RequestedData.ItemId.HasValue)
                    {
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
                        foreach (var email in RequestedData.Emails)
                        {
                                await Email.SendEmailAsync(email, RequestedData.Subject, RequestedData.Body, EmailServiceUrl, Token, null, new List<EmailAttachment> { new EmailAttachment { FileName = filesAr[0].Item2, FileBytes = filesAr[0].Item1 }, new EmailAttachment { FileName = filesEn[0].Item2, FileBytes = filesEn[0].Item1 } });
                                Thread.Sleep(500);
                        }
                    }
                }
                else
                {
                    var data = await GetFeedbacksAnswersStatistics(RequestedData);
                    _unitOfWork.MemoryCache.Set(RequestedData.feedbacksId + RequestedData.entityId, data, DateTimeOffset.Now.AddSeconds(30));

                    var urlAr = HtmlFeedbackReportUrl + "/ar/" + RequestedData.feedbacksId + "?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
                    var urlEn = HtmlFeedbackReportUrl + "/en/" + RequestedData.feedbacksId + "?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

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
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.RequiredFiled);
                return Result;
            }
        }

        public async Task<FileStreamResult> ExportFeedbacksStatistics(Dtos.FeedbacksAnswerAction RequestedData)
        {
            MemoryStream stream;
            List<(byte[], string)> files = new List<(byte[], string)>();

            var feedback = _unitOfWork.Feedbacks.GetAll().Where(x => x.Id == RequestedData.FeedbacksId && x.IsDeleted != true).AsNoTracking().FirstOrDefault();

            if (feedback != null)
            {
                EntityItems Entity = await GetEntityItems(RequestedData.EntityId.Value, RequestedData.FeedbacksId.Value);

                if (Entity != null && Entity.Items.Any())
                {
                    List<(byte[], string)> filesAr = new List<(byte[], string)>();
                    List<(byte[], string)> filesEn = new List<(byte[], string)>();

                    foreach (var item in Entity.Items.Where(x => RequestedData.ItemId.HasValue ? x.Id == RequestedData.ItemId : true))
                    {
                        var data = await GetFeedbacksAnswersStatistics(RequestedData);

                        _unitOfWork.MemoryCache.Set(RequestedData.feedbacksId + RequestedData.entityId + item.ID, data, DateTimeOffset.Now.AddSeconds(30));

                        var urlAr = HtmlFeedbackReportUrl + "/ar/" + RequestedData.feedbacksId + "?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&itemId=" + item.ID + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
                        var urlEn = HtmlFeedbackReportUrl + "/en/" + RequestedData.feedbacksId + "?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&itemId=" + item.ID + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

                        var pdfOption = new PdfOptions
                        {
                            Format = PaperFormat.Tabloid,
                            PrintBackground = true,
                            Landscape = true
                        };
                        var pdffFileAr = await PDF.GeneratePdfFromUrlAsync(urlAr, PDFServiceUrl, pdfOption, Token);
                        var pdffFileEn = await PDF.GeneratePdfFromUrlAsync(urlEn, PDFServiceUrl, pdfOption, Token);

                        if (RequestedData.ItemId.HasValue)
                        {
                            files.Add((pdffFileAr, item.NameAr.Replace(" ", "_") + ".pdf"));
                            files.Add((pdffFileEn, item.NameEn.Replace(" ", "_") + ".pdf"));
                        }
                        else
                        { 
                            filesAr.Add((pdffFileAr, item.NameAr.Replace(" ", "_") + ".pdf"));
                            filesEn.Add((pdffFileEn, item.NameEn.Replace(" ", "_") + ".pdf"));
                        }
                    }

                    if (!RequestedData.ItemId.HasValue)
                    {
                        var compressAr = Files.CompressToZip(filesAr);
                        var compressEn = Files.CompressToZip(filesEn);
                        files.Add((compressAr, Entity.NameAr.Replace("_", "") + ".zip"));
                        files.Add((compressEn, Entity.NameEn.Replace("_", "") + ".zip"));
                    }
                }
                else
                {
                    var data = await GetFeedbacksAnswersStatistics(RequestedData);
                    _unitOfWork.MemoryCache.Set(RequestedData.feedbacksId + RequestedData.entityId, data, DateTimeOffset.Now.AddSeconds(30));

                    var urlAr = HtmlFeedbackReportUrl + "/ar/" + RequestedData.feedbacksId + "?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
                    var urlEn = HtmlFeedbackReportUrl + "/en/" + RequestedData.feedbacksId + "?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

                    var pdfOption = new PdfOptions
                    {
                        Format = PaperFormat.Tabloid,
                        PrintBackground = true,
                        Landscape = true
                    };
                    var pdffFileAr = await PDF.GeneratePdfFromUrlAsync(urlAr, PDFServiceUrl, pdfOption, Token);
                    var pdffFileEn = await PDF.GeneratePdfFromUrlAsync(urlEn, PDFServiceUrl, pdfOption, Token);

                    files.Add((pdffFileAr, Entity.NameAr.Replace(" ", "_") + ".pdf"));
                    files.Add((pdffFileEn, Entity.NameEn.Replace(" ", "_") + ".pdf"));
                }

                stream = new MemoryStream(Files.CompressToZip(files));
                stream.Seek(0, SeekOrigin.Begin);

                return new FileStreamResult(stream, "application/zip")
                {
                    FileDownloadName = Entity.NameAr.Replace(" ", "_"),
                };
            }
            else
            {
                return null;
            }
        }

        #endregion


    }
}
