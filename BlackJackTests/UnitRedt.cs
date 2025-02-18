var conditions = linkRefs.SelectMany(link => 
    link.targetDocs.Select(targetDoc => $"(targetDoc = '{targetDoc}' AND targetRef = '{link.targetRef}')")
).ToList();
var query = string.Join(" OR ", conditions);
