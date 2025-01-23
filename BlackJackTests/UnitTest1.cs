[Test]
public async Task TestBulkUpdate_WithValidData()
{
    // Arrange: Create valid mock data
    var bulkToBeUpdated = new List<DataRow>
    {
        CreateMockDataRow(new Dictionary<string, string>
        {
            { "HtmlContentText", "<root><node>Valid Content</node></root>" },
            { "cmn_id", "12345" } // Add the required column
        })
    };

    // Create an empty DataTable for indexList and cmpList
    var emptyTable = new DataTable();
    var indexList = emptyTable.AsEnumerable();
    var cmpList = emptyTable.AsEnumerable();

    // Batch size and saveLocal flag
    var batchSize = 10;
    var saveLocal = false;

    // Act: Call BulkUpdate
    await ConvertXmlToHtml.BulkUpdate(bulkToBeUpdated, indexList, cmpList, batchSize, saveLocal);

    // Assert: Ensure the method completes successfully
    Assert.Pass("BulkUpdate processed the valid data successfully.");
}
