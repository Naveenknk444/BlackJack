static void UpdateXmlFileBasedOnFlags(XDocument xDocument, Dictionary<(string TargetDoc, string TargetRef), bool> linkXmlObject)
{
    // Get all <Link> elements in the XML
    var links = xDocument.Descendants("Link").ToList();

    foreach (var link in links)
    {
        string targetDoc = link.Attribute("TargetDoc")?.Value;
        string targetRef = link.Attribute("TargetRef")?.Value?.ToUpper(); // Ensure case-insensitivity

        if (!string.IsNullOrEmpty(targetDoc) && !string.IsNullOrEmpty(targetRef))
        {
            var key = (targetDoc, targetRef);

            // If the flag is false, remove attributes and keep only the inner text
            if (linkXmlObject.ContainsKey(key) && !linkXmlObject[key])
            {
                Console.WriteLine($"Removing attributes for: {targetDoc}, {targetRef}");

                link.Attributes().Remove(); // Remove all attributes
            }
        }
    }
}
