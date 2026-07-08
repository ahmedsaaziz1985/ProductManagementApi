namespace ProductManagementApi.Application.Common.Models;

public class ErrorResponse
{
    public string Title { get; init; } = string.Empty;
    public int Status { get; init; }
    public string TraceId { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string? Detail { get; init; }
    public IDictionary<string, string[]>? Errors { get; init; }
}
