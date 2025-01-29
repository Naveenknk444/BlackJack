[Test]
public async Task ExtractAll_ShouldReturnAllRecords()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);

    // Act
    var extractedRecords = await source.ExtractAll(CancellationToken.None);

    // Assert
    Assert.That(extractedRecords, Is.Not.Null, "Extracted records list should not be null.");
    Assert.That(extractedRecords.Count(), Is.GreaterThan(0), "Extracted records list should not be empty.");
}
