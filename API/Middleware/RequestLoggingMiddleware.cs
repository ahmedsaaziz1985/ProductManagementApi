using System.Diagnostics;

namespace ProductManagementApi.API.Middleware;

public class RequestLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (IsSwaggerRequest(context.Request.Path))
        {
            await next(context);
            return;
        }

        var method = context.Request.Method;
        var path = context.Request.Path.Value ?? string.Empty;
        var traceId = context.TraceIdentifier;

        logger.LogInformation(
            "HTTP {Method} {Path} started. TraceId: {TraceId}",
            method,
            path,
            traceId);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();

            var statusCode = context.Response.StatusCode;
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            if (statusCode >= 500)
            {
                logger.LogError(
                    "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms. TraceId: {TraceId}",
                    method,
                    path,
                    statusCode,
                    elapsedMilliseconds,
                    traceId);
            }
            else if (statusCode >= 400)
            {
                logger.LogWarning(
                    "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms. TraceId: {TraceId}",
                    method,
                    path,
                    statusCode,
                    elapsedMilliseconds,
                    traceId);
            }
            else
            {
                logger.LogInformation(
                    "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms. TraceId: {TraceId}",
                    method,
                    path,
                    statusCode,
                    elapsedMilliseconds,
                    traceId);
            }
        }
    }

    private static bool IsSwaggerRequest(PathString path) =>
        path.StartsWithSegments("/swagger") || path.StartsWithSegments("/openapi");
}
