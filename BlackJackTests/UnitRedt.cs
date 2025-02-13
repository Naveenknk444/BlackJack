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
            links[targetDoc] = (targetRef, true); // set the boolean value as needed
        }
    }

    return links;
}
