[Test]
public async Task Extract_HA_014_30_020_ShouldReturnValidHallexRecord()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);
    var validId = HallexIds.Prod.Active.DeclarationsOther; // Ensure this ID exists in test data
    bool saveHtmlFileToResourcesFolder = true; 

    // Act
    var extracted = await source.Extract(validId, saveHtmlFileToResourcesFolder);

    // Assert
    Assert.That(extracted, Is.Not.Null, "Extracted HallexRecord should not be null for a valid ID.");

    Assert.Multiple(() =>
    {
        Assert.That(extracted.LinkId, Is.EqualTo(validId), "LinkId does not match the expected value.");
        
        Assert.That(extracted.FullName, Is.Not.Null.And.Not.Empty, "FullName should not be null or empty.");
        Assert.That(extracted.Uname, Is.Not.Null.And.Not.Empty, "Uname should not be null or empty.");
        Assert.That(extracted.Deleted, Is.False, "Deleted flag mismatch.");
        Assert.That(extracted.PartText, Is.Not.Null.And.Not.Empty, "PartText should not be null or empty.");
        Assert.That(extracted.AppProfile, Is.EqualTo("HALLEX"), "AppProfile mismatch.");
        Assert.That(extracted.IsSensitive, Is.False, "IsSensitive flag mismatch.");

        Assert.That(extracted.SectionTitle, Is.Not.Null.And.Not.Empty, "SectionTitle should not be null or empty.");
        Assert.That(extracted.DisplaySection, Is.Not.Null.And.Not.Empty, "DisplaySection should not be null or empty.");
        Assert.That(extracted.PartNumber, Is.Not.Null.And.Not.Empty, "PartNumber should not be null or empty.");
        Assert.That(extracted.Chapter, Is.Not.Null.And.Not.Empty, "Chapter should not be null or empty.");
        Assert.That(extracted.SubChapter, Is.Not.Null.And.Not.Empty, "SubChapter should not be null or empty.");
        Assert.That(extracted.Section, Is.Not.Null.And.Not.Empty, "Section should not be null or empty.");
        Assert.That(extracted.LastUpdate, Is.GreaterThan(DateTime.MinValue), "LastUpdate should have a valid timestamp.");
    });
}
