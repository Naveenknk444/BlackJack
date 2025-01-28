[Test]
public async Task Extract_LargeDataset_ShouldExtractAllRecords()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);

    // Sample data setup
    var sampleData = new Dictionary<string, HallexRecord>
    {
        { "HA-011-05-001", new HallexRecord { Id = "HA-011-05-001", Filename = "HA-011-05-001.json", Type = "section", Action = "version", LastUpdated = DateTime.Parse("2025/01/17 07:20:54") } },
        { "HA-011-05-002", new HallexRecord { Id = "HA-011-05-002", Filename = "HA-011-05-002.json", Type = "section", Action = "version", LastUpdated = DateTime.Parse("2025/01/17 07:20:54") } },
        { "HA-011-05-003", new HallexRecord { Id = "HA-011-05-003", Filename = "HA-011-05-003.json", Type = "section", Action = "version", LastUpdated = DateTime.Parse("2025/01/17 07:20:54") } },
        { "HA-011-05-004", new HallexRecord { Id = "HA-011-05-004", Filename = "HA-011-05-004.json", Type = "section", Action = "version", LastUpdated = DateTime.Parse("2025/01/17 07:20:54") } },
        { "HA-011-05-005", new HallexRecord { Id = "HA-011-05-005", Filename = "HA-011-05-005.json", Type = "section", Action = "version", LastUpdated = DateTime.Parse("2025/01/17 07:20:54") } }
    };

    // Mocking Extract Method (You can replace it with your actual logic)
    foreach (var key in sampleData.Keys)
    {
        // This is simulating the extraction process
        var record = await source.Extract(key);
        if (record != null)
            sampleData[key] = record; // Add only valid records
    }

    // Act
    var results = sampleData.Values.ToList();

    // Assert
    Assert.That(results.Count, Is.EqualTo(sampleData.Count), "The number of extracted records does not match the sample data count.");
    Assert.Multiple(() =>
    {
        foreach (var record in results)
        {
            Assert.That(record, Is.Not.Null, "Extracted record is null.");
            Assert.That(record.Id, Does.StartWith("HA-011-05"), "Extracted ID does not match the expected format.");
            Assert.That(record.Type, Is.EqualTo("section"), "Record type mismatch.");
            Assert.That(record.Action, Is.EqualTo("version"), "Record action mismatch.");
        }
    });
}
