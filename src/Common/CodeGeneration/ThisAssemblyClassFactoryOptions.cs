using Microsoft.CodeAnalysis;

namespace CodeGeneration;

sealed record ThisAssemblyClassFactoryOptions(
    string ThisAssemblyClassName,
    bool GeneratePublicClass,
    bool GenerateStaticClasses,
    bool GenerateAllConstantsAsFields)
{
    public static IncrementalValueProvider<ThisAssemblyClassFactoryOptions> GetProvider(IncrementalGeneratorInitializationContext context)
    {
        return context.AnalyzerConfigOptionsProvider
            .Select((provider, _) => new ThisAssemblyClassFactoryOptions(
                provider.GlobalOptions.GetValueOrDefault("build_options.ThisAssembly_ClassName", "ThisAssembly"),
                provider.GlobalOptions.GetValueOrDefault("build_options.ThisAssembly_GeneratePublicClass", false),
                provider.GlobalOptions.GetValueOrDefault("build_options.ThisAssembly_GenerateStaticClasses", true),
                provider.GlobalOptions.GetValueOrDefault("build_options.ThisAssembly_GenerateAllConstantsAsFields", false)
            ));
    }
}
