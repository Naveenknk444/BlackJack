[Test]
public async Task Extract_ValidId_HA_014_30_021_ShouldReturnHallexRecord()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);
    var validId = "HA-014-30-021"; // Different test ID

    // Act
    var extracted = await source.Extract(validId);

    // Assert
    Assert.That(extracted, Is.Not.Null, "Extracted HallexRecord should not be null for a valid ID.");
    Assert.Multiple(() =>
    {
        Assert.That(extracted.filename, Is.Not.Null.And.Not.Empty, "Filename should not be null or empty.");
        Assert.That(extracted.action, Is.EqualTo("version"), "Action should match expected value.");
        Assert.That(extracted.@type, Is.EqualTo("section"), "Type should match expected value.");
        Assert.That(extracted.lastUpdated, Is.GreaterThan(DateTime.MinValue), "LastUpdated should be a valid timestamp.");
    });
}
