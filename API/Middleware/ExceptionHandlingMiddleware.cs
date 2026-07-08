using System.Text.Json;
using ProductManagementApi.API.Common;

namespace ProductManagementApi.API.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var includeDetails = environment.IsDevelopment();
        var errorResponse = ExceptionMapper.Map(exception, context.TraceIdentifier, includeDetails);

        LogException(context, exception, errorResponse.Status);

        if (!context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorResponse.Status;
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, JsonOptions));
        }
    }

    private void LogException(HttpContext context, Exception exception, int statusCode)
    {
        var method = context.Request.Method;
        var path = context.Request.Path.Value ?? string.Empty;
        var traceId = context.TraceIdentifier;

        if (ExceptionMapper.IsClientError(exception))
        {
            logger.LogWarning(
                exception,
                "HTTP {Method} {Path} responded {StatusCode}. TraceId: {TraceId}. Message: {Message}",
                method,
                path,
                statusCode,
                traceId,
                exception.Message);
            return;
        }

        logger.LogError(
            exception,
            "HTTP {Method} {Path} responded {StatusCode}. TraceId: {TraceId}",
            method,
            path,
            statusCode,
            traceId);
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
    };
}
