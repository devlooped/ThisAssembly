using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace ThisAssembly
{
    [Generator]
    public class ConstantsGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            context.CheckDebugger("ThisAssemblyConstants");

            var constantFiles = context.AdditionalFiles
                    .Where(f => context.AnalyzerConfigOptions
                        .GetOptions(f)
                        .TryGetValue("build_metadata.AdditionalFiles.SourceItemType", out var itemType)
                        && itemType == "Constant");

            if (!constantFiles.Any())
                return;

            var pairs = constantFiles.Select(f =>
            {
                context.AnalyzerConfigOptions.GetOptions(f).TryGetValue("build_metadata.Constant.Value", out var value);
                context.AnalyzerConfigOptions.GetOptions(f).TryGetValue("build_metadata.Constant.Comment", out var comment);
                return new Constant(Path.GetFileName(f.Path), value, string.IsNullOrWhiteSpace(comment) ? null : comment);
            }).Where(x => x.Value != null).ToList();

            var root = Area.Load(pairs);
            var language = context.ParseOptions.Language;
            var file = language.Replace("#", "Sharp") + ".sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);
            var output = template.Render(new Model(root), member => member.Name);

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

            context.ApplyDesignTimeFix(output, "ThisAssembly.Constants", language);
            context.AddSource("ThisAssembly.Constants", SourceText.From(output, Encoding.UTF8));
        }
    }
}
