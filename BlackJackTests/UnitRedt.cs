static void UpdateLinkFlagsFromOracle(Dictionary<string, bool> linkXmlObject)
{
    using (var oracleConn = DBConnection.GetOracleConnection()) // Use existing DB connection method
    {
        oracleConn.Open();

        if (linkXmlObject.Count > 0)
        {
            // Construct batch SQL query with multiple TargetRef values
            string query = "SELECT TargetRef FROM YOUR_ORACLE_TABLE WHERE TargetRef IN (" +
                           string.Join(",", linkXmlObject.Keys.Select((_, i) => $":targetRef{i}")) + ")";

            using (var cmd = new OracleCommand(query, oracleConn))
            {
                // Add parameters dynamically
                int index = 0;
                foreach (var targetRef in linkXmlObject.Keys)
                {
                    cmd.Parameters.Add(new OracleParameter($"targetRef{index++}", targetRef));
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string existingRef = reader["TargetRef"].ToString();
                        if (linkXmlObject.ContainsKey(existingRef))
                        {
                            linkXmlObject[existingRef] = true; // Update flag if found in Oracle
                        }
                    }
                }
            }
        }
    }
}
