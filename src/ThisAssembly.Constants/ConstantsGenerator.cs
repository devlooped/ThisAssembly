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
    public class ConstantsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var files = context.AdditionalTextsProvider
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Where(x =>
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.AdditionalFiles.SourceItemType", out var itemType)
                    && itemType == "Constant")
                .Where(x => x.Right.GetOptions(x.Left).TryGetValue("build_metadata.Constant.Value", out var value) && value != null)
                .Select((x, ct) =>
                {
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.Constant.Value", out var value);
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.Constant.Comment", out var comment);
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.Constant.Root", out var root);
                    return (
                        name: Path.GetFileName(x.Left.Path),
                        value: value!,
                        comment: string.IsNullOrWhiteSpace(comment) ? null : comment,
                        root: string.IsNullOrWhiteSpace(root) ? "Constants" : root!);
                })
                .Combine(context.CompilationProvider.Select((p, _) => p.Language));

            context.RegisterSourceOutput(
                files,
                GenerateConstant);

        }

        void GenerateConstant(SourceProductionContext spc, ((string name, string value, string? comment, string root), string language) args)
        {
            var ((name, value, comment, root), language) = args;

            var rootArea = Area.Load(new List<Constant> { new Constant(name, value, comment), }, root);
            var file = language.Replace("#", "Sharp") + ".sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);
            var output = template.Render(new Model(rootArea), member => member.Name);

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

            spc.AddSource($"{name}.g.cs", SourceText.From(output, Encoding.UTF8));
        }
    }
}
