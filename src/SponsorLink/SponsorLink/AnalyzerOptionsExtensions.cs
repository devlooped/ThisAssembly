using Microsoft.CodeAnalysis.Diagnostics;

static class AnalyzerOptionsExtensions
{
    /// <summary>
    /// Gets whether the current build is a design-time build.
    /// </summary>
    public static bool IsDesignTimeBuild(this AnalyzerConfigOptionsProvider options) =>
        options.GlobalOptions.TryGetValue("build_property.DesignTimeBuild", out var value) &&
        bool.TryParse(value, out var isDesignTime) && isDesignTime;

    /// <summary>
    /// Gets whether the current build is a design-time build.
    /// </summary>
    public static bool IsDesignTimeBuild(this AnalyzerConfigOptions options) =>
        options.TryGetValue("build_property.DesignTimeBuild", out var value) &&
        bool.TryParse(value, out var isDesignTime) && isDesignTime;
}