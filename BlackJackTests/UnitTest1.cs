[Test]
public async Task Extract_ValidId_ShouldReturnHallexRecord()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);
    var validId = HallexIds.Prod.Active.I105_ComponentRoles; // Replace with an actual valid ID

    // Act
    var extracted = await source.Extract(validId);

    // Assert
    Assert.IsNotNull(extracted, "Extracted HallexRecord should not be null for a valid ID.");
    Assert.Multiple(() =>
    {
        Assert.That(extracted.LinkId, Is.EqualTo(validId), "LinkId does not match the expected value.");
        Assert.That(extracted.FullName, Is.EqualTo("null"), "FullName is not as expected.");
        Assert.That(extracted.Deleted, Is.EqualTo(false), "Deleted flag mismatch.");
        Assert.That(extracted.AppProfile, Is.EqualTo("HALLEX"), "AppProfile mismatch.");
        Assert.That(extracted.DisplaySection, Is.EqualTo("I 10.5"), "DisplaySection mismatch.");
    });
}
