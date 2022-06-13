using System.Collections.Generic;
using System.IO;

namespace Utilities;

static class StringExtensions
{
    public static IEnumerable<string> SplitToLines(this string @this)
    {
        using var reader = new StringReader(@this);
        while (reader.ReadLine() is { } line)
        {
            yield return line;
        }
    }
}
