using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml;

class Program
{
    static void Main()
    {
        string xmlContent = @"YOUR_XML_STRING_HERE"; // Replace with actual XML content

        XmlReaderSettings settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Ignore // Ignores DTD to avoid entity errors
        };

        using (var reader = XmlReader.Create(new System.IO.StringReader(xmlContent), settings))
        {
            XDocument doc = XDocument.Load(reader);

            // Extract all <Link> elements
            var links = doc.Descendants("Link")
                           .Select(link => link.ToString())
                           .ToList();

            // Display results
            foreach (var link in links)
            {
                Console.WriteLine(link);
            }
        }
    }
}
