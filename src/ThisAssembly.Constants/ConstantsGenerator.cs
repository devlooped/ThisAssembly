using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
                .Combine(context.ParseOptionsProvider);

            context.RegisterSourceOutput(
                files,
                GenerateConstant);

        }

        void GenerateConstant(SourceProductionContext spc, ((string name, string value, string? comment, string root), ParseOptions parse) args)
        {
            var ((name, value, comment, root), parse) = args;

            var rootArea = Area.Load(new List<Constant> { new Constant(name, value, comment), }, root);
            var file = parse.Language.Replace("#", "Sharp") + ".sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);
            var model = new Model(rootArea);
            if (parse is CSharpParseOptions cs && (int)cs.LanguageVersion >= 1100)
                model.RawStrings = true;

            var output = template.Render(model, member => member.Name);

            // Apply formatting since indenting isn't that nice in Scriban when rendering nested 
            // structures via functions.
            if (parse.Language == LanguageNames.CSharp)
            {
                output = SyntaxFactory.ParseCompilationUnit(output)
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
