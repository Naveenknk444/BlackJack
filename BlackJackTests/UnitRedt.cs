private string CreateQuery(List<Link> links)
{
    var conditions = links.Select(link => $"(targetDoc = '{link.TargetDoc}' AND targetRef = '{link.TargetRef}')");
    var query = $"SELECT 1 FROM YourTable WHERE " + string.Join(" OR ", conditions) + " LIMIT 1";
    return query;
}
