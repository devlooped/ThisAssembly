using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Scriban;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace ThisAssembly
{
    [Generator]
    public class AssemblyInfoGenerator : ISourceGenerator
    {
        readonly HashSet<string> attributes = new HashSet<string>(new[]
        {
            nameof(AssemblyConfigurationAttribute),
            nameof(AssemblyCompanyAttribute),
            nameof(AssemblyTitleAttribute),
            nameof(AssemblyProductAttribute),
            nameof(AssemblyVersionAttribute),
            nameof(AssemblyInformationalVersionAttribute),
            nameof(AssemblyFileVersionAttribute),
        });

        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.DebugThisAssemblyGenerator", out var debugValue) &&
                bool.TryParse(debugValue, out var shouldDebug) &&
                shouldDebug)
                Debugger.Launch();

            var metadata = context.Compilation.Assembly.GetAttributes()
                .Where(x => attributes.Contains(x.AttributeClass?.Name))
                .Select(x => new KeyValuePair<string, string>(x.AttributeClass.Name.Substring(8).Replace("Attribute", ""), (string)x.ConstructorArguments[0].Value))
                .ToDictionary(x => x.Key, x => x.Value);

            var language = context.ParseOptions.Language;
            var file = language.Replace("#", "Sharp") + ".sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);
            var output = template.Render(new Model(metadata), member => member.Name);

            var includeFix = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.IncludeSourceGeneratorIntellisenseFix", out var raw) &&
                bool.TryParse(raw, out var value) &&
                value;

            if (includeFix)
            {
                if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ThisAssemblyIntellisenseFixExtension", out var extension))
                    extension = ".ta.g." + (language == LanguageNames.CSharp ? "cs" : language == LanguageNames.VisualBasic ? "vb" : "fs");

                if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.IntermediateOutputPath", out var intermediate))
                    throw new NotSupportedException();

                var path = Path.Combine(intermediate, "ThisAssembly.AssemblyInfo" + extension);
                Directory.CreateDirectory(intermediate);
                File.WriteAllText(path, output, Encoding.UTF8);

            }

            context.AddSource("ThisAssembly.AssemblyInfo", SourceText.From(output, Encoding.UTF8));
        }
    }
}
