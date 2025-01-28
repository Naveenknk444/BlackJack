[Test]
public async Task GetHtmlDocument_ValidLocalFile_ShouldLoadSuccessfully()
{
    // Arrange
    var filePath = "TestData/sample.html"; // Ensure this file exists in the test directory.
    var htmlContent = "<html><body>Sample Content</body></html>";

    // Create the test file
    if (!Directory.Exists("TestData"))
        Directory.CreateDirectory("TestData");

    await File.WriteAllTextAsync(filePath, htmlContent);

    // Act
    var document = await HallexDataMigratorSource.GetHtmlDocument(PolicyNetHtmlOrigin.Local, filePath, CancellationToken.None);

    // Assert
    Assert.IsNotNull(document, "Document should not be null for a valid local file.");
    Assert.AreEqual("Sample Content", document.DocumentNode.InnerText.Trim(), "The document content does not match the expected value.");
}
