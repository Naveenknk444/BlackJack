using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

class Program
{
    static void Main()
    {
        // Load configuration (appsettings.json)
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Get the Oracle connection string (same way as the existing app)
        string ppsConnectionString = GetOracleConnectionString(configuration);

        Console.WriteLine($"Using Connection String: {ppsConnectionString}");

        if (string.IsNullOrEmpty(ppsConnectionString))
        {
            Console.WriteLine("Error: No connection string found for 'pps'.");
            return;
        }

        // Test the Oracle connection
        TestOracleConnection(ppsConnectionString);
    }

    static string GetOracleConnectionString(IConfiguration configuration)
    {
        return configuration.GetConnectionString("pps"); // Fetch connection string for "pps"
    }

    static void TestOracleConnection(string connectionString)
    {
        try
        {
            using (var connection = new OracleConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("✅ Successfully connected to Oracle Database!");

                // Run a test query
                string testQuery = "SELECT COUNT(*) FROM anchor"; // Change this to a valid table
                using (var command = new OracleCommand(testQuery, connection))
                {
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Total records in anchor table: {count}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error connecting to Oracle: {ex.Message}");
        }
    }
}
