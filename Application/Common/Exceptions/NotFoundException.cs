namespace ProductManagementApi.Application.Common.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string name, object key)
        : base("Not found.", $"Entity \"{name}\" ({key}) was not found.", ApplicationStatusCodes.Status404NotFound)
    {
        EntityName = name;
        Key = key;
    }

    public string EntityName { get; }
    public object Key { get; }
}
