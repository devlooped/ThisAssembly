using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace ThisAssembly
{
    [Generator]
    public class ProjectPropertyGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            context.CheckDebugger("ThisAssemblyProject");

            if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ThisAssemblyProject", out var properties))
                return;

            var metadata = properties.Split('|')
                .Select(prop => new KeyValuePair<string, string?>(prop,
                    context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property." + prop, out var value) ?
                    value : null))
                .Where(pair => pair.Value != null)
                .Distinct(new KeyValueComparer())
                .ToDictionary(x => x.Key, x => x.Value);

            var model = new Model(metadata);
            var language = context.ParseOptions.Language;
            var file = language.Replace("#", "Sharp") + ".sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);
            var output = template.Render(model, member => member.Name);

            context.AddSource("ThisAssembly.Project", SourceText.From(output, Encoding.UTF8));
        }

        class KeyValueComparer : IEqualityComparer<KeyValuePair<string, string?>>
        {
            public bool Equals(KeyValuePair<string, string?> x, KeyValuePair<string, string?> y)
                => x.Key == y.Key && x.Value == y.Value;

            public int GetHashCode(KeyValuePair<string, string?> obj)
                => new HashCode().AddRange(obj.Key, obj.Value ?? "").ToHashCode();
        }

        public static string[] GetItems(GeneratorExecutionContext context)
            => context
                .AdditionalFiles
                .Where(f => context.AnalyzerConfigOptions
                    .GetOptions(f)
                    .TryGetValue("build_metadata.ThisAssemblyProject.ItemSpec", out var identity))
                .Select(f => f.Path)
                .ToArray();
    }
}
