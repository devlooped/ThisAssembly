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
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.EmbeddedResource.Kind", out var kind);
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.EmbeddedResource.Comment", out var comment);
                    return (resourceName!, kind, comment: string.IsNullOrWhiteSpace(comment) ? null : comment);
                })
                .Combine(context.AnalyzerConfigOptionsProvider
                    .SelectMany((p, _) =>
                    {
                        if (!p.GlobalOptions.TryGetValue("build_property.EmbeddedResourceStringExtensions", out var extensions) ||
                            extensions == null)
                            return Array.Empty<string>();

                        return extensions.Split('|');
                    })
                    .WithComparer(StringComparer.OrdinalIgnoreCase)
                    .Collect());

            context.RegisterSourceOutput(
                files,
                GenerateSource);
        }

        static void GenerateSource(SourceProductionContext spc, ((string resourceName, string? kind, string? comment), ImmutableArray<string> extensions) arg2)
        {
            var ((resourceName, kind, comment), extensions) = arg2;

            var file = "CSharp.sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);

            var isText = kind?.Equals("text", StringComparison.OrdinalIgnoreCase) == true ||
                extensions.Contains(Path.GetExtension(resourceName));
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
