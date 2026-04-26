using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAuthorizationFilter
    {
        private const string API_KEY_HEADER_NAME = "ApiKey";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
            var apiKey = configuration["ApiKey"]; // Get API key from appsettings.json

            if (!context.HttpContext.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var providedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("API Key is missing.");
                return;
            }

            if (!apiKey.Equals(providedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("API Key is Unauthorized."); // 403 Forbidden
                return;
            }
        }
    }
}
