[Test]
public async Task Extract_ValidId_ShouldPopulateAllFieldsCorrectly()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);
    var validId = "HA-011-05-001"; // Use a valid test ID

    // Act
    var extracted = await source.Extract(validId);

    // Assert
    Assert.That(extracted, Is.Not.Null, "Extracted HallexRecord should not be null for a valid ID.");

    Assert.Multiple(() =>
    {
        Assert.That(extracted.Id, Is.EqualTo(validId), "ID does not match.");
        Assert.That(extracted.Filename, Is.EqualTo("HA-011-05-001.json"), "Filename does not match.");
        Assert.That(extracted.Type, Is.EqualTo("section"), "Type does not match.");
        Assert.That(extracted.Action, Is.EqualTo("version"), "Action does not match.");
        Assert.That(extracted.LastUpdated, Is.EqualTo(DateTime.Parse("2025/01/17 07:20:54")), "LastUpdated does not match.");
        Assert.That(extracted.FullName, Is.EqualTo("Full Name 1"), "FullName does not match.");
        Assert.That(extracted.SectionTitle, Is.EqualTo("Title 1"), "SectionTitle does not match.");
        Assert.That(extracted.DisplaySection, Is.EqualTo("Display 1"), "DisplaySection does not match.");
        Assert.That(extracted.IsSensitive, Is.EqualTo(false), "IsSensitive flag does not match.");
    });
}
