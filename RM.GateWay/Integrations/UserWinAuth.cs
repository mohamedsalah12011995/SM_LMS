using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;


namespace RM.GateWay.Integrations
{
    public class UserWinAuth
    {
        public static async Task<OperationOutput> GenerateTokenWinAuth(string GenerateTokenWinAuthUrl, string userName)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request;
            var LoginInfo = new { UserName = userName};

            var RequestEncapsulation = JsonConvert.SerializeObject(LoginInfo);
            request = new HttpRequestMessage(HttpMethod.Post, GenerateTokenWinAuthUrl);
          //  request.Headers.Add("Authorization", Token);
            request.Content = new StringContent(RequestEncapsulation, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.SendAsync(request);
            return JsonConvert.DeserializeObject<OperationOutput>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
