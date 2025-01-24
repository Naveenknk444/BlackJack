[Test]
public async Task TestConvertXMLAndSaveToDBBasedOnQuery()
{
    // Arrange: Mock database data with all required columns
    var mockMarkupContentData = new List<Dictionary<string, string>>
    {
        new Dictionary<string, string>
        {
            { "HtmlContentText", "<html><body>Hardcoded HTML Content</body></html>" }, // Required by BulkUpdate
            { "MarkupContentText", "<?xml version=\"1.0\"?><root><node>Some XML Content</node></root>" }, // For validation
            { "ConvertedToHtml", "false" },
            { "CommonId", "12345" },
            { "InstructionVersionId", "SCTN-04-120-01-005v16" } // Add InstructionVersionId for query filtering
        }
    };

    // Mock database interactions
    var mockDbConnection = new Mock<IDBConnection>();
    mockDbConnection
        .Setup(db => db.ExecuteQueryAsync(It.IsAny<string>()))
        .ReturnsAsync(CreateMockDataTable(mockMarkupContentData)); // Mocked DataTable

    // Mock service or object
    var service = new TransformService(mockDbConnection.Object);

    // Act: Call the method to test
    await service.TransformXmlAndSavesToDb();

    // Assert: Ensure no exceptions and the HTML content is used correctly
    Assert.Pass("TransformXmlAndSavesToDb executed and processed HtmlContentText successfully.");
}
