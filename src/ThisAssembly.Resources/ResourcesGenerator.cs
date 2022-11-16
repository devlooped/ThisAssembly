using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace ThisAssembly
{
    [Generator(LanguageNames.CSharp)]
    public class ResourcesGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(
                spc => spc.AddSource(
                    "ThisAssembly.Resources.EmbeddedResource.cs",
                    SourceText.From(EmbeddedResource.GetContent("EmbeddedResource.cs"), Encoding.UTF8)));

            var files = context.AdditionalTextsProvider
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Where(x =>
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.AdditionalFiles.SourceItemType", out var itemType)
                    && itemType == "EmbeddedResource")
                .Where(x => x.Right.GetOptions(x.Left).TryGetValue("build_metadata.EmbeddedResource.Value", out var value) && value != null)
                .Select((x, ct) =>
                {
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.EmbeddedResource.Value", out var resourceName);
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.EmbeddedResource.Type", out var type);
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.EmbeddedResource.Comment", out var comment);
                    return (resourceName!, type, comment: string.IsNullOrWhiteSpace(comment) ? null : comment);
                })
                .Combine(context.AnalyzerConfigOptionsProvider
                    .Select((p, _) =>
                    {
                        p.GlobalOptions.TryGetValue("build_property.EmbeddedResourceStringExtensions", out var extensions);
                        return extensions!;
                    }));

            context.RegisterSourceOutput(
                files,
                GenerateSource);
        }

        static void GenerateSource(SourceProductionContext spc, ((string resourceName, string? type, string? comment), string extensions) arg2)
        {
            var ((resourceName, type, comment), extensions) = arg2;

            var file = "CSharp.sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);

            var isText = type != null && type.Equals("text", StringComparison.OrdinalIgnoreCase)
                || extensions.Split(';').Contains(Path.GetFileName(resourceName));
            var root = Area.Load(new Resource(resourceName, comment, isText));
            var model = new Model(root);

            var output = template.Render(model, member => member.Name);

            // Apply formatting since indenting isn't that nice in Scriban when rendering nested 
            // structures via functions.
            output = Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseCompilationUnit(output)
                .NormalizeWhitespace()
                .GetText()
                .ToString();

            spc.AddSource(
                $"{resourceName.Replace('\\', '.').Replace('/', '.')}.g.cs", 
                SourceText.From(output, Encoding.UTF8));
        }
    }
}
