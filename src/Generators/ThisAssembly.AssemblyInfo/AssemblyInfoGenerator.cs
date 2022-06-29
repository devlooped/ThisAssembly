using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeGeneration;
using CodeGeneration.Model;
using Microsoft.CodeAnalysis;
using Utilities;

namespace ThisAssembly
{
    [Generator(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    public sealed class AssemblyInfoGenerator : ThisAssemblyGenerator
    {
        static readonly HashSet<string> _attributes = new(new[]
        {
            nameof(AssemblyConfigurationAttribute),
            nameof(AssemblyCompanyAttribute),
            nameof(AssemblyCopyrightAttribute),
            nameof(AssemblyTitleAttribute),
            nameof(AssemblyDescriptionAttribute),
            nameof(AssemblyProductAttribute),
            nameof(AssemblyVersionAttribute),
            nameof(AssemblyInformationalVersionAttribute),
            nameof(AssemblyFileVersionAttribute),
        });

        protected override string GeneratorName => "ThisAssembly.AssemblyInfo";

        protected override void InitializeGenerator(IncrementalGeneratorInitializationContext context)
        {
            var constantsProvider = context.CompilationProvider
                .SelectMany(static (compilation, _) => compilation.Assembly.GetAttributes())
                .Where(static attr => attr.ConstructorArguments.Length == 1)
                .Where(static attr => !string.IsNullOrEmpty(attr.AttributeClass?.Name))
                .Where(static attr => _attributes.Contains(attr.AttributeClass!.Name))
                .Collect()
                .Select(static (attrs, _) => attrs
                    .Distinct(AttributeDataClassNameComparer.Instance)
                    .Select(static x => new Constant(
                        x.AttributeClass!.Name.Substring(8).Replace("Attribute", ""),
                        x.ConstructorArguments[0].Value?.ToString()))
                    .ToList());

            var provider = context.ParseOptionsProvider
                .Combine(OptionsProvider)
                .Combine(constantsProvider);

            context.RegisterSourceOutput(provider, (ctx, data) =>
            {
                var ((parseOptions, options), constants) = data;
                var model = new Class(options.ThisAssemblyClassName, PartialTypeKind.OtherPart)
                {
                    NestedClasses = new Class[]
                    {
                        new("Info", PartialTypeKind.MainPart)
                        {
                            XmlSummary = "Provides access to assembly attribute values without requiring reflection.",
                            Constants = constants,
                        }
                    }
                };

                var sourceText = CodeFactory.Build(model, options, parseOptions);
                ctx.AddSource(options.ThisAssemblyClassName + ".Info", sourceText);
            });
        }
    }
}
