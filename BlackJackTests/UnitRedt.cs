var conditions = linkRefs.SelectMany(link =>
{
    var targetDocsField = typeof(LinkRef).GetField("targetDoc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    var targetDocs = targetDocsField?.GetValue(link) as Collection<string>;

    return targetDocs?.Select(targetDoc => $"(targetDoc = '{targetDoc}' AND targetRef = '{link.targetRef}')") ?? Enumerable.Empty<string>();
}).ToList();

var query = string.Join(" OR ", conditions);
