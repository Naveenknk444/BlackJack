public class DBConnection
{
    private static string oracleConnectionString = ConfigurationManager.ConnectionStrings["OracleDBContext"].ConnectionString;

    public NpgsqlConnection GetOracleConnection()
    {
        var connection = new NpgsqlConnection(oracleConnectionString);
        connection.Open();
        return connection;
    }
}

