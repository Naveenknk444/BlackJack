[Test]
public async Task ExtractAll_ShouldReturnValidHallexRecords_Efficiently()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);
    var stopwatch = new System.Diagnostics.Stopwatch();
    
    // Act
    stopwatch.Start();
    var extractedRecords = await source.ExtractAll(CancellationToken.None);
    stopwatch.Stop();

    // Assert
    Assert.That(extractedRecords, Is.Not.Null, "Extracted records should not be null.");
    Assert.That(extractedRecords.Count(), Is.GreaterThan(0), "Extracted records list should not be empty.");

    // Limit execution time to 5 seconds (adjust based on actual performance)
    Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(5000), "ExtractAll took too long to execute.");

    // Validate data integrity
    Assert.Multiple(() =>
    {
        foreach (var record in extractedRecords.Take(10)) // Validate only the first 10 records to reduce test time
        {
            Assert.That(record.filename, Is.Not.Null.And.Not.Empty, "Filename should not be null or empty.");
            Assert.That(record.action, Is.EqualTo("version"), "Action should match expected value.");
            Assert.That(record.@type, Is.EqualTo("section"), "Type should match expected value.");
            Assert.That(record.lastUpdated, Is.GreaterThan(DateTime.MinValue), "LastUpdated should be a valid timestamp.");
        }
    });
}
