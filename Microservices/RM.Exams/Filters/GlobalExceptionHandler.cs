using Microsoft.AspNetCore.Diagnostics;
using RM.Core.Helpers;
using RM.Exams.Dtos;


namespace RM.Exams.Filters
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
    /*
     
     using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IStringLocalizer<SharedResource> localizer)
    {
        _logger = logger;
        _localizer = localizer;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Log the exception
        _logger.LogError(
            "{ExceptionMessage} | {ExceptionTrace}",
            exception.Message, exception.StackTrace);

        // Get the localized error message
        string errorMessage = _localizer["ErrorMessage"]; // Always use the same key

        // Create the response object
        var response = new
        {
            success = false,
            message = errorMessage // Localized message
        };

        // Set the status code to 400 (BadRequest)
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = "application/json";

        // Write the JSON response
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        // Return true to indicate the exception was handled
        return true;
    }
}
     
     
     */

}
