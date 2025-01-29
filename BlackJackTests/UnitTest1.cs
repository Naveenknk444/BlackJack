[Test]
public async Task ExtractAll_ShouldReturnValidFilenames()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);

    // Act
    var extractedRecords = await source.ExtractAll(CancellationToken.None);

    // Assert
    Assert.Multiple(() =>
    {
        foreach (var record in extractedRecords.Take(10)) // Check only first 10 records to optimize performance
        {
            Assert.That(record.filename, Does.EndWith(".json"), $"Filename {record.filename} does not have .json extension.");
        }
    });
}
