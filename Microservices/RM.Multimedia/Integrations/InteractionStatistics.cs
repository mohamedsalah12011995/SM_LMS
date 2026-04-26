using RM.Core.Helpers;
using RM.Multimedia.Dtos;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RM.Multimedia.Integrations
{
    public class InteractionStatistics
    {
        public static async Task<OperationOutput> SaveInteractionStatistics(string StatisticsServiceUrl, string Token, int? ReferenceId, int? EntityId, int? ItemId,
            Enums.InteractionStatisticsType StatisticsType)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request;
            var InteractionStatisticsInfo = new
            {
                ReferenceId = ReferenceId,
                ItemId = ItemId,
                EntityId = EntityId,
                StatisticsType = (int)Enums.InteractionStatisticsType.ViewsCount
            };

            var RequestEncapsulation = JsonSerializer.Serialize(InteractionStatisticsInfo);
            request = new HttpRequestMessage(HttpMethod.Post, StatisticsServiceUrl);
            request.Headers.Add("Authorization", Token);
            request.Content = new StringContent(RequestEncapsulation, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.SendAsync(request);
            return JsonSerializer.Deserialize<OperationOutput>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
