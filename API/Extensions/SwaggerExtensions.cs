using Microsoft.OpenApi;

namespace ProductManagementApi.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Product Management API",
                Version = "v1",
                Description = "APIs for managing products"
            });
        });

        return services;
    }

    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Management API v1");
            options.RoutePrefix = "swagger";
            options.DocumentTitle = "Product Management API";
        });

        return app;
    }
}
