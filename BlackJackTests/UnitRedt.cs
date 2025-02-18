static void UpdateLinkFlagsFromOracle(Dictionary<(string TargetDoc, string TargetRef), bool> linkXmlObject)
{
    using (var oracleConn = DBConnection.GetOracleConnection()) // Use existing DB connection method
    {
        oracleConn.Open();
        Console.WriteLine("Connected to Oracle successfully.");

        if (linkXmlObject.Count > 0)
        {
            // Construct batch SQL query with parameters
            string query = "SELECT clctn_nm, ankr_num FROM ppsowner.anchor WHERE " +
                           string.Join(" OR ", linkXmlObject.Keys.Select((key, i) => 
                               $"(UPPER(clctn_nm) = :targetDoc{i} AND UPPER(ankr_num) = :targetRef{i})"));

            using (var cmd = new OracleCommand(query, oracleConn))
            {
                int index = 0;
                foreach (var key in linkXmlObject.Keys)
                {
                    cmd.Parameters.Add(new OracleParameter($":targetDoc{index}", key.TargetDoc.ToUpper())); // Convert to upper case
                    cmd.Parameters.Add(new OracleParameter($":targetRef{index}", key.TargetRef.ToUpper())); // Convert to upper case
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
                        string existingDoc = reader["clctn_nm"].ToString().ToUpper(); // Convert DB result to upper case
                        string existingRef = reader["ankr_num"].ToString().ToUpper(); // Convert DB result to upper case

                        Console.WriteLine($"Found in Oracle: {existingDoc}, {existingRef}"); // Debugging

                        // Convert to uppercase before checking the dictionary
                        var key = (existingDoc, existingRef);
                        if (linkXmlObject.ContainsKey(key))
                        {
                            linkXmlObject[key] = true; // Set flag to true
                            Console.WriteLine($"Flag updated: {existingDoc}, {existingRef} -> Exists: {linkXmlObject[key]}");
                        }
                    }
                }
            }
        }
    }
}
