private DataRow CreateMockDataRow(Dictionary<string, string> columnValues)
{
    var table = new DataTable();
    foreach (var column in columnValues.Keys)
    {
        table.Columns.Add(column, typeof(string)); // Add all required columns
    }
    var row = table.NewRow();
    foreach (var column in columnValues)
    {
        row[column.Key] = column.Value; // Set the column values
    }
    return row;
}
