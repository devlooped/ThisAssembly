using System.Linq;
using CodeGeneration;
using CodeGeneration.Model;
using Microsoft.CodeAnalysis;

namespace ThisAssembly
{
    [Generator(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    public sealed class MetadataGenerator : ThisAssemblyGenerator
    {
        protected override string GeneratorName => "ThisAssembly.Metadata";

        protected override void InitializeGenerator(IncrementalGeneratorInitializationContext context)
        {
            var constantsProvider = context.CompilationProvider
                .SelectMany(static (compilation, _) => compilation.Assembly.GetAttributes())
                .Where(static attr => attr.ConstructorArguments.Length == 2)
                .Where(static attr => attr.AttributeClass!.Name == "AssemblyMetadataAttribute")
                .Collect()
                .Select(static (attrs, _) => attrs
                    .Select(static x => new Constant(
                        x.ConstructorArguments[0].Value!.ToString(),
                        x.ConstructorArguments[1].Value?.ToString()))
                    .ToList());

            var provider = context.ParseOptionsProvider
                .Combine(OptionsProvider)
                .Combine(constantsProvider);

            context.RegisterSourceOutput(provider, (ctx, data) =>
            {
                var ((parseOptions, options), constants) = data;
                var metadataClass = new Class("Metadata", PartialTypeKind.MainPart)
                {
                    XmlSummary = "Provides access to AssemblyMetadata attributes without requiring reflection.",
                    Constants = constants,
                };

                var model = new Class(options.ThisAssemblyClassName, PartialTypeKind.OtherPart);
                model.Add(metadataClass);
                var sourceText = CodeFactory.Build(model, options, parseOptions);
                ctx.AddSource(options.ThisAssemblyClassName + ".Metadata", sourceText);
            });
        }
    }
}
