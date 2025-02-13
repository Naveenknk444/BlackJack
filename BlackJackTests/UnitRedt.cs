private List<bool> CheckRecordsInOracle(List<string> linkRefs)
{
    var exists = new List<bool>();
    string connectionString = ConfigurationManager.ConnectionStrings["OracleDb"].ConnectionString;

    using (OracleConnection conn = new OracleConnection(connectionString))
    {
        conn.Open();
        foreach (var ref in linkRefs)
        {
            using (OracleCommand cmd = new OracleCommand("SELECT COUNT(1) FROM YourTable WHERE YourColumn = :ref", conn))
            {
                cmd.Parameters.Add(new OracleParameter("ref", ref));
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                exists.Add(count > 0);
            }
        }
    }
    return exists;
}
