using System;
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
public class StringsGenerator : IIncrementalGenerator
{
    static readonly Regex SeeExpr = new("<see.+sponsorlink\"/>", RegexOptions.Compiled);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Read the ThisAssemblyNamespace property or default to null
        var right = context.AnalyzerConfigOptionsProvider
            .Select((c, t) => (
                c.GlobalOptions.TryGetValue("build_property.ThisAssemblyNamespace", out var ns) && !string.IsNullOrEmpty(ns) ? ns : null,
                c.GlobalOptions.TryGetValue("build_property.ThisAssemblyVisibility", out var visibility) && !string.IsNullOrEmpty(visibility) ? visibility : null
              ))
            .Combine(context.CompilationProvider.Select((s, _) => s.Language));

        context.RegisterSourceOutput(
            right,
            (spc, args) =>
            {
                var ((ns, _), _) = args;

                var strings = EmbeddedResource.GetContent($"ThisAssembly.Strings.sbntxt");
                var template = Template.Parse(strings);
                var output = template.Render(new { Namespace = ns }, member => member.Name);
                spc.AddSource("ThisAssembly.Strings.g.cs", SourceText.From(output, Encoding.UTF8));
            });

        var files = context.AdditionalTextsProvider
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Where(x =>
                x.Right.GetOptions(x.Left).TryGetValue("build_metadata.ResxCode.ThisAssemblyStrings", out var resourceString)
                && bool.TryParse(resourceString, out var isResourceString) && isResourceString)
            .Where(x => x.Right.GetOptions(x.Left).TryGetValue("build_metadata.ResxCode.ManifestResourceName", out var value) && value != null)
            .Select((x, ct) =>
            {
                x.Right.GetOptions(x.Left).TryGetValue("build_metadata.ResxCode.ManifestResourceName", out var resourceName);
                return (fileName: Path.GetFileName(x.Left.Path), text: x.Left.GetText(ct), resourceName!);
            })
            .Where(x => x.text != null);


        context.RegisterSourceOutput(
            files.Combine(right).Combine(context.ParseOptionsProvider.Combine(context.GetStatusOptions())),
            GenerateSource);
    }

    static void GenerateSource(SourceProductionContext spc,
        (((string fileName, SourceText? text, string resourceName), ((string? ns, string? visibility), string language)), (ParseOptions parse, StatusOptions options)) arg)
    {
        var (((fileName, resourceText, resourceName), ((ns, visibility), language)), (parse, options)) = arg;

        var file = language.Replace("#", "Sharp") + ".sbntxt";
        var template = Template.Parse(EmbeddedResource.GetContent(file), file);

        var rootArea = ResourceFile.LoadText(resourceText!.ToString(), "Strings");
        var model = new Model(rootArea, resourceName, ns, "public".Equals(visibility, StringComparison.OrdinalIgnoreCase));
        if (IsEditor)
        {
            var status = Diagnostics.GetOrSetStatus(options);
            if (status == SponsorStatus.Unknown || status == SponsorStatus.Expired)
            {
                model.Warn = string.Format(CultureInfo.CurrentCulture, Resources.Editor_Disabled, Funding.Product, Funding.HelpUrl);
                model.Remarks = Resources.Editor_DisabledRemarks;
            }
            else if (status == SponsorStatus.Grace && Diagnostics.TryGet() is { } grace && grace.Properties.TryGetValue(nameof(SponsorStatus.Grace), out var days))
            {
                model.Remarks = string.Format(CultureInfo.CurrentCulture, Resources.Editor_GraceRemarks, days);
            }
        }

        var output = template.Render(model, member => member.Name);

        output = SeeExpr.Replace(SyntaxFactory
            .ParseCompilationUnit(output, options: parse as CSharpParseOptions)
            .NormalizeWhitespace()
            .GetText()
            .ToString(),
            $"<see cref=\"{Funding.HelpUrl}\"/>");

        spc.AddSource(resourceName, SourceText.From(output, Encoding.UTF8));
    }
}
