using BlackjackDataAccess.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        // Optionally seed the database when the application starts
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                // Initialize the DbContext for database access
                var context = services.GetRequiredService<ApplicationDbContext>();

                // Optional: Add database seeding logic here (if needed)
                // SeedData.Initialize(services); // Uncomment if you have a SeedData class
            }
            catch (Exception ex)
            {
                // Log the error if something goes wrong during database initialization
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during database seeding.");
            }
        }

        // Run the application
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                // Use Startup.cs to configure the services and middleware pipeline
                webBuilder.UseStartup<Startup>();
            });
}
