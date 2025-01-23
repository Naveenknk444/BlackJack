[Test]
public void TestResolveEntities_WithValidXml()
{
    // Arrange: Valid XML input
    string xmlInput = "<?xml version=\"1.0\"?><root><section>SECTION_TITLE</section></root>";
    string expectedOutput = "<?xml version=\"1.0\"?><root><section>CLEANED_TITLE</section></root>"; // Expected transformation

    // Mock indexItems and cmpList
    var indexItems = Enumerable.Empty<DataRow>();
    var cmpList = Enumerable.Empty<DataRow>();

    // Act
    var result = ConvertXmlToHtml.ResolveEntities(xmlInput, "commonId", indexItems, cmpList);

    // Assert
    Assert.That(result, Is.EqualTo(expectedOutput), "The transformed XML does not match the expected output.");
}
