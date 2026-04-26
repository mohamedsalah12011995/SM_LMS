using RM.Competitions.Dtos;
using RM.Core.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RM.Competitions.Integrations
{
    public class InvokeService
    {
        public static async Task<OperationOutput> Invoke(string ServiceUrl, string Token, dynamic RequestedData)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request;

            var RequestEncapsulation = JsonSerializer.Serialize(RequestedData);
            request = new HttpRequestMessage(HttpMethod.Post, ServiceUrl);
            request.Headers.Add("Authorization", Token);
            request.Content = new StringContent(RequestEncapsulation, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.SendAsync(request);
            return JsonSerializer.Deserialize<OperationOutput>(response.Content.ReadAsStringAsync().Result);
        }
        public static async Task<T> Invoke<T>(string ServiceUrl, string Token, dynamic RequestedData, ApplicationOperation.ResponseOutput Input, string OperationOutputKey)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request;
            JsonSerializerOptions SerializerOptions = new() { PropertyNameCaseInsensitive = false };

            var RequestEncapsulation = JsonSerializer.Serialize(RequestedData);
            request = new HttpRequestMessage(HttpMethod.Post, ServiceUrl);
            request.Headers.Add("Authorization", Token);
            request.Content = new StringContent(RequestEncapsulation, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.SendAsync(request);
            return JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result, SerializerOptions);
        }
        public static T DeserilizeServiceResponseToObject<T>(ApplicationOperation.ResponseOutput Input, string OperationOutputKey)
        {
            JsonSerializerOptions SerializerOptions = new() { PropertyNameCaseInsensitive = false };
            return JsonSerializer.Deserialize<T>(Input.Output[OperationOutputKey].ToString(), SerializerOptions);

        }
    }
}
