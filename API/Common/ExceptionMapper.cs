using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Application.Common.Exceptions;
using ProductManagementApi.Application.Common.Models;
using ProductManagementApi.Domain.Exceptions;
using AppStatusCodes = ProductManagementApi.Application.Common.Exceptions.ApplicationStatusCodes;

namespace ProductManagementApi.API.Common;

public static class ExceptionMapper
{
    public static ErrorResponse Map(Exception exception, string traceId, bool includeDetails)
    {
        return exception switch
        {
            AppException appException => new ErrorResponse
            {
                Title = appException.Title,
                Status = appException.StatusCode,
                TraceId = traceId,
                Type = GetTypeUri(appException.StatusCode),
                Detail = appException.Message,
                Errors = appException.Errors
            },

            DomainException domainException => new ErrorResponse
            {
                Title = "Business rule violation.",
                Status = AppStatusCodes.Status400BadRequest,
                TraceId = traceId,
                Type = GetTypeUri(AppStatusCodes.Status400BadRequest),
                Detail = domainException.Message
            },

            DbUpdateException dbUpdateException when IsUniqueConstraintViolation(dbUpdateException) => new ErrorResponse
            {
                Title = "Conflict.",
                Status = AppStatusCodes.Status409Conflict,
                TraceId = traceId,
                Type = GetTypeUri(AppStatusCodes.Status409Conflict),
                Detail = "A record with the same unique value already exists."
            },

            DbUpdateException dbUpdateException => new ErrorResponse
            {
                Title = "Database error.",
                Status = AppStatusCodes.Status500InternalServerError,
                TraceId = traceId,
                Type = GetTypeUri(AppStatusCodes.Status500InternalServerError),
                Detail = includeDetails
                    ? dbUpdateException.InnerException?.Message ?? dbUpdateException.Message
                    : "A database error occurred while processing the request."
            },

            _ => new ErrorResponse
            {
                Title = "An unexpected error occurred.",
                Status = AppStatusCodes.Status500InternalServerError,
                TraceId = traceId,
                Type = GetTypeUri(AppStatusCodes.Status500InternalServerError),
                Detail = includeDetails ? exception.Message : "An unexpected error occurred."
            }
        };
    }

    public static bool IsClientError(Exception exception) =>
        exception switch
        {
            AppException appException => appException.StatusCode < AppStatusCodes.Status500InternalServerError,
            DomainException => true,
            DbUpdateException dbUpdate when IsUniqueConstraintViolation(dbUpdate) => true,
            _ => false
        };

    private static bool IsUniqueConstraintViolation(DbUpdateException exception)
    {
        var message = exception.InnerException?.Message ?? exception.Message;

        return message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)
            || message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase)
            || message.Contains("2627")
            || message.Contains("2601");
    }

    private static string GetTypeUri(int statusCode) => statusCode switch
    {
        AppStatusCodes.Status400BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        AppStatusCodes.Status404NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        AppStatusCodes.Status409Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
        _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
    };
}
