
    // Arrange: Create a DataTable with all required columns
    var mockTable = new DataTable();
    mockTable.Columns.Add("HtmlContentText", typeof(string));
    mockTable.Columns.Add("MarkupContentText", typeof(string));
    mockTable.Columns.Add("ConvertedToHtml", typeof(string));
    mockTable.Columns.Add("CommonId", typeof(string));
    mockTable.Columns.Add("InstructionVersionId", typeof(string));

    // Add rows to the DataTable
    mockTable.Rows.Add(
        "<html><body>Hardcoded HTML Content</body></html>", // HtmlContentText
        "<?xml version=\"1.0\"?><root><node>Some XML Content</node></root>", // MarkupContentText
        "false", // ConvertedToHtml
        "12345", // CommonId
        "SCTN-04-120-01-005v16" // InstructionVersionId
    );

    // Convert rows into a list of DataRow
    var mockMarkupContentData = mockTable.AsEnumerable().ToList();
