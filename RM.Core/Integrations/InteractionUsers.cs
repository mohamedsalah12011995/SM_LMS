using RM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace RM.Core.Integrations
{
    public class InteractionUsers
    {
        public static async Task<string> GetUserToken(string UsersServiceUrl,string UserName,string Password)
        {

            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request;

            request = new HttpRequestMessage(HttpMethod.Post, UsersServiceUrl+ "GenerateToken");
            var responseToken = await httpClient.SendAsync(request);
            var Token = "bearer " + Strings.DecompressString(JsonSerializer.Deserialize<ApplicationOperation.ResponseOutput>(responseToken.Content.ReadAsStringAsync().Result).Output["UserJWT"].ToString());

            var RequestEncapsulation = JsonSerializer.Serialize(new { UserName, Password });
            request = new HttpRequestMessage(HttpMethod.Post, UsersServiceUrl + "UserLogin");
            request.Headers.Add("Authorization", Token);
            request.Content = new StringContent(RequestEncapsulation, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.SendAsync(request);
            return "bearer " + Strings.DecompressString(JsonSerializer.Deserialize<ApplicationOperation.ResponseOutput>(response.Content.ReadAsStringAsync().Result).Output["UserJWT"].ToString());
        }
    }
}
