using System;

/// <summary />
public static class Extensions
{
    /// <summary />
    public static string ReplaceLineEndings(this string input) => ReplaceLineEndings(input, Environment.NewLine);

    /// <summary />
    public static string ReplaceLineEndings(this string input, string replacementText)
    {
#if NET6_0_OR_GREATER
        return input.ReplaceLineEndings(replacementText);
#else
        // First normalize to LF
        var lineFeedInput = input
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Replace("\f", "\n")
            .Replace("\x0085", "\n")
            .Replace("\x2028", "\n")
            .Replace("\x2029", "\n");

        // Then normalize to the replacement text
        return lineFeedInput.Replace("\n", replacementText);
#endif
    }
}
