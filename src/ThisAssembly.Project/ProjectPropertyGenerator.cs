using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Scriban;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ThisAssembly
{
    [Generator]
    public class ProjectPropertyGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.DebugThisAssemblyGenerator", out var debugValue) &&
                bool.TryParse(debugValue, out var shouldDebug) &&
                shouldDebug)
                Debugger.Launch();

            if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ThisAssemblyProject", out var properties))
                return;

            var metadata = properties.Split('|')
                .Select(prop => new KeyValuePair<string, string>(prop,
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

            context.ApplyDesignTimeFix(output, "ThisAssembly.Project", language);
            context.AddSource("ThisAssembly.Project", SourceText.From(output, Encoding.UTF8));
        }

        class KeyValueComparer : IEqualityComparer<KeyValuePair<string, string>>
        {
            public bool Equals(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
                => x.Key == y.Key && x.Value == y.Value;

            public int GetHashCode(KeyValuePair<string, string> obj)
                => HashCode.Combine(obj.Key, obj.Value);
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
