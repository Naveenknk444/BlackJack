[Test]
public async Task TestBulkUpdate_WithMalformedXmlContent()
{
    // Arrange: Mock data with malformed XML
    var bulkToBeUpdated = new List<DataRow>
    {
        CreateMockDataRow(new Dictionary<string, string>
        {
            { "HtmlContentText", "<root><node>Missing Closing Tag" }, // Malformed XML
            { "cmn_id", "12345" },
            { "instr_ver_id", "67890" }
        })
    };

    var emptyTable = new DataTable();
    var indexList = emptyTable.AsEnumerable();
    var cmpList = emptyTable.AsEnumerable();

    var batchSize = 10;
    var saveLocal = false;

    // Act & Assert: Ensure the method handles malformed XML gracefully
    try
    {
        await ConvertXmlToHtml.BulkUpdate(bulkToBeUpdated, indexList, cmpList, batchSize, saveLocal);
        Assert.Pass("BulkUpdate handled malformed XML without crashing.");
    }
    catch (Exception ex)
    {
        Assert.Fail($"BulkUpdate threw an exception for malformed XML: {ex.Message}");
    }
}
