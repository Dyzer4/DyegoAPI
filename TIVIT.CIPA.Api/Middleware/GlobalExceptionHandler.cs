using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TIVIT.CIPA.Api.Domain.Extensions;
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Middleware
{
    internal sealed class GlobalExceptionHandler(IHostEnvironment _env, ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error",
            };

            bool isGlobalAdmin = Profile.GlobalAdmin == httpContext.GetRole();

            if (_env.IsDevelopment() || isGlobalAdmin)
            {
                problemDetails.Type = exception.GetType().Name;
                problemDetails.Detail = exception.Message;
                problemDetails.Extensions["traceId"] = Activity.Current?.Id;
                problemDetails.Extensions["requestId"] = httpContext.TraceIdentifier;
                problemDetails.Extensions["data"] = exception.StackTrace;
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
