private Dictionary<string, (string TargetRef, bool isLink)> GetLinksFromXML(string xmlContent)
{
    var doc = XDocument.Parse(xmlContent);
    var links = new Dictionary<string, (string, bool)>();

    foreach (var element in doc.Descendants().Where(e => e.Attribute("link") != null))
    {
        var targetDoc = element.Attribute("targetDoc")?.Value;
        var targetRef = element.Attribute("targetRef")?.Value;

        if (!string.IsNullOrEmpty(targetDoc) && !string.IsNullOrEmpty(targetRef))
        {
            // Check if the record exists in the database
            bool exists = CheckRecordInDatabase(targetDoc, targetRef);

            // Add to dictionary
            links[targetDoc] = (targetRef, exists);
        }
    }

    return links;
}

// Method to check if the record exists in the database
private bool CheckRecordInDatabase(string targetDoc, string targetRef)
{
    // Implement database check logic here
    // Example: query the database to see if the combination exists
    // Return true if exists, false otherwise
    return false;
}
