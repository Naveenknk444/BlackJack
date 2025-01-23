[Test]
public async Task TestBulkUpdate_WithMissingRequiredColumn()
{
    // Arrange: Mock data with missing 'instr_ver_id' column
    var bulkToBeUpdated = new List<DataRow>
    {
        CreateMockDataRow(new Dictionary<string, string>
        {
            { "HtmlContentText", "<?xml version=\"1.0\"?><root><node>Valid Content</node></root>" },
            { "cmn_id", "12345" } // Missing 'instr_ver_id'
        })
    };

    var emptyTable = new DataTable();
    var indexList = emptyTable.AsEnumerable();
    var cmpList = emptyTable.AsEnumerable();

    var batchSize = 10;
    var saveLocal = false;

    // Act & Assert: Expect an exception due to the missing 'instr_ver_id' column
    var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
        await ConvertXmlToHtml.BulkUpdate(bulkToBeUpdated, indexList, cmpList, batchSize, saveLocal)
    );

    Assert.That(exception.Message, Does.Contain("Column 'instr_ver_id' does not belong to table"), 
        "The error message should indicate the missing column.");
}
