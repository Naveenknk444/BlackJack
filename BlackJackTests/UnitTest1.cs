[Test]
public void TestResolveEntities_WithEmptyInput()
{
    // Arrange: Empty XML input
    string xmlInput = "";

    // Mock indexItems and cmpList
    var indexItems = Enumerable.Empty<DataRow>();
    var cmpList = Enumerable.Empty<DataRow>();

    // Act
    var result = ConvertXmlToHtml.ResolveEntities(xmlInput, "commonId", indexItems, cmpList);

    // Assert
    Assert.That(result, Is.Empty, "The method should return an empty string for empty input.");
}
