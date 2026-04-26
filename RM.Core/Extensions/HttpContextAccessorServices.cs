

using Microsoft.AspNetCore.Http;

namespace RM.Core.Extensions
{
    public static class HttpContextAccessorExtension
    {
        public static string GetParamValueFromHeader(this IHttpContextAccessor _context, string pramValue)
        {
            try
            {
                return _context.HttpContext.Request.Headers[pramValue].FirstOrDefault();
            }
            catch 
            {
                return null;
            }
        }

       
    }
}
