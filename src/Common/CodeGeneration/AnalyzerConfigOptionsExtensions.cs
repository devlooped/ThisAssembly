using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeGeneration;

static class AnalyzerConfigOptionsExtensions
{
    public static string GetValueOrDefault(this AnalyzerConfigOptions @this, string name, string defaultValue)
        => @this.TryGetValue(name, out var result) && !string.IsNullOrEmpty(result) ? result : defaultValue;

    public static bool GetValueOrDefault(this AnalyzerConfigOptions @this, string name, bool defaultValue)
        => @this.TryGetValue(name, out var str) && bool.TryParse(str, out var result) ? result : defaultValue;
}
