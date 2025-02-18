static void UpdateLinkFlagsFromOracle(Dictionary<(string TargetDoc, string TargetRef), bool> linkXmlObject)
{
    using (var oracleConn = DBConnection.GetOracleConnection()) // Ensure this returns OracleConnection
    {
        oracleConn.Open();
        Console.WriteLine("Connected to Oracle successfully.");

        if (linkXmlObject.Count > 0)
        {
            // Construct batch SQL query with parameters
            string query = "SELECT clctn_nm, ankr_num FROM anchor WHERE " +
                           string.Join(" OR ", linkXmlObject.Keys.Select((key, i) => $"(clctn_nm = :targetDoc{i} AND ankr_num = :targetRef{i})"));

            using (var cmd = new OracleCommand(query, oracleConn))
            {
                int index = 0;
                foreach (var key in linkXmlObject.Keys)
                {
                    cmd.Parameters.Add(new OracleParameter($":targetDoc{index}", key.TargetDoc)); // Assigning actual value
                    cmd.Parameters.Add(new OracleParameter($":targetRef{index}", key.TargetRef)); // Assigning actual value
                    index++;
                }

                Console.WriteLine("Generated Query: " + query); // Debugging

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No matching records found in Oracle.");
                    }

                    while (reader.Read())
                    {
                        string existingDoc = reader["clctn_nm"].ToString();
                        string existingRef = reader["ankr_num"].ToString();

                        Console.WriteLine($"Found in Oracle: {existingDoc}, {existingRef}"); // Debugging

                        if (linkXmlObject.ContainsKey((existingDoc, existingRef)))
                        {
                            linkXmlObject[(existingDoc, existingRef)] = true; // Update flag if found in Oracle
                        }
                    }
                }
            }
        }
    }
}
