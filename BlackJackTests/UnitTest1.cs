[Test]
public void TestSaxonTransform_WithValidXmlAndXslt()
{
    // Arrange: Valid XML and XSLT
    string xmlInput = "<?xml version=\"1.0\"?><root><node>Test Content</node></root>";
    string xsltInput = @"<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
                            <xsl:template match=""/"">
                                <html><body><xsl:value-of select=""//node"" /></body></html>
                            </xsl:template>
                        </xsl:stylesheet>";
    var processor = new Processor();
    var xsltExecutable = processor.NewXsltCompiler().Compile(new StringReader(xsltInput));
    bool saveLocal = false;

    // Act
    var result = SaxonTransform(xmlInput, xsltExecutable, processor, saveLocal);

    // Assert
    Assert.That(result, Is.EqualTo("<html><body>Test Content</body></html>"), 
        "The transformed output does not match the expected HTML.");
}
