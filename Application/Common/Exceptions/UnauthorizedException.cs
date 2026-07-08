namespace ProductManagementApi.Application.Common.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Invalid email or password.")
        : base("Unauthorized.", message, ApplicationStatusCodes.Status401Unauthorized)
    {
    }

    public UnauthorizedException(string field, string message)
        : base("Unauthorized.", message, ApplicationStatusCodes.Status401Unauthorized)
    {
        Errors = new Dictionary<string, string[]>
        {
            [field] = [message]
        };
    }

    public override IDictionary<string, string[]>? Errors { get; }
}
