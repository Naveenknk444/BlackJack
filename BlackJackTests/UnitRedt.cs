public List<string> CheckRecordsInOracle(List<string> linkRefs)
{
    List<string> results = new List<string>();
    string query = $"SELECT * FROM YourTable WHERE LinkRef IN ('{string.Join("','", linkRefs)}')";

    using (var connection = new DBConnection().GetOracleConnection())
    {
        using (var command = new NpgsqlCommand(query, connection))
        {
            using (var reader = command. ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(reader["LinkRef"].ToString()); // Adjust based on your table schema
                }
            }
        }
    }
    return results;
}
