[Test]
public void TestSaxonTransform_WithMalformedXml()
{
    // Arrange: Malformed XML
    string xmlInput = "<?xml version=\"1.0\"?><root><node>Missing Closing Tag";
    string xsltInput = @"<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
                            <xsl:template match=""/"">
                                <html><body><xsl:value-of select=""//node"" /></body></html>
                            </xsl:template>
                        </xsl:stylesheet>";
    var processor = new Processor();
    var xsltExecutable = processor.NewXsltCompiler().Compile(new StringReader(xsltInput));
    bool saveLocal = false;

    // Act & Assert: Expect the method to return null due to malformed XML
    var result = SaxonTransform(xmlInput, xsltExecutable, processor, saveLocal);
    Assert.That(result, Is.Null, "The method did not handle malformed XML correctly.");
}
