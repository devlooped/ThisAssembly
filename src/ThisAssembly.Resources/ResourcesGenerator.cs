using System;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace ThisAssembly;

[Generator(LanguageNames.CSharp)]
public class ResourcesGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
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
                        return [];

                    return extensions.Split('|');
                })
                .WithComparer(StringComparer.OrdinalIgnoreCase)
                .Collect());

        // Read the ThisAssemblyNamespace property or default to null
        var right = context.AnalyzerConfigOptionsProvider
            .Select((c, t) => (
                c.GlobalOptions.TryGetValue("build_property.ThisAssemblyNamespace", out var ns) && !string.IsNullOrEmpty(ns) ? ns : null,
                c.GlobalOptions.TryGetValue("build_property.ThisAssemblyVisibility", out var visibility) && !string.IsNullOrEmpty(visibility) ? visibility : null
              ));

        context.RegisterSourceOutput(
            files.Combine(right).Combine(context.ParseOptionsProvider),
            GenerateSource);
    }

    static void GenerateSource(SourceProductionContext spc,
        (((ImmutableArray<(string resourceName, string? kind, string? comment)> files,
            ImmutableArray<string> extensions), (string? ns, string? visibility)), ParseOptions parse) args)
    {
        var (((files, extensions), (ns, visibility)), parse) = args;

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
            var model = new Model(root, ns, "public".Equals(visibility, StringComparison.OrdinalIgnoreCase));
            var output = template.Render(model, member => member.Name);

            output = SyntaxFactory
                .ParseCompilationUnit(output, options: parse as CSharpParseOptions)
                .NormalizeWhitespace()
                .GetText()
                .ToString();

            spc.AddSource(
                $"{basePath.Replace('\\', '.').Replace('/', '.')}.g.cs",
                SourceText.From(output, Encoding.UTF8));
        }
    }
}
