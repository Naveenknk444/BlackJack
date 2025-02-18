while (reader.Read())
                    {
                        string existingDoc = reader["clctn_nm"].ToString();
                        string existingRef = reader["ankr_num"].ToString();

                        Console.WriteLine($"Found in Oracle: {existingDoc}, {existingRef}"); // Debugging

                        // Update the flag if the record exists
                        var key = (existingDoc, existingRef);
                        if (linkXmlObject.ContainsKey(key))
                        {
                            linkXmlObject[key] = true; // Set flag to true
                            Console.WriteLine($"Flag updated: {existingDoc}, {existingRef} -> Exists: {linkXmlObject[key]}");
                        }
                    }
