var conditions = linkRefs.SelectMany(link =>
{
    var targetDocsField = typeof(LinkRef).GetField("targetDoc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    var targetRefField = typeof(LinkRef).GetField("targetRef", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

    var targetDocs = targetDocsField?.GetValue(link) as Collection<string>;
    var targetRef = targetRefField?.GetValue(link) as string;

    return targetDocs?.Select(targetDoc => $"(targetDoc = '{targetDoc}' AND targetRef = '{targetRef}')") ?? Enumerable.Empty<string>();
}).ToList();

var query = string.Join(" OR ", conditions);
