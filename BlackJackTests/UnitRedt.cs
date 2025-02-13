public bool CheckRecordsInOracle(string query)
{
    using (var connection = new DBConnection().GetOracleConnection())
    {
        using (var command = new NpgsqlCommand(query, connection))
        {
            using (var reader = command.ExecuteReader())
            {
                return reader.Read(); // True if records are found
            }
        }
    }
}
