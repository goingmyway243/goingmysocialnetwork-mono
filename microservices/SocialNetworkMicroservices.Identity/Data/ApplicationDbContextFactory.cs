using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SocialNetworkMicroservices.Identity.Data;

/// <summary>
/// Design-time factory for ApplicationDbContext to support EF Core migrations.
/// This is only used by EF Core tools (dotnet ef migrations), not at runtime.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Build configuration from appsettings files
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Get connection string from configuration
        var connectionString = configuration.GetConnectionString("goingmysocial-identity-db");
        
        /*if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'goingmysocial-identity-db' not found in appsettings.json. " +
                "Please ensure your appsettings.json contains a valid ConnectionStrings section.");
        }*/

        // Create DbContextOptions with Npgsql provider
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        
        // Optional: Enable sensitive data logging for development
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
