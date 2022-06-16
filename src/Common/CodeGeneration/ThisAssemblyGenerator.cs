using System;
using System.Collections.Generic;
using System.Text;
using CodeGeneration.Model;
using Microsoft.CodeAnalysis;

namespace CodeGeneration;

public abstract class ThisAssemblyGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        ClassFactoryOptionsProvider = ThisAssemblyClassFactoryOptions.GetProvider(context);
        InitializeCommonCode(context);
        InitializeGenerator(context);
    }

    private protected IncrementalValueProvider<ThisAssemblyClassFactoryOptions> ClassFactoryOptionsProvider { get; private set; }

    protected abstract void InitializeGenerator(IncrementalGeneratorInitializationContext context);

    protected abstract string GeneratorName { get; }

    void InitializeCommonCode(IncrementalGeneratorInitializationContext context)
    {
        var firstGeneratorNameProvider = context.AnalyzerConfigOptionsProvider
            .Select((provider, _) => provider.GlobalOptions.GetValueOrDefault("build_options.ThisAssemblyFirstGenerator", string.Empty));

        var provider = context.ParseOptionsProvider
            .Combine(ClassFactoryOptionsProvider)
            .Combine(firstGeneratorNameProvider);

        context.RegisterSourceOutput(provider, (ctx, data) =>
        {
            var ((parseOptions, options), firstGeneratorName) = data;
            if (firstGeneratorName != GeneratorName)
            {
                return;
            }

            var model = new ThisAssemblyClass()
            {
                IsMainPart = true,
                XmlSummary = "Provides access to current assembly information without requiring reflection.",
            };

            var sourceText = ThisAssemblyClassFactory.Build(model, options, parseOptions);
            ctx.AddSource("ThisAssembly", sourceText);
        });
    }
}
