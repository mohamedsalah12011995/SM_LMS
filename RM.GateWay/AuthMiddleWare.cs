

namespace RM.GateWay
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthMiddleWare
    {
        private readonly RequestDelegate _next;
 
        public AuthMiddleWare(RequestDelegate next )
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            if (httpContext.Request.Headers["Authorization"].Count > 0)
            {
                if (httpContext.Request.Headers["Authorization"][0].Contains("Bearer"))
                httpContext.Request.Headers["Authorization"] = "bearer " + RM.Core.Helpers.Strings.DecompressString(httpContext.Request.Headers["Authorization"].ToString().Split(" ")[1]);
            }
           // httpContext.Request.Headers.Add("FromGateway","true");
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthMiddleWareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleWare>();
        }
    }
}
