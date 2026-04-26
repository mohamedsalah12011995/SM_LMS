using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp.Media;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Statistics.Dtos;
using RM.Statistics.UnitOfWorks;
using System.Data;
using Microsoft.Extensions.Caching.Memory;
using PuppeteerSharp;
using RM.Core.Integrations;
using RM.Models;
using static RM.Statistics.Dtos.OperationOutput;

namespace RM.Statistics.Services
{
    public class StatisticResultService : BaseService , IStatisticResultService
    {
        private readonly IUnitOfWork _unitOfWork;
        private static string HtmlInteractionStatisticReportUrl = string.Empty;

        public StatisticResultService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            HtmlInteractionStatisticReportUrl = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("HtmlInteractionStatisticReportUrl").Value;

        }

        public async Task<OperationOutput> GetLookUps()
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<EntityItems> EntitiesItems = await GetEntities();
            var InteractionStatisticsTypes = _unitOfWork.InteractionStatisticsTypes.GetAll().AsNoTracking().ToList();
            var GregorianYears = Enumerable.Range(2023, DateTime.Now.Year - 2022).ToList();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.EntityItems, EntitiesItems),
               new OutputDictionary(OperationOutputKeys.YearsList, GregorianYears),
               new OutputDictionary(OperationOutputKeys.InteractionStatisticsTypes, InteractionStatisticsTypes));
        }

        private async Task<List<EntityItems>> GetEntities()
        {
            List<EntityItems> EntitiesItems = new List<EntityItems>();

            var Entitys = await _unitOfWork.InteractionStatistic.GetAll().Include(x => x.Entity).AsNoTracking().ToListAsync();

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

        private async Task<EntityItems> GetEntityItems(int EntityId)
        {
            var Entitys = await _unitOfWork.InteractionStatistic.GetAll().Include(x => x.Entity)
                                           .Where(x => x.EntityId == EntityId)
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

        public async Task<OperationOutput> GetInteractionStatistics(Dtos.Statistics RequestedData)
        {
            InteractionStatisticsResult YearsStatistics = null;
            int TotalCount = 0;

            EntityItems Entity = await GetEntityItems(RequestedData.EntityId.Value);

            (TotalCount, YearsStatistics) = await GetInteractionStatisticsResult(RequestedData);

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.TotalCount, TotalCount),
                     new OutputDictionary(OperationOutputKeys.YearsStatistics, YearsStatistics),
                     new OutputDictionary("EntityObj", Entity),
                     new OutputDictionary("ItemObj", Entity.Items.FirstOrDefault(x => x.Id == RequestedData.ItemId)));
        }


        private async Task<(int TotalCount, InteractionStatisticsResult YearsStatistics)> GetInteractionStatisticsResult(Dtos.Statistics RequestedData)
        {
            InteractionStatisticsResult YearsStatistics = null;

            EntityItems Entity = await GetEntityItems(RequestedData.EntityId.Value);
            int TotalCount = 0; int TotalAllCount = 0; bool isAvg = RequestedData.StatisticsType == (int)Enums.InteractionStatisticsType.TimeSpend;
            var result = await _unitOfWork.InteractionStatistic.GetAll().Where(x => x.CreatedDate.HasValue && x.Value.HasValue)
                .Where(x => x.InteractionStatisticsType == RequestedData.StatisticsType && x.EntityId == RequestedData.EntityId)
                .Where(x => RequestedData.ItemId.HasValue ? x.ItemId == RequestedData.ItemId : true)
                .Where(x => RequestedData.Year.HasValue ? x.CreatedDate.Value.Year == RequestedData.Year : true)
                .Where(x => RequestedData.Quarter.HasValue ?
                  RequestedData.Quarter == 1 ? x.CreatedDate.Value.Month <= 3
                : RequestedData.Quarter == 2 ? x.CreatedDate.Value.Month >= 4 && x.CreatedDate.Value.Month <= 6
                : RequestedData.Quarter == 3 ? x.CreatedDate.Value.Month >= 7 && x.CreatedDate.Value.Month <= 9
                : RequestedData.Quarter == 4 ? x.CreatedDate.Value.Month >= 10 : true : true)
                .ToListAsync();

            var Total = _unitOfWork.InteractionStatistic.GetAll()
                        .Where(x => x.CreatedDate.HasValue && x.Value.HasValue)
                        .Where(x => x.InteractionStatisticsType == RequestedData.StatisticsType && x.EntityId == RequestedData.EntityId)
                        .Where(x => RequestedData.ItemId.HasValue ? x.ItemId == RequestedData.ItemId : true);

            TotalCount = result.Sum(x => x.Value.Value);
            TotalAllCount = Total.Sum(x => x.Value.Value);
            if (isAvg)
            {
                TotalCount = result.Count() > 0 ? TotalCount / result.Count() : TotalCount;
                TotalAllCount = Total.Count() > 0 ? TotalAllCount / Total.Count() : TotalAllCount;
            }

            if (result.Any())
            {
                YearsStatistics = new InteractionStatisticsResult
                {
                    ThisDay = isAvg ? result.Count() > 0 ? result.Where(x => x.CreatedDate.Value.Day == TransactionDate.Day).Sum(x => x.Value) / result.Count() : 0 : result.Where(x => x.CreatedDate.Value.Day == TransactionDate.Day).Sum(x => x.Value),
                    ThisWeek = isAvg ? result.Count() > 0 ? result.Where(x => x.CreatedDate.Value.Year == TransactionDate.Year && x.CreatedDate.Value.Month == TransactionDate.Month && x.CreatedDate.Value.Day >= StartWeekDate.Day && x.CreatedDate.Value.Day <= EndWeekDate.Day).Sum(x => x.Value) / result.Count() : 0 : result.Where(x => x.CreatedDate.Value.Year == TransactionDate.Year && x.CreatedDate.Value.Month == TransactionDate.Month && x.CreatedDate.Value.Day >= StartWeekDate.Day && x.CreatedDate.Value.Day <= EndWeekDate.Day).Sum(x => x.Value),
                    ThisMonth = isAvg ? result.Count() > 0 ? result.Where(x => x.CreatedDate.Value.Month == TransactionDate.Month).Sum(x => x.Value) / result.Count() : 0 : result.Where(x => x.CreatedDate.Value.Month == TransactionDate.Month).Sum(x => x.Value),
                    ThisYear = isAvg ? result.Count() > 0 ? result.Where(x => x.CreatedDate.Value.Year == TransactionDate.Year).Sum(x => x.Value) / result.Count() : 0 : result.Where(x => x.CreatedDate.Value.Year == TransactionDate.Year).Sum(x => x.Value),
                    Total = TotalAllCount,
                    Years = result.GroupBy(x => new { x.CreatedDate.Value.Year }, (key, group) =>
                           new YearResult
                           {
                               Year = key.Year,
                               YearRate = 0,
                               YearCount = isAvg ? group.Count() > 0 ? group.Sum(x => x.Value.Value) / group.Count() : 0 : group.Sum(x => x.Value.Value),
                               Quarters = new List<QuarterResult>
                                   {
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(isAvg,1,group.Where(x=>x.CreatedDate.Value.Month <= 3).ToList(),group.Sum(x=>x.Value.Value),1,group.Where(x=>x.CreatedDate.Value.Month == 1).ToList(),2,group.Where(x=>x.CreatedDate.Value.Month == 2).ToList(),3,group.Where(x=>x.CreatedDate.Value.Month == 3).ToList())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(isAvg,2,group.Where(x=>x.CreatedDate.Value.Month >= 4 && x.CreatedDate.Value.Month <=6).ToList(),group.Sum(x=>x.Value.Value),4,group.Where(x=>x.CreatedDate.Value.Month == 4).ToList(),5,group.Where(x=>x.CreatedDate.Value.Month == 5).ToList(),6,group.Where(x=>x.CreatedDate.Value.Month == 6).ToList())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(isAvg,3,group.Where(x=>x.CreatedDate.Value.Month >= 7 && x.CreatedDate.Value.Month <=9).ToList(),group.Sum(x=>x.Value.Value),7,group.Where(x=>x.CreatedDate.Value.Month == 7).ToList(),8,group.Where(x=>x.CreatedDate.Value.Month == 8).ToList(),9,group.Where(x=>x.CreatedDate.Value.Month == 9).ToList())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(isAvg,4,group.Where(x=>x.CreatedDate.Value.Month >= 10).ToList(),group.Sum(x=>x.Value.Value),10,group.Where(x=>x.CreatedDate.Value.Month == 10).ToList(),11,group.Where(x=>x.CreatedDate.Value.Month == 11).ToList(),12,group.Where(x=>x.CreatedDate.Value.Month == 12).ToList()))
                                   }
                           }).ToList()
                };
            }
            else
            {
                YearsStatistics = new InteractionStatisticsResult
                {
                    ThisDay = 0,
                    ThisWeek = 0,
                    ThisMonth = 0,
                    ThisYear = 0,
                    Total = 0,
                    Years = new List<YearResult> {
                           new YearResult
                           {
                               Year = RequestedData.Year ?? TransactionDate.Year,
                               YearRate = 0,
                               YearCount = 0,
                               Quarters = new List<QuarterResult>
                                   {
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(false,1,new List<Models.InteractionStatistic>(),0,1,new List<Models.InteractionStatistic>(),2,new List<Models.InteractionStatistic>(),3,new List<Models.InteractionStatistic>())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(false, 2,new List<Models.InteractionStatistic>(),0,4,new List<Models.InteractionStatistic>(),5,new List<Models.InteractionStatistic>(),6,new List<Models.InteractionStatistic>())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(false,3,new List<Models.InteractionStatistic>(),0,7,new List<Models.InteractionStatistic>(),8,new List<Models.InteractionStatistic>(),9,new List<Models.InteractionStatistic>())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(false,4,new List<Models.InteractionStatistic>(),0,10,new List<Models.InteractionStatistic>(),11,new List<Models.InteractionStatistic>(),12,new List<Models.InteractionStatistic>()))
                                   }
                           }
                    }
                };
            }

            return (TotalCount, YearsStatistics);
        }


        #region SendEmail

        public async Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord cron)
        {
            OperationOutput Result = new OperationOutput();

            var CronSettings = _unitOfWork.CronSettings.GetAll().Where(x => x.IsActive == true && x.CronTypeId == cron.CronTypeId && x.EntityId == (int)Enums.Entities.InteractionStatistics).AsNoTracking().ToList();

            var EntitiesItems = await GetEntities();

            foreach (var entity in EntitiesItems)
            {
                var emails = CronSettings.Where(x => x.SubEntityId == null || x.SubEntityId == entity.Id && x.IsActive == true).SelectMany(x => Strings.ConvertStringToList(x.Emails, "$")).Distinct().ToList();

                if (emails.Any())
                {
                    if (entity.Items.Any())
                    {
                        foreach (var item in entity.Items)
                        {
                            await SendEmailInteractionStatistics(new Dtos.Statistics() { EntityId = entity.Id, ItemId = item.Id, Emails = emails, Subject = entity.NameAr + " - " + item.NameAr, FileName = item.NameEn });
                        }
                    }
                    else
                        await SendEmailInteractionStatistics(new Dtos.Statistics() { EntityId = entity.Id, Emails = emails, Subject = entity.NameAr, FileName = entity.NameEn });
                }
            }
            

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }

        public async Task<OperationOutput> GetInteractionStatisticsReport(Dtos.Statistics RequestedData)
        {
            OperationOutput Result = null;
            if (RequestedData.ItemId.HasValue)
                Result = _unitOfWork.MemoryCache.Get<OperationOutput>("Statistics" + RequestedData.entityId + RequestedData.itemId);
            else
                Result = _unitOfWork.MemoryCache.Get<OperationOutput>("Statistics" + RequestedData.entityId);
            if (Result != null)
                return Result;

            InteractionStatisticsResult YearsStatistics1 = null, YearsStatistics2 = null, YearsStatistics3 = null;
            int TotalCount1 = 0, TotalCount2 = 0, TotalCount3 = 0;

            EntityItems Entity = await GetEntityItems(RequestedData.EntityId.Value);

            RequestedData.StatisticsType = (int)Enums.InteractionStatisticsType.ViewsCount;
            (TotalCount1, YearsStatistics1) = await GetInteractionStatisticsResult(RequestedData);
            RequestedData.StatisticsType = (int)Enums.InteractionStatisticsType.TimeSpend;
            (TotalCount2, YearsStatistics2) = await GetInteractionStatisticsResult(RequestedData);
            RequestedData.StatisticsType = (int)Enums.InteractionStatisticsType.NewVisitor;
            (TotalCount3, YearsStatistics3) = await GetInteractionStatisticsResult(RequestedData);

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary("TotalCount1", TotalCount1),
                     new OutputDictionary("TotalCount2", TotalCount2),
                     new OutputDictionary("TotalCount3", TotalCount3),
                     new OutputDictionary("YearsStatistics1", YearsStatistics1),
                     new OutputDictionary("YearsStatistics2", YearsStatistics2),
                     new OutputDictionary("YearsStatistics3", YearsStatistics3),
                     new OutputDictionary("EntityObj", Entity),
                     new OutputDictionary("ItemObj", Entity.Items.FirstOrDefault(x => x.Id == RequestedData.ItemId)));

        }
        public async Task<OperationOutput> SendEmailInteractionStatistics(Dtos.Statistics RequestedData)
        {
            OperationOutput Result = new OperationOutput();

                EntityItems Entity = await GetEntityItems(RequestedData.EntityId.Value);

                if (Entity != null && Entity.Items.Any())
                {
                    List<(byte[], string)> filesAr = new List<(byte[], string)>();
                    List<(byte[], string)> filesEn = new List<(byte[], string)>();

                    foreach (var item in Entity.Items.Where(x => RequestedData.ItemId.HasValue ? x.Id == RequestedData.ItemId : true))
                    {
                        var data = await GetInteractionStatisticsReport(RequestedData);

                        _unitOfWork.MemoryCache.Set("Statistics" + RequestedData.entityId + item.ID, data, DateTimeOffset.Now.AddSeconds(30));

                        var urlAr = HtmlInteractionStatisticReportUrl + "/ar?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&itemId=" + item.ID + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
                        var urlEn = HtmlInteractionStatisticReportUrl + "/en?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&itemId=" + item.ID + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

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
                    var data = await GetInteractionStatisticsReport(RequestedData);
                    _unitOfWork.MemoryCache.Set("Statistics" + RequestedData.entityId, data, DateTimeOffset.Now.AddSeconds(30));

                    var urlAr = HtmlInteractionStatisticReportUrl + "/ar?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
                    var urlEn = HtmlInteractionStatisticReportUrl + "/en?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

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

        public async Task<FileStreamResult> ExportInteractionStatistics(Dtos.Statistics RequestedData)
        {
            MemoryStream stream;
            List<(byte[], string)> files = new List<(byte[], string)>();

                EntityItems Entity = await GetEntityItems(RequestedData.EntityId.Value);

                if (Entity != null && Entity.Items.Any())
                {
                    List<(byte[], string)> filesAr = new List<(byte[], string)>();
                    List<(byte[], string)> filesEn = new List<(byte[], string)>();

                    foreach (var item in Entity.Items.Where(x => RequestedData.ItemId.HasValue ? x.Id == RequestedData.ItemId : true))
                    {
                        var data = await GetInteractionStatisticsReport(RequestedData);

                        _unitOfWork.MemoryCache.Set("Statistics" + RequestedData.entityId + item.ID, data, DateTimeOffset.Now.AddSeconds(30));

                        var urlAr = HtmlInteractionStatisticReportUrl + "/ar?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&itemId=" + item.ID + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
                        var urlEn = HtmlInteractionStatisticReportUrl + "/en?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&itemId=" + item.ID + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

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
                    var data = await GetInteractionStatisticsReport(RequestedData);
                    _unitOfWork.MemoryCache.Set("Statistics" + RequestedData.entityId, data, DateTimeOffset.Now.AddSeconds(30));

                    var urlAr = HtmlInteractionStatisticReportUrl + "/ar?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
                    var urlEn = HtmlInteractionStatisticReportUrl + "/en?token=" + Token.Replace("bearer ", "") + "&entityId=" + RequestedData.entityId + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

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

        #endregion


    }
}
