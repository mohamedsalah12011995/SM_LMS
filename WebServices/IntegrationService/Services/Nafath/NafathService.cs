using IntegrationService.Records;
using IntegrationService.Records.Hars;
using IntegrationService.Records.Nafath;
using Mapster;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace IntegrationService.Services
{
    public class NafathService : INafathService
    {
        public NafathConfiguration _NafathConfiguration { get; set; }
        public IMemoryCache _MemoryCache { get; set; }
        public NafathService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, NafathConfiguration NafathConfiguration, IMemoryCache memoryCache)
        {
            _NafathConfiguration = NafathConfiguration;
            _MemoryCache = memoryCache;

        }

        public async Task<SendRequestResponse> SendRequestForGetRandom(SendRequestRecord data)
        {
            try
            {
                HttpResponseMessage response = await Invoke(JsonSerializer.Serialize(data), HttpMethod.Post);
                return response.Content.ReadFromJsonAsync<SendRequestResponse>().Result;
            }
            catch (Exception ex) 
            { 
                return new SendRequestResponse() { TransId=ex.Message,Random=ex.StackTrace};
            }
        }

        public async Task<NaFathCallbackStatusResult> NaFathCallbackStatus(NaFathCallbackStatusRecord data)
        {
            try
            {
                var parts = data.Response.Split('.');

                if (parts.Length < 2)
                {
                    return new NaFathCallbackStatusResult() { Status = "invalid" };
                }

               // string header = DecodeBase64(parts[0]);
                string payload = DecodeBase64(parts[1]);

                var res = JsonSerializer.Deserialize<NaFathCallbackStatusResult>(payload);

                if(!string.IsNullOrEmpty(res.TransId))
                   _MemoryCache.Set(res.TransId, res.Status, DateTimeOffset.Now.AddMinutes(5));
                
                return res;
            }
            catch (Exception ex)
            {
                return new NaFathCallbackStatusResult() {  Status = ex.Message, TransId = ex.StackTrace };
            }
        }

        public async Task<NaFathCallbackStatusResult> GetNaFathCallbackStatus(NaFathCallbackStatusResult data)
        {
            try
            {
                var res = new NaFathCallbackStatusResult();
                res.TransId = data.TransId;
                res.Status = _MemoryCache.Get<string>(data.TransId);
                return res;
            }
            catch (Exception ex)
            {
                return new NaFathCallbackStatusResult() { Status = ex.Message, TransId = ex.StackTrace };
            }
        }

        public async Task<CheckRequestResponse> CheckRequestStatusNaFath(CheckRequestRecord data)
        {
            try
            {
                HttpResponseMessage response = await Invoke(JsonSerializer.Serialize(data), HttpMethod.Post);
                return response.Content.ReadFromJsonAsync<CheckRequestResponse>().Result;
            }
            catch (Exception ex) 
            {
                return new CheckRequestResponse() {  Status = ex.Message, AccessToken = ex.StackTrace };
            }
        }


        private async Task<HttpResponseMessage> Invoke(string RequestEncapsulation , HttpMethod httpMethod)
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer
            };

            cookieContainer.Add(new Uri(_NafathConfiguration.API), new Cookie("Cookie", _NafathConfiguration.Cookie));

            using var httpClient = new HttpClient(handler);

            HttpRequestMessage request;       
            request = new HttpRequestMessage(httpMethod, _NafathConfiguration.API);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", _NafathConfiguration.ApiKey);

            request.Content = new StringContent(RequestEncapsulation, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.SendAsync(request);
            return response;
        }

        public string DecodeBase64(string base64)
        {
            // Fix padding if necessary
            base64 = base64.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            var bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
