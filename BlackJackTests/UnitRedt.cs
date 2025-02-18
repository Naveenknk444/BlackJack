using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

class Program
{
    static void Main()
    {
        string xml = @"<RegReferences>
                        <Link TargetDoc='CFR' TargetRef='CFR-20-404-316-C'>404.316(c)</Link>
                        <Link TargetDoc='CFR' TargetRef='CFR-20-404-328'>404.328</Link>
                        <Link TargetDoc='CFR' TargetRef='CFR-20-404-337-C'>404.337(c)</Link>
                        <Link TargetDoc='CFR' TargetRef='CFR-20-404-352-D'>404.352(d)</Link>
                        <Link TargetDoc='CFR' TargetRef='CFR-20-404-902-S'>404.902(s)</Link>
                        <Link TargetDoc='CFR' TargetRef='CFR-20-404-1586-B'>404.1586(b)</Link>
                        <Link TargetDoc='CFR' TargetRef='CFR-20-416-1338'>416.1338</Link>
                        <Link TargetDoc='CFR' TargetRef='CFR-20-416-1402-J'>416.1402(j)</Link>
                      </RegReferences>";

        XDocument doc = XDocument.Parse(xml);
        
        // Extract all Link elements
        var links = doc.Descendants("Link")
                       .Select(link => new
                       {
                           TargetDoc = link.Attribute("TargetDoc")?.Value,
                           TargetRef = link.Attribute("TargetRef")?.Value,
                           Text = link.Value
                       })
                       .ToList();

        // Display results
        foreach (var link in links)
        {
            Console.WriteLine($"TargetDoc: {link.TargetDoc}, TargetRef: {link.TargetRef}, Text: {link.Text}");
        }
    }
}
