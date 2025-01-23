[Test]
public void TestResolveEntities_WithMalformedXml()
{
    // Arrange: Malformed XML input
    string xmlInput = "<?xml version=\"1.0\"?><root><section>SECTION_TITLE";

    // Mock indexItems and cmpList
    var indexItems = Enumerable.Empty<DataRow>();
    var cmpList = Enumerable.Empty<DataRow>();

    // Act & Assert
    Assert.Throws<XmlException>(() =>
        ConvertXmlToHtml.ResolveEntities(xmlInput, "commonId", indexItems, cmpList),
        "The method did not throw an XmlException for malformed XML input.");
}
