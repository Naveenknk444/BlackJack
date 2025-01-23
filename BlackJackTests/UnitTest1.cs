[Test]
public async Task TestBulkUpdate_WithMalformedXmlInput()
{
    // Arrange: Mock data with malformed XML
    var bulkToBeUpdated = new List<DataRow>
    {
        CreateMockDataRow(new Dictionary<string, string>
        {
            { "HtmlContentText", "<root><node>Invalid Content" }, // Missing closing tags
            { "cmn_id", "12345" }
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
        Assert.Fail($"BulkUpdate threw an exception: {ex.Message}");
    }
}
