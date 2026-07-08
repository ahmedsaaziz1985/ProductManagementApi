using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ProductManagementApi.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static void Initialize(AppDbContext dbContext, ILogger logger)
    {
        if (!dbContext.Database.IsRelational())
        {
            dbContext.Database.EnsureCreated();
            return;
        }

        if (!dbContext.Database.CanConnect())
        {
            dbContext.Database.EnsureCreated();
            logger.LogInformation("Database created.");
            return;
        }

        if (IdentityTablesExist(dbContext))
        {
            dbContext.Database.EnsureCreated();
            return;
        }

        logger.LogWarning(
            "Identity tables are missing. Recreating database schema to include AspNetUsers and related tables.");

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        logger.LogInformation("Database schema recreated with Identity tables.");
    }

    private static bool IdentityTablesExist(AppDbContext dbContext)
    {
        var connection = dbContext.Database.GetDbConnection();

        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT OBJECT_ID(N'AspNetUsers', N'U')";
            var result = command.ExecuteScalar();
            return result is not null and not DBNull;
        }
        finally
        {
            connection.Close();
        }
    }
}
