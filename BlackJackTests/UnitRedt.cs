using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

class Program
{
    static void Main()
    {
        string connectionString = "Your_Oracle_Connection_String"; // Replace with your actual connection string

        string query = "SELECT * FROM anchor WHERE clctn_nm = 'CFR' AND ankr_num LIKE 'CFR-20-404-1597-A'";

        try
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Oracle Connection Successful!");

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Query executed successfully. Results:");

                        DataTable schemaTable = reader.GetSchemaTable();
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.WriteLine($"{schemaTable.Rows[i]["ColumnName"]}: {reader[i]}");
                            }
                            Console.WriteLine("--------------------------------------------------");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
