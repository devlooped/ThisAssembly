using Microsoft.CodeAnalysis;

namespace CodeGeneration;

sealed record ThisAssemblyOptions(
    string ThisAssemblyClassName,
    bool GeneratePublicClass,
    bool GenerateStaticClasses,
    bool GenerateAllConstantsAsFields)
{
    public static IncrementalValueProvider<ThisAssemblyOptions> GetProvider(IncrementalGeneratorInitializationContext context)
    {
        return context.AnalyzerConfigOptionsProvider
            .Select((provider, _) => new ThisAssemblyOptions(
                provider.GlobalOptions.GetValueOrDefault("build_property.ThisAssembly_ClassName", "ThisAssembly"),
                provider.GlobalOptions.GetValueOrDefault("build_property.ThisAssembly_GeneratePublicClass", false),
                provider.GlobalOptions.GetValueOrDefault("build_property.ThisAssembly_GenerateStaticClasses", true),
                provider.GlobalOptions.GetValueOrDefault("build_property.ThisAssembly_GenerateAllConstantsAsFields", false)
            ));
    }
}
