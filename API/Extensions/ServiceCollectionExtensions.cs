using ProductManagementApi.Application;
using ProductManagementApi.Infrastructure;

namespace ProductManagementApi.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);

        services.AddControllers();
        services.AddOpenApi();
        services.AddSwaggerDocumentation();

        return services;
    }
}
