[Test]
public void Extract_NullId_ShouldThrowArgumentException()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);

    // Act & Assert
    var ex = Assert.ThrowsAsync<ArgumentException>(async () => 
        await source.Extract(null));

    Assert.That(ex.Message, Does.Contain("ID cannot be null"), "Exception message does not indicate null ID issue.");
}
