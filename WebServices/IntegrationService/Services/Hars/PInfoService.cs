using IntegrationService.Records;
using IntegrationService.Records.Hars;
using Mapster;
using RM.Core.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace IntegrationService.Services
{
    public class PInfoService: IPInfoService
    {
        public PInfoConfiguration _PInfoConfiguration { get; set; }
        public PInfoService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, PInfoConfiguration PInfoConfiguration)
        {
            _PInfoConfiguration = PInfoConfiguration;
        }

        #region WSData
        public async Task<object> GetWSData(GetWSData data)
        {
            HttpResponseMessage response = await Invoke(JsonSerializer.Serialize(data.Object), HttpMethod.Get, data.API);
            return  response.Content.ReadFromJsonAsync<object>().Result;
        }

        public async Task<object> PostWSData(GetWSData data)
        {
            HttpResponseMessage response = await Invoke(JsonSerializer.Serialize(data.Object), HttpMethod.Post, data.API);
            return  response.Content.ReadFromJsonAsync<object>().Result;
        }

        #endregion

        #region YaqeenWS
        public async Task<GetPersonInfoDataByPassport> GetGccInfoByPassport(GetGccInfoByPassportRequestBody personInfo)
        {
            try
            {
                var info = personInfo.Adapt<GetGccInfoByPassport>(Records.Hars.GetGccInfoByPassport.MapConfig());
                HttpResponseMessage response = await Invoke(JsonSerializer.Serialize(info), HttpMethod.Get, "/yaqeenws/getpersoninfowithsecuirtystatus");
                var res = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GetGccInfoByPassportResponse>(res).ResponseBody.Adapt<GetPersonInfoDataByPassport>(GetPersonInfoDataByPassport.MapConfig());
            }
            catch (Exception ex)
            {
                return new GetPersonInfoDataByPassport() { Header = new Header { Message = ex.Message } };
            }
        }

        public async Task<GetPersonInfoData> GetPersonInfoWithSecuirtyStatus(GetPersonInfoRequestBody personInfo)
        {
            try
            {
                var info = personInfo.Adapt<GetPersonInfo>(Records.Hars.GetPersonInfo.MapConfig());
                HttpResponseMessage response = await Invoke(JsonSerializer.Serialize(info), HttpMethod.Get, "/yaqeenws/getpersoninfowithsecuirtystatus");
                var res = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GetPersonInfoResponse>(res).ResponseBody.Adapt<GetPersonInfoData>(GetPersonInfoData.MapConfig());
            }
            catch (Exception ex)
            {
                return new GetPersonInfoData() { Header = new Header { Message = ex.Message } };
            }
        }

        public async Task<GetPersonInfoData> GetGccInfoByNIN(GetPersonInfoRequestBody personInfo)
        {
            try
            {
                var info = personInfo.Adapt<GetPersonInfo>(Records.Hars.GetPersonInfo.MapConfig());
                HttpResponseMessage response = await Invoke(JsonSerializer.Serialize(info), HttpMethod.Get, "/yaqeenws/getgccinfobynin");
                var res = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GetPersonInfoResponse>(res).ResponseBody.Adapt<GetPersonInfoData>(GetPersonInfoData.MapConfig());
            }
            catch (Exception ex)
            {
                return new GetPersonInfoData() { Header = new Header { Message = ex.Message } };
            }
        }

        public async Task<GetPersonInfoData> GetPersonInfoWithDetailedSecuirtyStatus(GetPersonInfoRequestBody personInfo)
        {
            try
            {
                var info = personInfo.Adapt<GetPersonInfo>(Records.Hars.GetPersonInfo.MapConfig());
                HttpResponseMessage response = await Invoke(JsonSerializer.Serialize(info), HttpMethod.Get, "/yaqeenws/getpersoninfowithdetailedsecuirtystatus");
                var res = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GetPersonInfoResponse>(res).ResponseBody.Adapt<GetPersonInfoData>(GetPersonInfoData.MapConfig());
            }
            catch (Exception ex)
            {
                return new GetPersonInfoData() { Header = new Header { Message = ex.Message } };
            }
        }

        public async Task<GetCarInfoData> GetCarInfoByPlate(GetCarInfoByPlateRequestBody carInfo)
        {
            string res="";
            try
            {
                var info = carInfo.Adapt<GetCarInfoByPlate>(Records.Hars.GetCarInfoByPlate.MapConfig());

                HttpResponseMessage response = await Invoke(JsonSerializer.Serialize(info), HttpMethod.Get, "/yaqeenws/getcarinfobyplate");
                res = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GetCarInfoByPlateResponse>(res).ResponseBody.Adapt<GetCarInfoData>(GetCarInfoData.MapConfig());
            }
            catch (Exception ex)
            {
                return new GetCarInfoData() { Header = new Header { Message = ex.Message +"\r\n\r\n yaqeenRes:"+res+ "\r\n\r\n track:"+ex.StackTrace } };
            }
        }
        public async Task<GetPersonInfoData> GetPersonInfo(GetPersonInfoRequestBody personInfo)
        {
            try
            {
                var info = personInfo.Adapt<GetPersonInfo>(Records.Hars.GetPersonInfo.MapConfig());
                HttpResponseMessage response = await Invoke(JsonSerializer.Serialize(info), HttpMethod.Get, "/yaqeenws/getpersoninfo");
                var res = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GetPersonInfoResponse>(res).ResponseBody.Adapt<GetPersonInfoData>(GetPersonInfoData.MapConfig());
            }
            catch (Exception ex) 
            {
                return new GetPersonInfoData() { Header =new Header { Message = ex.Message } };
            }
        }

        #endregion

        #region PayrollWS
        public async Task<GetLastPayrollResponseBody> GetLastPayroll(GetLastPayroll personInfo)
        {
            HttpResponseMessage response = await Invoke(JsonSerializer.Serialize(personInfo), HttpMethod.Get, "/yaqeenws/payrollws/getlastpayroll?"+personInfo.EmployeeId);
            return JsonSerializer.Deserialize<GetLastPayrollResponse>(response.Content.ReadAsStringAsync().Result).ResponseBody;
        }

        #endregion

        #region passws

        public async Task<object> GetDetailedStatistics(GetDetailedStatistics data)
        {
            HttpResponseMessage response = await Invoke("{}", HttpMethod.Post, "/passws/getgeneralstatistics?categoryId=" + data.CategoryId + "&year=" + data.Year + "&fromMonth=" +data.FromMonth + "&toMonth=" + data.ToMonth + "&regionId=" + data.RegionId + "&isHijri=" + data.IsHijri);
            return response.Content.ReadFromJsonAsync<object>().Result;
        }

        #endregion



        public async Task<object> GetExternalData(GetExternalData data)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request;
            request = new HttpRequestMessage(data.HttpMethod.ToLower() == "get" ? HttpMethod.Get : HttpMethod.Post, data.API);
            request.Content = new StringContent(JsonSerializer.Serialize(data.Object), Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.SendAsync(request);
            return response.Content.ReadFromJsonAsync<object>().Result;
        }

        private async Task<HttpResponseMessage> Invoke(string RequestEncapsulation , HttpMethod httpMethod, string api)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request;       
            request = new HttpRequestMessage(httpMethod, _PInfoConfiguration.ServerHost+api);

            var byteArray = Encoding.ASCII.GetBytes($"{_PInfoConfiguration.UserName}:{_PInfoConfiguration.Password}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            request.Content = new StringContent(RequestEncapsulation, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.SendAsync(request);
            return response;
        }
    }
}
