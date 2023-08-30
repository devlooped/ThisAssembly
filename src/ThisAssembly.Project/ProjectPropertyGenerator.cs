using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace ThisAssembly
{
    [Generator]
    public class ProjectPropertyGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var properties = context.AnalyzerConfigOptionsProvider
                .SelectMany((p, ct) =>
                {
                    var go = p.GlobalOptions;
                    if (!go.TryGetValue("build_property.ThisAssemblyProject", out var values))
                        return Array.Empty<KeyValuePair<string, string?>>();

                    return values.Split('|')
                        .Select(prop => new KeyValuePair<string, string?>(
                            prop,
                            go.TryGetValue("build_property." + prop, out var value) ? value : null))
                        .Where(pair => pair.Value != null);
                })
                .Collect();

            context.RegisterSourceOutput(
                properties.Combine(context.ParseOptionsProvider),
                GenerateSource);
        }

        void GenerateSource(SourceProductionContext spc, (ImmutableArray<KeyValuePair<string, string?>> properties, ParseOptions parse) arg)
        {
            var (properties, parse) = arg;

            var model = new Model(properties);
            if (parse is CSharpParseOptions cs && (int)cs.LanguageVersion >= 1100)
                model.RawStrings = true;

            var file = parse.Language.Replace("#", "Sharp") + ".sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);
            var output = template.Render(model, member => member.Name);

            spc.AddSource(
                "ThisAssembly.Property.g.cs",
                SourceText.From(output, Encoding.UTF8));
        }
    }
}
