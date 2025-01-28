[Test]
public async Task Extract_InvalidId_ShouldReturnNull()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);
    var invalidId = "InvalidId"; // A dummy ID that does not exist.

    // Act
    var extracted = await source.Extract(invalidId);

    // Assert
    Assert.That(extracted, Is.Null, "Extract should return null for an invalid ID.");
}
