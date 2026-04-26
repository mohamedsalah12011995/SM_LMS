using IntegrationService.Records;
using IntegrationService.Records.SMS;
using Mapster;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace IntegrationService.Services
{
    public class SMSService : ISMSService
    {
        public SMSConfiguration _SMSConfiguration { get; set; }
        public PInfoConfiguration _PInfoConfiguration { get; set; }

        public SMSService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, SMSConfiguration SMSConfiguration, PInfoConfiguration PInfoConfiguration)
        {
            _SMSConfiguration = SMSConfiguration;
            _PInfoConfiguration = PInfoConfiguration;
        }

        public async Task<GetSMSData> SendMsg(SendMsgRecord data)
        {
            string res = "";
            var reqBody = new SMSData() { SendMsgData = new AllMessagesObj() { AllMessages = new SMSDataRequestBody() { Mobile = _SMSConfiguration.Mobile, Msg = data.Message, Numbers = data.Number, Password = _SMSConfiguration.Password, Sender = _SMSConfiguration.Sender } } };
            var result = new GetSMSData() { Header = new Header() { IsSuccess = false }, Body = new SMSDataBodyIntegration() };

            try
            {
                // var info = reqBody.Adapt<SMSData>(SMSData.MapConfig());
                HttpResponseMessage response = await Invoke(JsonSerializer.Serialize(reqBody));
                res = await response.Content.ReadAsStringAsync();
                // var ret = JsonSerializer.Deserialize<SMSDataResponse>(res).Adapt<GetSMSData>(GetSMSData.MapConfig());

                //if (ret.Body.SMSInformation.FirstOrDefault() != null && ret.Body.SMSInformation.FirstOrDefault()["ResponseStatus"] != "success")
                //    ret.Header.IsSuccess = false;
                if (res != null && res.Contains("success"))
                    result.Header.IsSuccess = true;

                return result;
            }
            catch (Exception ex)
            {
                return new GetSMSData() { Header = new Header() { IsSuccess = false, Message = ex.Message + System.Environment.NewLine + "yaqeenRes:" + res + System.Environment.NewLine + "track:" + ex.StackTrace + System.Environment.NewLine + "input:" + JsonSerializer.Serialize(reqBody) } };
            }
        }

        private async Task<HttpResponseMessage> Invoke(string RequestEncapsulation)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request;
            request = new HttpRequestMessage(HttpMethod.Post, _SMSConfiguration.API);

            var byteArray = Encoding.ASCII.GetBytes($"{_PInfoConfiguration.UserName}:{_PInfoConfiguration.Password}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            request.Content = new StringContent(RequestEncapsulation, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.SendAsync(request);
            return response;
        }
    }
}
