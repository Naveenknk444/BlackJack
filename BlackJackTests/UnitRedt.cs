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
