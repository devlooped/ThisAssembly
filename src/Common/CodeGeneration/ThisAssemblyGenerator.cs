using CodeGeneration.Model;
using Microsoft.CodeAnalysis;

namespace CodeGeneration;

public abstract class ThisAssemblyGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        OptionsProvider = ThisAssemblyOptions.GetProvider(context);
        InitializeCommonCode(context);
        InitializeGenerator(context);
    }

    private protected IncrementalValueProvider<ThisAssemblyOptions> OptionsProvider { get; private set; }

    protected abstract void InitializeGenerator(IncrementalGeneratorInitializationContext context);

    protected abstract string GeneratorName { get; }

    void InitializeCommonCode(IncrementalGeneratorInitializationContext context)
    {
        var firstGeneratorNameProvider = context.AnalyzerConfigOptionsProvider
            .Select((provider, _) => provider.GlobalOptions.GetValueOrDefault("build_property.ThisAssembly_FirstGeneratorName", string.Empty));

        var provider = context.ParseOptionsProvider
            .Combine(OptionsProvider)
            .Combine(firstGeneratorNameProvider);

        context.RegisterSourceOutput(provider, (ctx, data) =>
        {
            var ((parseOptions, options), firstGeneratorName) = data;
            if (firstGeneratorName != GeneratorName)
            {
                return;
            }

            var model = new Class(options.ThisAssemblyClassName, PartialTypeKind.MainPart)
            {
                XmlSummary = "Provides access to current assembly information without requiring reflection.",
            };

            var sourceText = CodeFactory.Build(model, options, parseOptions);
            ctx.AddSource(options.ThisAssemblyClassName, sourceText);
        });
    }
}
