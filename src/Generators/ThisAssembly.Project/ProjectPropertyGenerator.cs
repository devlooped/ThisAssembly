using System.Linq;
using CodeGeneration;
using CodeGeneration.Model;
using Microsoft.CodeAnalysis;

namespace ThisAssembly;

[Generator(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public class ProjectPropertyGenerator : ThisAssemblyGenerator
{
    protected override string GeneratorName => "ThisAssembly.Project";

    protected override void InitializeGenerator(IncrementalGeneratorInitializationContext context)
    {
        var constantsProvider = context.AnalyzerConfigOptionsProvider
            .SelectMany((provider, _) => provider.GlobalOptions.GetValueOrDefault("build_property.ThisAssembly_ProjectProperties", string.Empty).Split('|'))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Select((tuple, _) =>
            {
                var (name, optionsProvider) = tuple;
                return optionsProvider.GlobalOptions.TryGetValue("build_property." + name, out var value)
                    ? new Constant(name, value) { XmlSummary = $"{name} = \"{value}\"" }
                    : null!; // We will filter out nulls; the ! keeps nullability analysis happy
            })
            .Where(x => x != null)
            .Collect();

        var provider = context.ParseOptionsProvider
            .Combine(OptionsProvider)
            .Combine(constantsProvider);

        context.RegisterSourceOutput(provider, (ctx, data) =>
        {
            var ((parseOptions, options), constants) = data;
            var projectClass = new Class("Project", PartialTypeKind.MainPart)
            {
                XmlSummary = "Provides access to selected MSBuild project properties.",
                Constants = constants,
            };

            var model = new Class(options.ThisAssemblyClassName, PartialTypeKind.OtherPart);
            model.Add(projectClass);
            var sourceText = CodeFactory.Build(model, options, parseOptions);
            ctx.AddSource(options.ThisAssemblyClassName + ".Project", sourceText);
        });
    }
}
