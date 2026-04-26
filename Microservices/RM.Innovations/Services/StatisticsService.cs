using DocumentFormat.OpenXml.Bibliography;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using RM.Innovations.Dtos;
using RM.Innovations.UnitOfWorks;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Services;
using static RM.Innovations.Dtos.OperationOutput;

namespace RM.Innovations.Services
{
    public class StatisticsService : BaseService, IStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private static string HtmlIdeasReportUrl = string.Empty;


        public StatisticsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;

            HtmlIdeasReportUrl = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("HtmlIdeasReportUrl").Value;
        }

        public async Task<OperationOutput> GetLookUps(Dtos.Statistics RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var GregorianYears = Enumerable.Range(2023, DateTime.Now.Year - 2022).ToList();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary("YearsList", GregorianYears));
        }

        public async Task<OperationOutput> GetIdeasStatistics(Dtos.Statistics RequestedData)
        {
            StatisticsResult IdeasStatistics = null;

                var Ideas = await _unitOfWork.Ideas.GetAll().Include(x => x.LastAction).ThenInclude(x=>x.TypeNavigation)
                    .Where(x => RequestedData.Year.HasValue ? x.CreatedDate.Value.Year == RequestedData.Year : true)
                    .Where(x => RequestedData.Quarter.HasValue ?
                      RequestedData.Quarter == 1 ? x.CreatedDate.Value.Month <= 3
                    : RequestedData.Quarter == 2 ? x.CreatedDate.Value.Month >= 4 && x.CreatedDate.Value.Month <= 6
                    : RequestedData.Quarter == 3 ? x.CreatedDate.Value.Month >= 7 && x.CreatedDate.Value.Month <= 9
                    : RequestedData.Quarter == 4 ? x.CreatedDate.Value.Month >= 10 : true : true)
                    .ToListAsync();

            var majorStatuses = _unitOfWork.MajorLookups.GetAll().Where(x=>x.TypeId == (int) Enums.MajorLookupsTypes.IdeaActions).AsNoTracking().ToList();

            IdeasStatistics = GetStatistics(RequestedData, Ideas, majorStatuses);

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.IdeasCount, Ideas.Count),
                     new OutputDictionary(OperationOutputKeys.IdeasStatistics, IdeasStatistics));
        }

        private StatisticsResult GetStatistics(Dtos.Statistics RequestedData, List<Models.Idea> List,List<Models.MajorLookup> majorStatuses)
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

        private StatisticsResult GetInitStatistics(int Year, List<Models.MajorLookup> majorStatuses)
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
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(majorStatuses,1,new List<Models.Idea>(),0,1,new List<Models.Idea>(),2,new List<Models.Idea>(),3,new List<Models.Idea>())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(majorStatuses,2,new List<Models.Idea>(),0,4,new List<Models.Idea>(),5,new List<Models.Idea>(),6,new List<Models.Idea>())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(majorStatuses,3,new List<Models.Idea>(),0,7,new List<Models.Idea>(),8,new List<Models.Idea>(),9,new List<Models.Idea>())),
                                       new QuarterResult().Adapt<QuarterResult>(QuarterResult.SelectConfig(majorStatuses,4,new List<Models.Idea>(),0,10,new List<Models.Idea>(),11,new List<Models.Idea>(),12,new List<Models.Idea>()))
                                   }
                           }
                    }
            };
        }


        #region SendEmail

        public async Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord cron)
        {
            OperationOutput Result = new OperationOutput();

            var CronSettings = _unitOfWork.CronSettings.GetAll().Where(x => x.IsActive == true && x.CronTypeId == cron.CronTypeId && x.EntityId == (int)Enums.Entities.Innovations).AsNoTracking().ToList();
            var emails = CronSettings.SelectMany(x => Strings.ConvertStringToList(x.Emails, "$")).Distinct().ToList();


            var Entity = _unitOfWork.Entities.Find(x => x.Id == (int)Enums.Entities.Innovations);
            await SendEmailIdeasStatistics(new Statistics() { EntityId = (int)Enums.Entities.Innovations, Emails = emails, Subject = Entity.NameAr, FileName = Entity.NameEn });

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }


        public async Task<OperationOutput> GetIdeasStatisticsReport(Dtos.Statistics RequestedData)
        {
              var Result = _unitOfWork.MemoryCache.Get<OperationOutput>("IdeasStatistics" + RequestedData.Year + RequestedData.Quarter);

            if (Result != null)
                return Result;

            return await GetIdeasStatistics(RequestedData);
        }
        public async Task<OperationOutput> SendEmailIdeasStatistics(Dtos.Statistics RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            bool Success = false;

            var data = await GetIdeasStatistics(RequestedData);
            _unitOfWork.MemoryCache.Set("IdeasStatistics" + RequestedData.Year + RequestedData.Quarter, data, DateTimeOffset.Now.AddSeconds(30));

            var urlAr = HtmlIdeasReportUrl + "/ar"  + "?token=" + Token.Replace("bearer ", "") + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
            var urlEn = HtmlIdeasReportUrl + "/en"  + "?token=" + Token.Replace("bearer ", "") + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

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

            Result.Header = ApplicationOperation.OperationResult(Success == true ? Enums.ServiceMessages.TransactionSuccess : Enums.ServiceMessages.TransactionErorr);
            return Result;

        }

        public async Task<FileStreamResult> ExportIdeasStatistics(Dtos.Statistics RequestedData)
        {
            MemoryStream stream;
            List<(byte[], string)> files = new List<(byte[], string)>();

            var Entity = _unitOfWork.Entities.Find(x => x.Id == (int) Enums.Entities.Innovations);

            var data = await GetIdeasStatistics(RequestedData);
            _unitOfWork.MemoryCache.Set("IdeasStatistics" + RequestedData.Year + RequestedData.Quarter, data, DateTimeOffset.Now.AddSeconds(30));

            var urlAr = HtmlIdeasReportUrl + "/ar"  + "?token=" + Token.Replace("bearer ", "") + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;
            var urlEn = HtmlIdeasReportUrl + "/en"  + "?token=" + Token.Replace("bearer ", "") + "&year=" + RequestedData.Year + "&quarter=" + RequestedData.Quarter;

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


            stream = new MemoryStream(Files.CompressToZip(files));
            stream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(stream, "application/zip")
            {
                FileDownloadName = "IdeasStatisticsReport",
            };

        }

        #endregion

    }
}
