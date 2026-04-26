using Microsoft.AspNetCore.Diagnostics;
using RM.Articles.Dtos;
using RM.Core.Helpers;


namespace RM.Articles.Filters
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            //TODO
            var RequestedData = new object();
            //  var RequestedData = await _httpContextAccessor.HttpContext.Request.ReadFromJsonAsync<object>(cancellationToken);
            logger.LogError("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", exception.Message, exception.StackTrace, RequestedData);

            await httpContext.Response.WriteAsJsonAsync(new OperationOutput
            {
                Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionErorr)
            });
            return true;
        }
    }

}
