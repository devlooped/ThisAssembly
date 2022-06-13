using System;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.CodeAnalysis;

namespace CodeGeneration;

static class IncrementalGeneratorInitializationContextExtensions
{
    public static IncrementalValueProvider<string?> GetBuildPropertyProvider(this IncrementalGeneratorInitializationContext @this, string propertyName)
    {
        var optionName = "build_property." + propertyName;
        return @this.AnalyzerConfigOptionsProvider
            .Select((provider, _) => provider.GlobalOptions.TryGetValue(optionName, out var result) ? result : null);
    }

    public static IncrementalValueProvider<bool?> GetBooleanBuildPropertyProvider(this IncrementalGeneratorInitializationContext @this, string propertyName)
        => GetBuildPropertyProvider(@this, propertyName)
            .Select((s, _) => s?.Equals("true", StringComparison.OrdinalIgnoreCase));

}