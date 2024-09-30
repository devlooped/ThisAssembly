using System;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Devlooped.Sponsors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using static Devlooped.Sponsors.SponsorLink;

namespace ThisAssembly;

[Generator(LanguageNames.CSharp)]
public class ResourcesGenerator : IIncrementalGenerator
{
    static readonly Regex SeeExpr = new("<see.+sponsorlink\"/>", RegexOptions.Compiled);

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

        // Read the ThisAssemblyNamespace property or default to null
        var right = context.AnalyzerConfigOptionsProvider
            .Select((c, t) => (
                c.GlobalOptions.TryGetValue("build_property.ThisAssemblyNamespace", out var ns) && !string.IsNullOrEmpty(ns) ? ns : null,
                c.GlobalOptions.TryGetValue("build_property.ThisAssemblyVisibility", out var visibility) && !string.IsNullOrEmpty(visibility) ? visibility : null
              ));

        // this is required to ensure status is registered properly independently of analyzer runs. 
        var options = context.GetStatusOptions();

        context.RegisterSourceOutput(
            files.Combine(right).Combine(options.Combine(context.ParseOptionsProvider)),
            GenerateSource);
    }

    static void GenerateSource(SourceProductionContext spc,
        (((ImmutableArray<(string resourceName, string? kind, string? comment)> files,
            ImmutableArray<string> extensions), (string? ns, string? visibility)), (StatusOptions options, ParseOptions parse)) args)
    {
        var (((files, extensions), (ns, visibility)), (options, parse)) = args;

        var file = "CSharp.sbntxt";
        var template = Template.Parse(EmbeddedResource.GetContent(file), file);

        var groupsWithoutExtensions = files
            .GroupBy(f => Path.Combine(
                Path.GetDirectoryName(f.resourceName),
                Path.GetFileNameWithoutExtension(f.resourceName)));

        string? warn = default;
        string? remarks = default;
        if (IsEditor)
        {
            var status = Diagnostics.GetOrSetStatus(options);
            if (status == SponsorStatus.Unknown || status == SponsorStatus.Expired)
            {
                warn = string.Format(CultureInfo.CurrentCulture, Resources.Editor_Disabled, Funding.Product, Funding.HelpUrl);
                remarks = Resources.Editor_DisabledRemarks;
            }
            else if (status == SponsorStatus.Grace && Diagnostics.TryGet() is { } grace && grace.Properties.TryGetValue(nameof(SponsorStatus.Grace), out var days))
            {
                remarks = string.Format(CultureInfo.CurrentCulture, Resources.Editor_GraceRemarks, days);
            }
        }

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
            var model = new Model(root, ns, "public".Equals(visibility, StringComparison.OrdinalIgnoreCase))
            {
                Warn = warn,
                Remarks = remarks,
            };
            var output = template.Render(model, member => member.Name);

            output = SeeExpr.Replace(SyntaxFactory
                .ParseCompilationUnit(output, options: parse as CSharpParseOptions)
                .NormalizeWhitespace()
                .GetText()
                .ToString(),
                $"<see cref=\"{Funding.HelpUrl}\"/>");

            spc.AddSource(
                $"{basePath.Replace('\\', '.').Replace('/', '.')}.g.cs",
                SourceText.From(output, Encoding.UTF8));
        }
    }
}
