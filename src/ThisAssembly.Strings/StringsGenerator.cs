using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace ThisAssembly
{
    [Generator]
    public class StringsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterSourceOutput(
                context.CompilationProvider.Select((c, _) => c.Language),
                (spc, l) =>
                {
                    var extension = l switch
                    {
                        LanguageNames.CSharp => "cs",
                        LanguageNames.VisualBasic => "vb",
                        _ => throw new NotSupportedException()
                    };

                    var strings = EmbeddedResource.GetContent($"ThisAssembly.Strings.{extension}");
                    spc.AddSource($"ThisAssembly.Strings.g.{extension}", SourceText.From(strings, Encoding.UTF8));
                });

            var files = context.AdditionalTextsProvider
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Where(x =>
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.AdditionalFiles.SourceItemType", out var itemType)
                    && itemType == "ResourceString")
                .Where(x => x.Right.GetOptions(x.Left).TryGetValue("build_metadata.AdditionalFiles.ManifestResourceName", out var value) && value != null)
                .Select((x, ct) =>
                {
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.AdditionalFiles.ManifestResourceName", out var resourceName);
                    return (fileName: Path.GetFileName(x.Left.Path), text: x.Left.GetText(ct), resourceName!);
                })
                .Where(x => x.text != null);

            context.RegisterSourceOutput(
                files.Combine(context.CompilationProvider.Select((s, _) => s.Language)),
                GenerateSource);
        }

        static void GenerateSource(SourceProductionContext spc, ((string fileName, SourceText? text, string resourceName), string language) arg2)
        {
            var ((fileName, resourceText, resourceName), language) = arg2;

            var file = language.Replace("#", "Sharp") + ".sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);

            var rootArea = ResourceFile.LoadText(resourceText!.ToString(), "Strings");
            var model = new Model(rootArea, resourceName);

            var output = template.Render(model, member => member.Name);

            // Apply formatting since indenting isn't that nice in Scriban when rendering nested 
            // structures via functions.
            if (language == LanguageNames.CSharp)
            {
                output = Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseCompilationUnit(output)
                    .NormalizeWhitespace()
                    .GetText()
                    .ToString();
            }
            //else if (language == LanguageNames.VisualBasic)
            //{
            //    output = Microsoft.CodeAnalysis.VisualBasic.SyntaxFactory.ParseCompilationUnit(output)
            //        .NormalizeWhitespace()
            //        .GetText()
            //        .ToString();
            //}

            spc.AddSource(resourceName, SourceText.From(output, Encoding.UTF8));
        }
    }
}
