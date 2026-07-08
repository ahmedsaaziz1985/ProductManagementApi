using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ProductManagementApi.IntegrationTests.Integration;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Development);

        builder.UseSetting("ConnectionStrings:DefaultConnection", string.Empty);
        builder.UseSetting("Jwt:Key", "YourSuperSecretJwtKey_ChangeThis_InProduction_Min32Chars!");
        builder.UseSetting("Jwt:Issuer", "ProductManagementApi");
        builder.UseSetting("Jwt:Audience", "ProductManagementApiClient");
        builder.UseSetting("Jwt:ExpirationInMinutes", "60");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = string.Empty,
                ["Jwt:Key"] = "YourSuperSecretJwtKey_ChangeThis_InProduction_Min32Chars!",
                ["Jwt:Issuer"] = "ProductManagementApi",
                ["Jwt:Audience"] = "ProductManagementApiClient",
                ["Jwt:ExpirationInMinutes"] = "60"
            });
        });
    }
}
