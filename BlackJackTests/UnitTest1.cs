[Test]
public void TestResolveEntities_RemovesMicrosoftEdgeLinks()
{
    // Arrange: XML input with microsoft-edge links
    string xmlInput = "<?xml version=\"1.0\"?><root><link href=\"microsoft-edge:https://example.com\">Link</link></root>";
    string expectedOutput = "<?xml version=\"1.0\"?><root><link href=\"https://example.com\">Link</link></root>"; // Without microsoft-edge

    // Mock indexItems and cmpList
    var indexItems = Enumerable.Empty<DataRow>();
    var cmpList = Enumerable.Empty<DataRow>();

    // Act
    var result = ConvertXmlToHtml.ResolveEntities(xmlInput, "commonId", indexItems, cmpList);

    // Assert
    Assert.That(result, Is.EqualTo(expectedOutput), "The microsoft-edge links were not removed correctly.");
}
