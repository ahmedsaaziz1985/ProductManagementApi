using ProductManagementApi.Application.Common.Interfaces.Services;

namespace ProductManagementApi.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}
