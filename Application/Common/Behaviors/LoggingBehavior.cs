using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagementApi.Application.Common.Exceptions;
using ProductManagementApi.Domain.Exceptions;

namespace ProductManagementApi.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation(
            "Handling {RequestName} {@Request}",
            requestName,
            request);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next();

            stopwatch.Stop();

            logger.LogInformation(
                "Handled {RequestName} successfully in {ElapsedMilliseconds}ms",
                requestName,
                stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception exception)
        {
            stopwatch.Stop();
            LogException(exception, requestName, request, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    private void LogException(
        Exception exception,
        string requestName,
        TRequest request,
        long elapsedMilliseconds)
    {
        switch (exception)
        {
            case ValidationException validationException:
                logger.LogWarning(
                    exception,
                    "Validation failed for {RequestName} after {ElapsedMilliseconds}ms. Errors: {@ValidationErrors}",
                    requestName,
                    elapsedMilliseconds,
                    validationException.Errors);
                break;

            case ConflictException conflictException:
                logger.LogWarning(
                    exception,
                    "Conflict in {RequestName} after {ElapsedMilliseconds}ms. Errors: {@ConflictErrors}",
                    requestName,
                    elapsedMilliseconds,
                    conflictException.Errors);
                break;

            case UnauthorizedException:
                logger.LogWarning(
                    exception,
                    "Unauthorized access in {RequestName} after {ElapsedMilliseconds}ms: {Message}",
                    requestName,
                    elapsedMilliseconds,
                    exception.Message);
                break;

            case NotFoundException:
                logger.LogWarning(
                    exception,
                    "Resource not found in {RequestName} after {ElapsedMilliseconds}ms",
                    requestName,
                    elapsedMilliseconds);
                break;

            case AppException:
                logger.LogWarning(
                    exception,
                    "Application error in {RequestName} after {ElapsedMilliseconds}ms: {Message}",
                    requestName,
                    elapsedMilliseconds,
                    exception.Message);
                break;

            case DomainException:
                logger.LogWarning(
                    exception,
                    "Domain rule violated in {RequestName} after {ElapsedMilliseconds}ms: {DomainMessage}",
                    requestName,
                    elapsedMilliseconds,
                    exception.Message);
                break;

            default:
                logger.LogError(
                    exception,
                    "Unhandled error in {RequestName} after {ElapsedMilliseconds}ms. Request: {@Request}",
                    requestName,
                    elapsedMilliseconds,
                    request);
                break;
        }
    }
}
