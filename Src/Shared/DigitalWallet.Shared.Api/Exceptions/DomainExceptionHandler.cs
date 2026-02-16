using DigitalWallet.Shared.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DigitalWallet.Shared.Api.Exceptions;

internal sealed class DomainExceptionHandler(ILogger<DomainExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not DomainException domainEx)
            return false;

        logger.LogWarning("Domain rule violated: {Message}", exception.Message);

        var (statusCode, title) = domainEx switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Resource Not Found"),
            InvalidRequestException => (StatusCodes.Status400BadRequest, "Validation Error"),
            _ => (StatusCodes.Status400BadRequest, "Domain Error")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Type = domainEx.GetType().Name
        };

        if (domainEx is InvalidRequestException invalidRequestEx)
        {
            problemDetails.Extensions["errors"] = invalidRequestEx.Errors;
        }

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
