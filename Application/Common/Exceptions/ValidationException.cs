using FluentValidation.Results;

namespace ProductManagementApi.Application.Common.Exceptions;

public class ValidationException : AppException
{
    public ValidationException()
        : base("Validation failed.", "One or more validation failures have occurred.", ApplicationStatusCodes.Status400BadRequest)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(failure => failure.PropertyName, failure => failure.ErrorMessage)
            .ToDictionary(group => group.Key, group => group.ToArray());
    }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("Validation failed.", "One or more validation failures have occurred.", ApplicationStatusCodes.Status400BadRequest)
    {
        Errors = errors;
    }

    public override IDictionary<string, string[]>? Errors { get; }
}
