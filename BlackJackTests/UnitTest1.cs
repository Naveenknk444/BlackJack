[Test]
public async Task GetHtmlDocument_ValidLocalFile_ShouldLoadSuccessfully()
{
    // Arrange
    var filePath = "TestData/sample.html"; // File path for the test
    var htmlContent = "<html><body>Test Content</body></html>";

    // Ensure the directory exists
    if (!Directory.Exists("TestData"))
        Directory.CreateDirectory("TestData");

    // Create the file with test content
    await File.WriteAllTextAsync(filePath, htmlContent);

    // Act
    var document = await HallexDataMigratorSource.GetHtmlDocument(PolicyNetHtmlOrigin.Local, filePath, CancellationToken.None);

    // Assert
    Assert.IsNotNull(document, "Document should not be null for a valid local file.");
    Assert.AreEqual("Test Content", document.DocumentNode.InnerText.Trim(), "The document content does not match the expected value.");
}
