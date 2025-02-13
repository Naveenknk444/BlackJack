using System.Xml.Linq;
using System.Collections.Generic;

private Dictionary<string, string> GetLinksFromXML(string xmlContent)
{
    var doc = XDocument.Parse(xmlContent);
    var links = new Dictionary<string, string>();

    foreach (var element in doc.Descendants().Where(e => e.Attribute("link") != null))
    {
        var targetDoc = element.Attribute("targetDoc")?.Value;
        var targetRef = element.Attribute("targetRef")?.Value;

        if (!string.IsNullOrEmpty(targetDoc) && !string.IsNullOrEmpty(targetRef))
        {
            links[targetDoc] = targetRef;
        }
    }

    return links;
}
