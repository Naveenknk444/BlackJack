[Test]
public void TestResolveEntities_WithMissingXmlDeclaration()
{
    // Arrange: XML without declaration
    string xmlInput = "<root><section>SECTION_TITLE</section></root>";

    // Mock indexItems and cmpList
    var indexItems = Enumerable.Empty<DataRow>();
    var cmpList = Enumerable.Empty<DataRow>();

    // Act
    var result = ConvertXmlToHtml.ResolveEntities(xmlInput, "commonId", indexItems, cmpList);

    // Assert
    Assert.That(result, Is.Empty, "The method should return an empty string when the XML declaration is missing.");
}
