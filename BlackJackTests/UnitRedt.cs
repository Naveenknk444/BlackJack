public async Task<string> GetUpdatedXMLContentAsync(string xmlContent)
{
    // Step 1: Extract XML and links
    var doc = ExtractLinksFromXML(xmlContent, out var linkPairs);

    // Step 2: Fetch matching records from database
    var anchorSet = await FetchExistingAnchorsAsync(linkPairs);

    // Step 3: Update XML content based on database results
    UpdateXMLContent(doc, linkPairs, anchorSet);

    // Step 4: Return modified XML as a string
    return doc.ToString();
}


private XDocument ExtractLinksFromXML(string xmlContent, out List<(XElement Element, string TargetDoc, string TargetRef)> linkPairs)
{
    var doc = XDocument.Parse(xmlContent);

    linkPairs = doc.Descendants()
        .Where(e => e.Attribute("link") != null)
        .Select(e => new 
        { 
            Element = e,
            TargetDoc = e.Attribute("targetDoc")?.Value, 
            TargetRef = e.Attribute("targetRef")?.Value 
        })
        .Where(e => !string.IsNullOrEmpty(e.TargetDoc) && !string.IsNullOrEmpty(e.TargetRef))
        .Distinct()
        .Select(e => (e.Element, e.TargetDoc, e.TargetRef))
        .ToList();

    return doc; // Returns the XML document instead of a list
}
private async Task<HashSet<(string TargetDoc, string TargetRef)>> FetchExistingAnchorsAsync(List<(XElement Element, string TargetDoc, string TargetRef)> linkPairs)
{
    if (!linkPairs.Any())
        return new HashSet<(string, string)>();

    var targetDocs = linkPairs.Select(e => e.TargetDoc).ToHashSet();
    var targetRefs = linkPairs.Select(e => e.TargetRef).ToHashSet();

    using var db = SourceDbContextFactory.CreateDbContext();
    var existingAnchors = await db.Anchor
        .Where(a => targetDocs.Contains(a.ClcthNm) && targetRefs.Contains(a.AnkrNum))
        .Select(a => new { a.ClcthNm, a.AnkrNum })
        .ToListAsync();

    return new HashSet<(string, string)>(existingAnchors.Select(a => (a.ClcthNm, a.AnkrNum)));
}
private void UpdateXMLContent(XDocument doc, List<(XElement Element, string TargetDoc, string TargetRef)> linkPairs, HashSet<(string TargetDoc, string TargetRef)> anchorSet)
{
    foreach (var pair in linkPairs)
    {
        bool exists = anchorSet.Contains((pair.TargetDoc, pair.TargetRef));

        if (!exists)
        {
            pair.Element.SetAttributeValue("link", null); // Remove the link attribute
        }
    }
}

