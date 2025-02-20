public string FormatTargetRef(string targetRef)
{
    string[] parts = targetRef.Split('-');
    if (parts.Length >= 4 && parts[3].Length == 3)
    {
        parts[3] = "0" + parts[3];
    }
    return string.Join("-", parts);
}
