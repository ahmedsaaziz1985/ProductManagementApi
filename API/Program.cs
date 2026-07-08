using Microsoft.EntityFrameworkCore;
using ProductManagementApi.API.Extensions;
using ProductManagementApi.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    DatabaseInitializer.Initialize(dbContext, logger);

    app.MapOpenApi();
    app.UseSwaggerDocumentation();
}

app.UseCustomExceptionHandling();
app.UseRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;
