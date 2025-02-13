    // Moved method: Fetch existing records from the Anchor table
    public async Task<HashSet<(string TargetDoc, string TargetRef)>> FetchExistingAnchorsAsync(
        List<(string TargetDoc, string TargetRef)> linkPairs)
    {
        if (!linkPairs.Any())
            return new HashSet<(string, string)>();

        var targetDocs = linkPairs.Select(e => e.TargetDoc).ToHashSet();
        var targetRefs = linkPairs.Select(e => e.TargetRef).ToHashSet();

        using var db = _dbContextFactory.CreateDbContext();
        var existingAnchors = await db.Anchor
            .Where(a => targetDocs.Contains(a.ClcthNm) && targetRefs.Contains(a.AnkrNum))
            .Select(a => new { a.ClcthNm, a.AnkrNum })
            .ToListAsync();

        return new HashSet<(string, string)>(existingAnchors.Select(a => (a.ClcthNm, a.AnkrNum)));
    }
