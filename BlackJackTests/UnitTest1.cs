
[Test]
public async Task Extract_ValidId_ShouldReturnHallexRecordNew()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);
    var validId = "HA-011-05-001"; // Replace with an actual valid ID

    // Act
    var extracted = await source.Extract(validId);

    // Assert
    Assert.That(extracted, Is.Not.Null, "Extracted HallexRecordNew should not be null for a valid ID.");

    Assert.Multiple(() =>
    {
        Assert.That(extracted.lastUpdated, Is.GreaterThan(DateTime.MinValue), "LastUpdated should be a valid timestamp.");
        Assert.That(extracted.filename, Is.Not.Null.And.Not.Empty, "Filename should not be null or empty.");
        Assert.That(extracted.action, Is.Not.Null.And.Not.Empty, "Action should not be null or empty.");
        Assert.That(extracted.Id, Is.Not.Null.And.Not.Empty, "ID should not be null or empty.");
        Assert.That(extracted.type, Is.Not.Null.And.Not.Empty, "Type should not be null or empty.");
        Assert.That(extracted.PolicyNetObjectTypeCode, Is.EqualTo(PolicyNetObjectTypeCodes.HEARINGS_APPEALS_AND_LITIGATION_LAW_LEX_MANUAL),
            "PolicyNetObjectTypeCode does not match expected value.");
        Assert.That(extracted.CommonId, Is.EqualTo(validId), "CommonId does not match expected ID.");
    });
}
