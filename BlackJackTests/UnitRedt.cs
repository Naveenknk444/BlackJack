// Load XML into XmlDocument
var xDoc = new XmlDocument();
xDoc.PreserveWhitespace = true;
xDoc.LoadXml(xmlStringReplaced); // Keep this as it is

// Convert XmlDocument to XDocument for LINQ operations
XDocument xDocument;
using (var reader = new XmlNodeReader(xDoc))
{
    xDocument = XDocument.Load(reader);
}

// Extract all <Link> elements from <RegReferences>
var linksDoc = xDocument.Descendants("Link")
                        .Select(link => new
                        {
                            TargetDoc = link.Attribute("TargetDoc")?.Value,
                            TargetRef = link.Attribute("TargetRef")?.Value,
                            Text = link.Value
                        })
                        .ToList();

// If you need to log or process these links further, you can loop through them
foreach (var link in linksDoc)
{
    Console.WriteLine($"TargetDoc: {link.TargetDoc}, TargetRef: {link.TargetRef}, Text: {link.Text}");
}

// Now, continue with the existing `ResolveEntities` logic as needed
