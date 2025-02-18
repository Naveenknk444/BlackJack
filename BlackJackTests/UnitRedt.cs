// Step 1: Extract all <Link> elements and store them in a Dictionary with both TargetDoc and TargetRef
var linkXmlObject = xDocument.Descendants("Link")
    .Select(link => new
    {
        TargetDoc = link.Attribute("TargetDoc")?.Value,
        TargetRef = link.Attribute("TargetRef")?.Value
    })
    .Where(x => !string.IsNullOrEmpty(x.TargetDoc) && !string.IsNullOrEmpty(x.TargetRef))
    .Distinct()
    .ToDictionary(x => (x.TargetDoc, x.TargetRef), x => false); // Store as a tuple (TargetDoc, TargetRef)

// Step 2: Check these records in Oracle and update the flag
UpdateLinkFlagsFromOracle(linkXmlObject);

// Step 3: Display the final dictionary (for debugging/logging)
foreach (var entry in linkXmlObject)
{
    Console.WriteLine($"TargetDoc: {entry.Key.TargetDoc}, TargetRef: {entry.Key.TargetRef}, Exists in Oracle: {entry.Value}");
}
