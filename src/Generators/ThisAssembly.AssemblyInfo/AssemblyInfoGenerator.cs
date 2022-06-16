using System.Collections.Generic;
using System.Collections.Immutable;
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
                    .Select(static x => new ClassConstant()
                    {
                        Name = x.AttributeClass!.Name.Substring(8).Replace("Attribute", ""),
                        Value = x.ConstructorArguments[0].Value?.ToString()
                    })
                    .ToImmutableArray());

            var provider = context.ParseOptionsProvider
                .Combine(ClassFactoryOptionsProvider)
                .Combine(constantsProvider);

            context.RegisterSourceOutput(provider, (ctx, data) =>
            {
                var ((parseOptions, options), constants) = data;
                var model = new ThisAssemblyClass()
                {
                    NestedClassList = new()
                    {
                        new()
                        {
                            Name = "Info",
                            XmlSummary = "Provides access to assembly attribute values without requiring reflection.",
                            Constants = constants,
                        }
                    }
                };

                var sourceText = ThisAssemblyClassFactory.Build(model, options, parseOptions);
                ctx.AddSource("ThisAssembly.Info", sourceText);
            });
        }
    }
}
