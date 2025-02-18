static void UpdateLinkFlagsFromOracle(Dictionary<(string TargetDoc, string TargetRef), bool> linkXmlObject)
{
    using (var oracleConn = DBConnection.GetOracleConnection()) // Use existing DB connection method
    {
        oracleConn.Open();

        if (linkXmlObject.Count > 0)
        {
            // Construct SQL query using IN clause for batch processing
            string query = "SELECT UPPER(clctn_nm) AS clctn_nm, UPPER(ankr_num) AS ankr_num FROM ppsowner.anchor " +
                           "WHERE UPPER(clctn_nm) IN (" + 
                           string.Join(",", linkXmlObject.Keys.Select((key, i) => $":targetDoc{i}")) + ") " +
                           "AND UPPER(ankr_num) IN (" +
                           string.Join(",", linkXmlObject.Keys.Select((key, i) => $":targetRef{i}")) + ")";

            using (var cmd = new OracleCommand(query, oracleConn))
            {
                int index = 0;
                foreach (var key in linkXmlObject.Keys)
                {
                    cmd.Parameters.Add(new OracleParameter($":targetDoc{index}", key.TargetDoc.ToUpper())); // Convert to uppercase
                    cmd.Parameters.Add(new OracleParameter($":targetRef{index}", key.TargetRef.ToUpper())); // Convert to uppercase
                    index++;
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string existingDoc = reader["clctn_nm"].ToString().ToUpper(); // Ensure case consistency
                        string existingRef = reader["ankr_num"].ToString().ToUpper(); // Ensure case consistency

                        var key = (existingDoc, existingRef);
                        if (linkXmlObject.ContainsKey(key))
                        {
                            linkXmlObject[key] = true; // Set flag to true
                        }
                    }
                }
            }
        }
    }
}
