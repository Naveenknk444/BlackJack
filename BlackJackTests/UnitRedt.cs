using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml;

class Program
{
    static void Main()
    {
        string xmlInput = @"YOUR_XML_STRING_HERE"; // Replace with actual XML content

        XmlReaderSettings settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Ignore // Ignores DTD processing to avoid undeclared entity errors
        };

        using (var reader = XmlReader.Create(new System.IO.StringReader(xmlInput), settings))
        {
            XDocument doc = XDocument.Load(reader);

            // Extract <RegReferences> tag
            var regReferences = doc.Descendants("RegReferences").FirstOrDefault();
            
            if (regReferences != null)
            {
                // Extract all <Link> elements inside <RegReferences>
                var links = regReferences.Descendants("Link")
                                         .Select(link => link.ToString())
                                         .ToList();

                // Display extracted <Link> elements
                foreach (var link in links)
                {
                    Console.WriteLine(link);
                }
            }
            else
            {
                Console.WriteLine("No <RegReferences> tag found in the XML.");
            }
        }
    }
}
