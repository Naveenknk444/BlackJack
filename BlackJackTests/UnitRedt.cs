static string GetRawOracleQuery(Dictionary<(string TargetDoc, string TargetRef), bool> linkXmlObject)
{
    if (linkXmlObject.Count == 0)
        return string.Empty;

    // Construct WHERE conditions dynamically for each (TargetDoc, TargetRef) pair
    string query = "SELECT UPPER(clctn_nm) AS clctn_nm, UPPER(ankr_num) AS ankr_num FROM ppsowner.anchor WHERE ";

    query += string.Join(" OR ", linkXmlObject.Keys.Select(key => 
        $"(UPPER(clctn_nm) = '{key.TargetDoc.ToUpper()}' AND UPPER(ankr_num) = '{key.TargetRef.ToUpper()}')"
    ));

    return query;
}
