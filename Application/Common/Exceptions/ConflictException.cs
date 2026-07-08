namespace ProductManagementApi.Application.Common.Exceptions;

public class ConflictException : AppException
{
    public ConflictException(string message)
        : base("Conflict.", message, ApplicationStatusCodes.Status409Conflict)
    {
    }

    public ConflictException(string field, string message)
        : base("Conflict.", message, ApplicationStatusCodes.Status409Conflict)
    {
        Errors = new Dictionary<string, string[]>
        {
            [field] = [message]
        };
    }

    public override IDictionary<string, string[]>? Errors { get; }
}
