using RM.Core.Services;
using CronJobService.UnitOfWorks;
using RM.Core.Helpers;
using PuppeteerSharp;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using RM.Core.Integrations;
using Microsoft.EntityFrameworkCore;
using CronJobService.Dtos;

namespace CronJobService.Services
{
    public class CronSettingsService : BaseService, ICronSettingsService
    {

        private readonly IUnitOfWork _unitOfWork;
        protected string UsersServiceUrl = string.Empty;
        protected string SysUserName = string.Empty;
        protected string SysUserPassword = string.Empty;
        public CronSettingsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            UsersServiceUrl = _configuration.GetSection("AppSettings").GetSection("UsersServiceUrl").Value;
            SysUserName = _configuration.GetSection("AppSettings").GetSection("SysUserName").Value;
            SysUserPassword = _configuration.GetSection("AppSettings").GetSection("SysUserPassword").Value;
        }

        public async Task<OperationOutput> DoWork(int CronType)
        {
            var Token = await InteractionUsers.GetUserToken(UsersServiceUrl, SysUserName, SysUserPassword);

            if (CronType == (int) Enums.CronType.EveryDay)  
                await DoEveryDay(CronType, Token);
            else if (CronType == (int)Enums.CronType.EveryWeek)
                await DoEveryWeek(CronType, Token);
            else if (CronType == (int)Enums.CronType.EveryMonth)
                await DoEveryMonth(CronType, Token);
            else if (CronType == (int)Enums.CronType.EveryQuaters)
                await DoEveryQuarter(CronType, Token);
            else if (CronType == (int)Enums.CronType.WhenFinishToDate)
                await DoEveryHalfDay(CronType, Token);
            else return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.WrongeData);

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        private async Task DoEveryDay(int CronType,string token)
        {
            var APIs = _unitOfWork.MajorLookups.GetAll()
                        .Where(x => x.IsActive == true && x.TypeId == (int) Enums.MajorLookupsTypes.CronEveryDay || x.TypeId == (int)Enums.MajorLookupsTypes.CronJobsAll)
                        .AsNoTracking().ToList();

            foreach (var API in APIs)
                await CallAPI(CronType, token, API);
        }

        private async Task DoEveryWeek(int CronType, string token)
        {
            var APIs = _unitOfWork.MajorLookups.GetAll()
                        .Where(x => x.IsActive == true && x.TypeId == (int)Enums.MajorLookupsTypes.CronEveryWeek || x.TypeId == (int)Enums.MajorLookupsTypes.CronJobsAll)
                        .AsNoTracking().ToList();

            foreach (var API in APIs)
                await CallAPI(CronType, token, API);
        }

        private async Task DoEveryMonth(int CronType, string token)
        {
            var APIs = _unitOfWork.MajorLookups.GetAll()
                        .Where(x => x.IsActive == true && x.TypeId == (int)Enums.MajorLookupsTypes.CronEveryMonth || x.TypeId == (int)Enums.MajorLookupsTypes.CronJobsAll)
                        .AsNoTracking().ToList();

            foreach (var API in APIs)
                await CallAPI(CronType, token, API);
        }

        private async Task DoEveryQuarter(int CronType, string token)
        {
            var APIs = _unitOfWork.MajorLookups.GetAll()
                        .Where(x => x.IsActive == true && x.TypeId == (int)Enums.MajorLookupsTypes.CronEveryQuarter || x.TypeId == (int)Enums.MajorLookupsTypes.CronJobsAll)
                        .AsNoTracking().ToList();

            foreach (var API in APIs)
                await CallAPI(CronType, token, API);
        }

        private async Task DoEveryHalfDay(int CronType, string token)
        {
            var APIs = _unitOfWork.MajorLookups.GetAll()
                        .Where(x => x.IsActive == true && x.TypeId == (int)Enums.MajorLookupsTypes.CronEvery12Hour || x.TypeId == (int)Enums.MajorLookupsTypes.CronJobsAll)
                        .AsNoTracking().ToList();

            foreach (var API in APIs)
                await CallAPI(CronType, token, API);
        }

        private static async Task CallAPI(int CronType, string token, RM.Models.MajorLookup API)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request;
            var postInfo = new { CronTypeId = CronType, Body = API.NameAr };
            var RequestEncapsulation = JsonSerializer.Serialize(postInfo);
            request = new HttpRequestMessage(HttpMethod.Post, API.NameEn);
            request.Headers.Add("Authorization", token);
            request.Content = new StringContent(RequestEncapsulation, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            await httpClient.SendAsync(request);
        }
    }
}
