static string GenerateOracleQuery(Dictionary<(string TargetDoc, string TargetRef), bool> linkXmlObject)
{
    if (linkXmlObject.Count == 0)
    {
        return string.Empty; // No data to query
    }

    // Construct WHERE conditions dynamically
    string query = "SELECT clctn_nm, ankr_num FROM anchor WHERE " +
                   string.Join(" OR ", linkXmlObject.Keys.Select((key, i) => $"(clctn_nm = :targetDoc{i} AND ankr_num = :targetRef{i})"));

    return query;
}
