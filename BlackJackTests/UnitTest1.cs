[Test]
public async Task Extract_LargeDataset_ShouldExtractAllRecords()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);

    // Sample data based on HallexRecord structure
    var sampleData = new Dictionary<string, HallexRecord>
    {
        { "HA-011-05-001", new HallexRecord { 
            Id = "HA-011-05-001", 
            Filename = "HA-011-05-001.json", 
            Type = "section", 
            Action = "version", 
            LastUpdated = DateTime.Parse("2025/01/17 07:20:54"), 
            FullName = "Full Name 1",
            SectionTitle = "Title 1",
            DisplaySection = "Display 1",
            IsSensitive = false
        }},
        { "HA-011-05-002", new HallexRecord { 
            Id = "HA-011-05-002", 
            Filename = "HA-011-05-002.json", 
            Type = "section", 
            Action = "version", 
            LastUpdated = DateTime.Parse("2025/01/17 07:20:54"), 
            FullName = "Full Name 2",
            SectionTitle = "Title 2",
            DisplaySection = "Display 2",
            IsSensitive = true
        }},
        { "HA-011-05-003", new HallexRecord { 
            Id = "HA-011-05-003", 
            Filename = "HA-011-05-003.json", 
            Type = "section", 
            Action = "version", 
            LastUpdated = DateTime.Parse("2025/01/17 07:20:54"), 
            FullName = "Full Name 3",
            SectionTitle = "Title 3",
            DisplaySection = "Display 3",
            IsSensitive = false
        }}
    };

    var extractedResults = new List<HallexRecord>();

    // Act - Call the Extract method for each sample record
    foreach (var key in sampleData.Keys)
    {
        var record = await source.Extract(key);
        if (record != null)
            extractedResults.Add(record);
    }

    // Assert
    Assert.That(extractedResults.Count, Is.EqualTo(sampleData.Count), "The number of extracted records does not match the sample data count.");
    
    Assert.Multiple(() =>
    {
        foreach (var record in extractedResults)
        {
            Assert.That(record, Is.Not.Null, "Extracted record is null.");
            Assert.That(record.Id, Does.StartWith("HA-011-05"), "Extracted ID does not match the expected format.");
            Assert.That(record.Filename, Does.EndWith(".json"), "Filename format is incorrect.");
            Assert.That(record.Type, Is.EqualTo("section"), "Record type mismatch.");
            Assert.That(record.Action, Is.EqualTo("version"), "Record action mismatch.");
            Assert.That(record.LastUpdated, Is.Not.EqualTo(default(DateTime)), "LastUpdated is not correctly set.");
            Assert.That(record.FullName, Does.StartWith("Full Name"), "FullName mismatch.");
            Assert.That(record.SectionTitle, Does.StartWith("Title"), "SectionTitle mismatch.");
            Assert.That(record.DisplaySection, Does.StartWith("Display"), "DisplaySection mismatch.");
            Assert.That(record.IsSensitive, Is.TypeOf<bool>(), "IsSensitive property should be boolean.");
        }
    });
}
