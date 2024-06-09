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
                    "ThisAssembly.EmbeddedResource.cs",
                    SourceText.From(EmbeddedResource.GetContent("EmbeddedResource.cs"), Encoding.UTF8)));

            var files = context.AdditionalTextsProvider
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Where(x =>
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.EmbeddedResource.ThisAssemblyResource", out var assemblyResource)
                    && bool.TryParse(assemblyResource, out var isAssemblyResource) && isAssemblyResource)
                .Where(x => x.Right.GetOptions(x.Left).TryGetValue("build_metadata.EmbeddedResource.Value", out var value) && value != null)
                .Select((x, ct) =>
                {
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.EmbeddedResource.Value", out var resourceName);
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.EmbeddedResource.Kind", out var kind);
                    x.Right.GetOptions(x.Left).TryGetValue("build_metadata.EmbeddedResource.Comment", out var comment);
                    return (resourceName: resourceName!, kind, comment: string.IsNullOrWhiteSpace(comment) ? null : comment);
                })
                .Collect()
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

        static void GenerateSource(
            SourceProductionContext spc,
            (
                ImmutableArray<(string resourceName, string? kind, string? comment)> files,
                ImmutableArray<string> extensions) arg2)
        {
            var (files, extensions) = arg2;

            var file = "CSharp.sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);

            var groupsWithoutExtensions = files
                .GroupBy(f => Path.Combine(
                    Path.GetDirectoryName(f.resourceName),
                    Path.GetFileNameWithoutExtension(f.resourceName)));
            foreach (var group in groupsWithoutExtensions)
            {
                var basePath = group.Key;
                var resources = group
                    .Select(f =>
                    {
                        var isText = f.kind?.Equals("text", StringComparison.OrdinalIgnoreCase) == true ||
                            extensions.Contains(Path.GetExtension(f.resourceName));
                        var name = group.Count() == 1
                            ? Path.GetFileNameWithoutExtension(f.resourceName)
                            : Path.GetExtension(f.resourceName)[1..];
                        return new Resource(
                            Name: name,
                            Comment: f.comment,
                            IsText: isText,
                            Path: f.resourceName);
                    })
                    .ToList();

                var root = Area.Load(basePath, resources);
                var model = new Model(root);

                var output = template.Render(model, member => member.Name);

                // Apply formatting since indenting isn't that nice in Scriban when rendering nested 
                // structures via functions.
                output = Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseCompilationUnit(output)
                    .NormalizeWhitespace()
                    .GetText()
                    .ToString();

                spc.AddSource(
                    $"{basePath.Replace('\\', '.').Replace('/', '.')}.g.cs",
                    SourceText.From(output, Encoding.UTF8));
            }
        }
    }
}
