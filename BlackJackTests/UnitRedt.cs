using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

class Program
{
    static void Main()
    {
        string xmlStringReplaced = @"YOUR_XML_STRING_HERE"; // Replace with actual XML

        XmlDocument xDoc = new XmlDocument();
        xDoc.PreserveWhitespace = true;
        xDoc.LoadXml(xmlStringReplaced);

        // Convert XmlDocument to XDocument
        XDocument xDocument;
        using (var reader = new XmlNodeReader(xDoc))
        {
            xDocument = XDocument.Load(reader);
        }

        // Step 1: Extract all <Link> elements and store them in a Dictionary
        var linkXmlObject = xDocument.Descendants("Link")
                                     .Select(link => link.Attribute("TargetRef")?.Value)
                                     .Where(targetRef => !string.IsNullOrEmpty(targetRef))
                                     .Distinct()
                                     .ToDictionary(targetRef => targetRef, targetRef => false); // Initially set to false

        // Step 2: Check these records in Oracle and update the flag
        UpdateLinkFlagsFromOracle(linkXmlObject);

        // Step 3: Display the final dictionary (for debugging/logging)
        foreach (var entry in linkXmlObject)
        {
            Console.WriteLine($"TargetRef: {entry.Key}, Exists in Oracle: {entry.Value}");
        }
    }

    static void UpdateLinkFlagsFromOracle(Dictionary<string, bool> linkXmlObject)
    {
        string oracleConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PolicyNetOracleDbContext"].ConnectionString;

        using (OracleConnection conn = new OracleConnection(oracleConnectionString))
        {
            conn.Open();
            
            foreach (var targetRef in linkXmlObject.Keys.ToList()) // Iterate over keys
            {
                string query = $"SELECT COUNT(*) FROM YOUR_ORACLE_TABLE WHERE TargetRef = :targetRef";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("targetRef", targetRef));

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    linkXmlObject[targetRef] = count > 0; // Set flag to true if record exists
                }
            }
        }
    }
}
