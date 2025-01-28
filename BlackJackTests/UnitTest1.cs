[Test]
public async Task Extract_ValidId_ShouldPopulateAllExistingFieldsCorrectly()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);
    var validId = "HA-011-05-001"; // Replace with an actual valid ID

    // Act
    var extracted = await source.Extract(validId);

    // Assert
    Assert.That(extracted, Is.Not.Null, "Extracted HallexRecord should not be null for a valid ID.");

    Assert.Multiple(() =>
    {
        Assert.That(extracted.PolicyNetObjectTypeCode, Is.EqualTo(PolicyNetObjectTypeCodes.HEARINGS_APPEALS_AND_LITIGATION_LAW_LEX_MANUAL), 
            "PolicyNetObjectTypeCode does not match.");
        
        Assert.That(extracted.Uname, Is.Not.Null.And.Not.Empty, "Uname should not be null or empty.");
        Assert.That(extracted.FullName, Is.Not.Null.And.Not.Empty, "FullName should not be null or empty.");
        Assert.That(extracted.SectionTitle, Is.Not.Null.And.Not.Empty, "SectionTitle should not be null or empty.");
        Assert.That(extracted.DisplaySection, Is.Not.Null.And.Not.Empty, "DisplaySection should not be null or empty.");
        Assert.That(extracted.PartNumber, Is.Not.Null.And.Not.Empty, "PartNumber should not be null or empty.");
        Assert.That(extracted.Chapter, Is.Not.Null.And.Not.Empty, "Chapter should not be null or empty.");
        Assert.That(extracted.SubChapter, Is.Not.Null.And.Not.Empty, "SubChapter should not be null or empty.");
        Assert.That(extracted.Section, Is.Not.Null.And.Not.Empty, "Section should not be null or empty.");
        Assert.That(extracted.LastUpdate, Is.GreaterThan(DateTime.MinValue), "LastUpdate should have a valid timestamp.");
        Assert.That(extracted.IsSensitive, Is.TypeOf<bool>(), "IsSensitive should be a boolean value.");
        Assert.That(extracted.Deleted, Is.TypeOf<bool>(), "Deleted should be a boolean value.");
        Assert.That(extracted.AppProfile, Is.Not.Null.And.Not.Empty, "AppProfile should not be null or empty.");
    });
}
