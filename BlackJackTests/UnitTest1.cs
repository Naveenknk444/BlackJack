[Test]
public async Task Extract_ValidId_ShouldReturnMockedHallexRecord()
{
    // Arrange
    var mockApiService = new Mock<IHallexApiService>();
    var validId = "HA-014-30-020";

    var mockRecord = new HallexRecord
    {
        filename = "HA-014-30-020.json",
        action = "version",
        @type = "section",
        lastUpdated = DateTime.Parse("2025-01-17T07:20:54Z"),
        PolicyNetObjectTypeCode = PolicyNetObjectTypeCodes.HEARINGS_APPEALS_AND_LITIGATION_LAW_LEX_MANUAL
    };

    // Mock API response
    mockApiService.Setup(api => api.FetchHallexRecord(validId))
                  .ReturnsAsync(mockRecord);

    var source = new HallexDataMigratorSource(mockApiService.Object);

    // Act
    var extracted = await source.Extract(validId);

    // Assert
    Assert.That(extracted, Is.Not.Null, "Extracted HallexRecord should not be null for a valid ID.");
    Assert.Multiple(() =>
    {
        Assert.That(extracted.filename, Is.EqualTo(mockRecord.filename), "Filename does not match.");
        Assert.That(extracted.action, Is.EqualTo(mockRecord.action), "Action does not match.");
        Assert.That(extracted.@type, Is.EqualTo(mockRecord.@type), "Type does not match.");
        Assert.That(extracted.lastUpdated, Is.EqualTo(mockRecord.lastUpdated), "LastUpdated does not match.");
        Assert.That(extracted.PolicyNetObjectTypeCode, Is.EqualTo(mockRecord.PolicyNetObjectTypeCode), "PolicyNetObjectTypeCode does not match.");
    });
}
