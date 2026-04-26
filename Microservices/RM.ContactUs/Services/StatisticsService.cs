using DocumentFormat.OpenXml.Bibliography;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using RM.ContactUs.Dtos;
using RM.ContactUs.UnitOfWorks;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Services;
using static RM.ContactUs.Dtos.OperationOutput;

namespace RM.ContactUs.Services
{
    public class StatisticsService : BaseService, IStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private static string HtmlSuggestionsAndComplaintsReportUrl = string.Empty;


        public StatisticsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;

            HtmlSuggestionsAndComplaintsReportUrl = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("HtmlSuggestionsAndComplaintsReportUrl").Value;
        }

        public async Task<OperationOutput> GetLookUps(Dtos.Statistics RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Entities = _unitOfWork.Entities.GetAll().AsNoTracking().ToList().Adapt<List<Entity>>();
       
            List<EntityItems> EntitiesItems = await GetEntities();
            var GregorianYears = Enumerable.Range(2023, DateTime.Now.Year - 2022).ToList();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary("EntityList", Entities),
               new OutputDictionary("YearsList", GregorianYears),
               new OutputDictionary(OperationOutputKeys.EntityItems, EntitiesItems));
        }

        private async Task<List<EntityItems>> GetEntities()
        {
            List<EntityItems> EntitiesItems = new List<EntityItems>();

            var Entitys = await _unitOfWork.ContactUs.GetAll().Where(x => x.ItemEntityId.HasValue).Include(x => x.ItemEntity).AsNoTracking().ToListAsync();

            var Entities = Entitys.GroupBy(x => new { x.ItemEntityId }, (key, group) => new EntityItems
            {
                Id = key.ItemEntityId,
                NameAr = group.First().ItemEntity.NameAr,
                NameEn = group.First().ItemEntity.NameEn,
                Items = group.Where(x => x.ItemId.HasValue).Select(x => new EntityItems { Id = x.ItemId }).ToList()
            });

            foreach (var entityItem in Entities)
                EntitiesItems.Add(getItemDetails(entityItem));
            return EntitiesItems;
        }

        private async Task<EntityItems> GetEntityItems(int? ItemEntityId)
        {
            var Entitys = await _unitOfWork.ContactUs.GetAll().Include(x => x.ItemEntity)
                                           .Where(x => x.ItemEntityId == ItemEntityId)
                                           .AsNoTracking().ToListAsync();

            var entityItem = Entitys.GroupBy(x => new { x.ItemEntityId }, (key, group) => new EntityItems
            {
                Id = key.ItemEntityId,
                NameAr = group.First().ItemEntity.NameAr,
                NameEn = group.First().ItemEntity.NameEn,
                Items = group.Where(x=>x.ItemId.HasValue).Select(x => new EntityItems { Id = x.ItemId }).ToList()
            }).FirstOrDefault();

            return getItemDetails(entityItem);
        }

        private EntityItems getItemDetails(EntityItems item)
        {
            if (item != null && item.Items != null)
            {
                var itemsIds = item.Items.Select(x => x.Id).ToList();
                if (itemsIds.Any())
                {
                    if (item.Id == (int)Enums.Entities.EServices)
                        item.Items = _unitOfWork.Eservices.GetAll().Where(x => itemsIds.Contains(x.Id))
                                     .Select(x => new EntityItems { Id = x.Id, NameAr = x.NameAr, NameEn = x.NameEn }).AsNoTracking().ToList();
                    else if (item.Id == (int)Enums.Entities.GovServices)
                        item.Items = _unitOfWork.GovServices.GetAll().Where(x => itemsIds.Contains(x.Id))
                                     .Select(x => new EntityItems { Id = x.Id, NameAr = x.NameAr, NameEn = x.NameEn }).AsNoTracking().ToList();
                }
            }
            return item;
        }

        public async Task<OperationOutput> GetSuggestionsAndComplaintsStatistics(Dtos.Statistics RequestedData)
        {
            StatisticsResult SuggestionsStatistics = null;
            StatisticsResult ComplaintsStatistics = null;
            int SuggestionsCount=0, ComplaintsCount=0;

            if (RequestedData.EntityId == (int)Enums.Entities.Suggestions)
            {
                var majorStatuses = _unitOfWork.MajorStatus.GetAll().AsNoTracking().ToList();

                var Suggestions = await _unitOfWork.ContactUs.GetAll().Where(x => x.EntityId == (int)Enums.Entities.Suggestions).Include(x => x.ItemEntity)
                    .Where(x => RequestedData.Year.HasValue ? x.CreatedDate.Value.Year == RequestedData.Year : true)
                    .Where(x => RequestedData.Quarter.HasValue ?
                      RequestedData.Quarter == 1 ? x.CreatedDate.Value.Month <= 3
                    : RequestedData.Quarter == 2 ? x.CreatedDate.Value.Month >= 4 && x.CreatedDate.Value.Month <= 6
                    : RequestedData.Quarter == 3 ? x.CreatedDate.Value.Month >= 7 && x.CreatedDate.Value.Month <= 9
                    : RequestedData.Quarter == 4 ? x.CreatedDate.Value.Month >= 10 : true : true)
                    .ToListAsync();

                SuggestionsCount = Suggestions.Count;
                SuggestionsStatistics = GetStatistics(RequestedData,Suggestions, majorStatuses);

            }
            else if (RequestedData.EntityId == (int)Enums.Entities.Complaint)
            {
                var majorStatuses = _unitOfWork.MajorStatus.GetAll().AsNoTracking().ToList();
                //var Recomendations = _unitOfWork.Recommendations.GetAll().Where(x => x.EntityId == (int)Enums.Entities.Complaint).AsNoTracking().ToList();

                //var ItemStatistics = _unitOfWork.InteractionStatistic.GetAll()
                //    .Where(x => RequestedData.ItemEntityId.HasValue ? x.EntityId == RequestedData.ItemEntityId : true)
                //    .Where(x => RequestedData.ItemId.HasValue ? x.ItemId == RequestedData.ItemId : true)
                //    .Where(x => RequestedData.Year.HasValue ? x.CreatedDate.Value.Year == RequestedData.Year : true)
                //    .Where(x => RequestedData.Quarter.HasValue ?
                //      RequestedData.Quarter == 1 ? x.CreatedDate.Value.Month <= 3
                //    : RequestedData.Quarter == 2 ? x.CreatedDate.Value.Month >= 4 && x.CreatedDate.Value.Month <= 6
                //    : RequestedData.Quarter == 3 ? x.CreatedDate.Value.Month >= 7 && x.CreatedDate.Value.Month <= 9
                //    : RequestedData.Quarter == 4 ? x.CreatedDate.Value.Month >= 10 : true : true)
                //    .ToListAsync();

                var Complaints = await _unitOfWork.ContactUs.GetAll().Where(x => x.EntityId == (int)Enums.Entities.Complaint).Include(x => x.ItemEntity).Include(x => x.LastStatus)
                    .Where(x => RequestedData.ItemEntityId.HasValue ? x.ItemEntityId == RequestedData.ItemEntityId : true)
                    .Where(x => RequestedData.ItemId.HasValue ? x.ItemId == RequestedData.ItemId : true)
                    .Where(x => RequestedData.Year.HasValue ? x.CreatedDate.Value.Year == RequestedData.Year : true)
                    .Where(x => RequestedData.Quarter.HasValue ?
                      RequestedData.Quarter == 1 ? x.CreatedDate.Value.Month <= 3
                    : RequestedData.Quarter == 2 ? x.CreatedDate.Value.Month >= 4 && x.CreatedDate.Value.Month <= 6
                    : RequestedData.Quarter == 3 ? x.CreatedDate.Value.Month >= 7 && x.CreatedDate.Value.Month <= 9
                    : RequestedData.Quarter == 4 ? x.CreatedDate.Value.Month >= 10 : true : true)
                    .ToListAsync();

                ComplaintsCount = Complaints.Count;
                ComplaintsStatistics = GetStatistics(RequestedData,Complaints, majorStatuses);
            }

            EntityItems Entity = RequestedData.ItemEntityId.HasValue ? await GetEntityItems(RequestedData.ItemEntityId):null;

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.SuggestionsCount, SuggestionsCount),
                     new OutputDictionary(OperationOutputKeys.ComplaintsCount, ComplaintsCount),
                     new OutputDictionary(OperationOutputKeys.SuggestionsStatistics, SuggestionsStatistics),
                     new OutputDictionary(OperationOutputKeys.ComplaintsStatistics, ComplaintsStatistics),
                     new OutputDictionary("EntityObj", Entity),
                     new OutputDictionary("ItemObj", Entity != null ? Entity.Items.FirstOrDefault(x => x.Id == RequestedData.ItemId) : null));
        }

        private StatisticsResult GetStatistics(Dtos.Statistics RequestedData, List<Models.ContactU> List,List<Models.MajorStatus> majorStatuses)
        {
            if(!List.Any())
                return GetInitStatistics(RequestedData.Year ?? TransactionDate.Year, majorStatuses);

            return new StatisticsResult
            {
                ThisDay = List.Count(x => x.CreatedDate.Value.Day == TransactionDate.Day),
                ThisWeek = List.Count(x => x.CreatedDate.Value.Year == TransactionDate.Year && x.CreatedDate.Value.Month == TransactionDate.Month && x.CreatedDate.Value.Day >= StartWeekDate.Day && x.CreatedDate.Value.Day <= EndWeekDate.Day),
                ThisMonth = List.Count(x => x.CreatedDate.Value.Month == TransactionDate.Month),
                ThisYear = List.Count(x => x.CreatedDate.Value.Year == TransactionDate.Year),
                Total = List.Count,
                Years = List.GroupBy(x => new { x.CreatedDate.Value.Year }, (key, group) =>
                       new YearResult
                       {
                           Year = key.Year,
                           YearRate = 0,
                           YearCount = group.Count(),
                           Quarters = new List<QuarterResult>
                               {
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(majorStatuses,1,group.Where(x=>x.CreatedDate.Value.Month <= 3).ToList(),group.Count(),1,group.Where(x=>x.CreatedDate.Value.Month == 1).ToList(),2,group.Where(x=>x.CreatedDate.Value.Month == 2).ToList(),3,group.Where(x=>x.CreatedDate.Value.Month == 3).ToList())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(majorStatuses,2,group.Where(x=>x.CreatedDate.Value.Month >= 4 && x.CreatedDate.Value.Month <=6).ToList(),group.Count(),4,group.Where(x=>x.CreatedDate.Value.Month == 4).ToList(),5,group.Where(x=>x.CreatedDate.Value.Month == 5).ToList(),6,group.Where(x=>x.CreatedDate.Value.Month == 6).ToList())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(majorStatuses,3,group.Where(x=>x.CreatedDate.Value.Month >= 7 && x.CreatedDate.Value.Month <=9).ToList(),group.Count(),7,group.Where(x=>x.CreatedDate.Value.Month == 7).ToList(),8,group.Where(x=>x.CreatedDate.Value.Month == 8).ToList(),9,group.Where(x=>x.CreatedDate.Value.Month == 9).ToList())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(majorStatuses,4,group.Where(x=>x.CreatedDate.Value.Month >= 10).ToList(),group.Count(),10,group.Where(x=>x.CreatedDate.Value.Month == 10).ToList(),11,group.Where(x=>x.CreatedDate.Value.Month == 11).ToList(),12,group.Where(x=>x.CreatedDate.Value.Month == 12).ToList()))
                               }
                       }).ToList()
            };
        }

        private StatisticsResult GetInitStatistics(int Year, List<Models.MajorStatus> majorStatuses)
        {
            return new StatisticsResult
            {
                ThisDay = 0,
                ThisWeek = 0,
                ThisMonth = 0,
                ThisYear = 0,
                Total = 0,
                Years = new List<YearResult> {
                           new YearResult
                           {
                               Year = Year,
                               YearRate = 0,
                               YearCount = 0,
                               Quarters = new List<QuarterResult>
                                   {
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(majorStatuses,1,new List<Models.ContactU>(),0,1,new List<Models.ContactU>(),2,new List<Models.ContactU>(),3,new List<Models.ContactU>())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(majorStatuses,2,new List<Models.ContactU>(),0,4,new List<Models.ContactU>(),5,new List<Models.ContactU>(),6,new List<Models.ContactU>())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(majorStatuses,3,new List<Models.ContactU>(),0,7,new List<Models.ContactU>(),8,new List<Models.ContactU>(),9,new List<Models.ContactU>())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(majorStatuses,4,new List<Models.ContactU>(),0,10,new List<Models.ContactU>(),11,new List<Models.ContactU>(),12,new List<Models.ContactU>()))
                                   }
                           }
                    }
            };
        }


        #region SendEmail

        public async Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord cron)
        {
            OperationOutput Result = new OperationOutput();

            await CronJobEntity((int)Enums.Entities.Complaint, cron);
            await CronJobEntity((int)Enums.Entities.Suggestions, cron);

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }

        private async Task CronJobEntity(int? EntityId, CronJobRecord cron)
        {

            var CronSettings = _unitOfWork.CronSettings.GetAll().Where(x => x.IsActive == true && x.CronTypeId == cron.CronTypeId && x.EntityId == EntityId).AsNoTracking().ToList();
            var emails = CronSettings.SelectMany(x => Strings.ConvertStringToList(x.Emails, "$")).Distinct().ToList();

            if (EntityId == (int)Enums.Entities.Complaint)
            {
                var EntitiesItems = await GetEntities();

                foreach (var itemEntity in EntitiesItems)
                {

                    if (itemEntity.Items.Any())
                    {
                        foreach (var item in itemEntity.Items)
                        {
                            await SendEmailSuggestionsAndComplaintsStatistics(new Statistics() {EntityId = EntityId, ItemEntityId = itemEntity.Id, ItemId = item.Id, Emails = emails, Subject = itemEntity.NameAr + " - " + item.NameAr, FileName = item.NameEn });
                        }
                    }
                    else
                        await SendEmailSuggestionsAndComplaintsStatistics(new Statistics() { EntityId = EntityId, ItemEntityId = itemEntity.Id, Emails = emails, Subject = itemEntity.NameAr, FileName = itemEntity.NameEn });
                }
            }
            else
            {
                var Entity = _unitOfWork.Entities.Find(x=>x.Id == EntityId);
                await SendEmailSuggestionsAndComplaintsStatistics(new Statistics() { EntityId = EntityId, Emails = emails, Subject = Entity.NameAr, FileName = Entity.NameEn });

            }
        }

        public async Task<OperationOutput> GetSuggestionsAndComplaintsStatisticsReport(Dtos.Statistics RequestedData)
        {
            OperationOutput Result = null;
            if (RequestedData.ItemId.HasValue)
                Result = _unitOfWork.MemoryCache.Get<OperationOutput>(RequestedData.itemEntityId + RequestedData.itemId);
            else
                Result = _unitOfWork.MemoryCache.Get<OperationOutput>(RequestedData.entityId);
            if (Result != null)
                return Result;

            return await GetSuggestionsAndComplaintsStatistics(RequestedData);
        }
        public async Task<OperationOutput> SendEmailSuggestionsAndComplaintsStatistics(Dtos.Statistics RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            bool Success = false;
            if (RequestedData.ItemEntityId.HasValue)
            {
                EntityItems Entity = await GetEntityItems(RequestedData.ItemEntityId.Value);
                if (Entity != null && Entity.Items.Any())
                {
                    List<(byte[], string)> filesAr = new List<(byte[], string)>();
                    List<(byte[], string)> filesEn = new List<(byte[], string)>();

                    foreach (var item in Entity.Items.Where(x => RequestedData.ItemId.HasValue ? x.Id == RequestedData.ItemId : true))
                    {
                        var data = await GetSuggestionsAndComplaintsStatistics(RequestedData);

                        _unitOfWork.MemoryCache.Set(RequestedData.itemEntityId + item.ID, data, DateTimeOffset.Now.AddSeconds(30));

                        var urlAr = HtmlSuggestionsAndComplaintsReportUrl + "/ar/" + RequestedData.entityId + "?token=" + Token.Replace("bearer ", "") + "&itemEntityId=" + RequestedData.itemEntityId + "&itemId=" + item.ID + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
                        var urlEn = HtmlSuggestionsAndComplaintsReportUrl + "/en/" + RequestedData.entityId + "?token=" + Token.Replace("bearer ", "") + "&itemEntityId=" + RequestedData.itemEntityId + "&itemId=" + item.ID + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

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
                            Success = await Email.SendEmailAsync(email, RequestedData.Subject, RequestedData.Body, EmailServiceUrl, Token, null, new List<EmailAttachment> { new EmailAttachment { FileName = filesAr[0].Item2, FileBytes = filesAr[0].Item1 }, new EmailAttachment { FileName = filesEn[0].Item2, FileBytes = filesEn[0].Item1 } });
                            Thread.Sleep(500);
                        }
                    }
                }
            }
            else
            {
                var data = await GetSuggestionsAndComplaintsStatistics(RequestedData);
                _unitOfWork.MemoryCache.Set(RequestedData.entityId, data, DateTimeOffset.Now.AddSeconds(30));

                var urlAr = HtmlSuggestionsAndComplaintsReportUrl + "/ar/" + RequestedData.entityId + "?token=" + Token.Replace("bearer ", "") + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
                var urlEn = HtmlSuggestionsAndComplaintsReportUrl + "/en/" + RequestedData.entityId + "?token=" + Token.Replace("bearer ", "") + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

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
                    Success = await Email.SendEmailAsync(email, RequestedData.Subject, RequestedData.Body, EmailServiceUrl, Token, null, new List<EmailAttachment> { new EmailAttachment { FileName = RequestedData.FileName + "_Ar.pdf", FileBytes = pdffFileAr }, new EmailAttachment { FileName = RequestedData.FileName + "_En.pdf", FileBytes = pdffFileEn } });
                    Thread.Sleep(500);
                }
            }
            Result.Header = ApplicationOperation.OperationResult(Success == true? Enums.ServiceMessages.TransactionSuccess : Enums.ServiceMessages.TransactionErorr);
            return Result;

        }

        public async Task<FileStreamResult> ExportSuggestionsAndComplaintsStatistics(Dtos.Statistics RequestedData)
        {
            MemoryStream stream;
            List<(byte[], string)> files = new List<(byte[], string)>();

            if (RequestedData.ItemEntityId.HasValue)
            {
                EntityItems Entity = await GetEntityItems(RequestedData.ItemEntityId);

                if (Entity != null && Entity.Items.Any())
                {
                    List<(byte[], string)> filesAr = new List<(byte[], string)>();
                    List<(byte[], string)> filesEn = new List<(byte[], string)>();

                    foreach (var item in Entity.Items.Where(x => RequestedData.ItemId.HasValue ? x.Id == RequestedData.ItemId : true))
                    {
                        var data = await GetSuggestionsAndComplaintsStatistics(RequestedData);

                        _unitOfWork.MemoryCache.Set(RequestedData.itemEntityId + item.ID, data, DateTimeOffset.Now.AddSeconds(30));

                        var urlAr = HtmlSuggestionsAndComplaintsReportUrl + "/ar/" + RequestedData.entityId + "?token=" + Token.Replace("bearer ", "") + "&itemEntityId=" + RequestedData.itemEntityId + "&itemId=" + item.ID + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
                        var urlEn = HtmlSuggestionsAndComplaintsReportUrl + "/en/" + RequestedData.entityId + "?token=" + Token.Replace("bearer ", "") + "&itemEntityId=" + RequestedData.itemEntityId + "&itemId=" + item.ID + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

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
            }
            else
            {
                var Entity =  _unitOfWork.Entities.Find(x => x.Id == RequestedData.EntityId);

                var data = await GetSuggestionsAndComplaintsStatistics(RequestedData);
                _unitOfWork.MemoryCache.Set(RequestedData.entityId, data, DateTimeOffset.Now.AddSeconds(30));

                var urlAr = HtmlSuggestionsAndComplaintsReportUrl + "/ar/" + RequestedData.entityId + "?token=" + Token.Replace("bearer ", "")  + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
                var urlEn = HtmlSuggestionsAndComplaintsReportUrl + "/en/" + RequestedData.entityId + "?token=" + Token.Replace("bearer ", "")  +  "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

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
                    FileDownloadName = "SuggestionsAndComplaintsReport",
                };

        }

        #endregion

    }
}
