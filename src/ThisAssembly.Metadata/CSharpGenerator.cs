using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Scriban;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.IO;

namespace ThisAssembly
{
    [Generator]
    public class CSharpGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            var includeFix = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.IncludeSourceGeneratorIntellisenseFix", out var raw) &&
                bool.TryParse(raw, out var value) &&
                value;

            var metadata = context.Compilation.Assembly.GetAttributes()
                .Where(x => x.AttributeClass?.Name == nameof(System.Reflection.AssemblyMetadataAttribute) &&
                    Microsoft.CodeAnalysis.CSharp.SyntaxFacts.IsValidIdentifier((string)x.ConstructorArguments[0].Value))
                .Select(x => new KeyValuePair<string, string>((string)x.ConstructorArguments[0].Value, (string)x.ConstructorArguments[1].Value));

            var language = context.ParseOptions.Language;
            var file = language.Replace("#", "Sharp") + ".sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);

            var output = template.Render(new Model(metadata), member => member.Name);

            if (includeFix)
            {
                if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ThisAssemblyIntellisenseFixExtension", out var extension))
                    extension = ".ta.g." + (language == LanguageNames.CSharp ? "cs" : language == LanguageNames.VisualBasic ? "vb" : "fs");

                if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.IntermediateOutputPath", out var intermediate))
                    throw new NotSupportedException();

                var path = Path.Combine(intermediate, "ThisAssembly.Metadata" + extension);
                Directory.CreateDirectory(intermediate);
                File.WriteAllText(path, output, Encoding.UTF8);

            }

            context.AddSource("ThisAssembly.Metadata", SourceText.From(output, Encoding.UTF8));
        }
    }
}
