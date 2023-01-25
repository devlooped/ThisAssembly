using System.Text.RegularExpressions;

static class PathSanitizer
{
    static readonly Regex invalidCharsRegex = new(@"\W");
    public static string Sanitize(string path, string parent)
    {
        var partStr = invalidCharsRegex.Replace(path, "_");
        if (char.IsDigit(partStr[0]))
            partStr = "_" + partStr;
        if (partStr == parent)
            partStr = "_" + partStr;
        return partStr;
    }
}