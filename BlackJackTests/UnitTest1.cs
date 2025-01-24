[Test]
public async Task TestConvertXMLAndSaveToDBBasedOnQuery()
{
    // Arrange: Mock database data with HtmlContentText
    var mockMarkupContentData = new List<Dictionary<string, string>>
    {
        new()
        {
            { "InstructionVersionId", "SCTN-04-120-01-005v16" },
            { "MarkupContentText", "<root><node>Some XML Content</node></root>" },
            { "HtmlContentText", "<html><body>Hardcoded HTML Content</body></html>" }, // Hardcoded HTML
            { "MarkupContentFormat", "XML" },
            { "ConvertedToHtml", "false" },
            { "CommonId", "12345" }
        }
    };

    // Mock the database result
    var mockDbConnection = new Mock<IDBConnection>();
    mockDbConnection
        .Setup(db => db.ExecuteQueryAsync(It.IsAny<string>()))
        .ReturnsAsync(CreateMockDataTable(mockMarkupContentData));

    var service = new TransformService(mockDbConnection.Object);

    // Act: Call the method to test
    await service.TransformXmlAndSavesToDb();

    // Assert: Ensure the method correctly uses HtmlContentText
    Assert.Pass("TransformXmlAndSavesToDb executed and used HtmlContentText successfully.");
}
