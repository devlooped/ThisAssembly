using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Devlooped.Sponsors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using static Devlooped.Sponsors.SponsorLink;
using Resources = Devlooped.Sponsors.Resources;

namespace ThisAssembly;

[Generator(LanguageNames.CSharp)]
public class ConstantsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var files = context.AdditionalTextsProvider
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Where(x =>
                x.Right.GetOptions(x.Left).TryGetValue("build_metadata.Constant.ItemType", out var itemType)
                && itemType == "Constant")
            .Select((x, ct) =>
            {
                x.Right.GetOptions(x.Left).TryGetValue("build_metadata.Constant.Value", out var value);
                x.Right.GetOptions(x.Left).TryGetValue("build_metadata.Constant.Comment", out var comment);
                x.Right.GetOptions(x.Left).TryGetValue("build_metadata.Constant.Root", out var root);

                // Revert auto-escaping due to https://github.com/dotnet/roslyn/issues/51692
                if (value != null && value.StartsWith("|") && value.EndsWith("|"))
                    value = value[1..^1].Replace('|', ';');

                var name = Path.GetFileName(x.Left.Path);
                if (string.IsNullOrEmpty(root))
                {
                    root = "Constants";
                }
                else if (root == ".")
                {
                    var parts = name.Split(['.'], 2);
                    if (parts.Length == 2)
                    {
                        // root should be the first part up to the first dot of name
                        // and name should be the rest
                        // note we only do this if there's an actual dot, otherwise, we 
                        // just leave the root's default of Constants
                        root = parts[0];
                        name = parts[1];
                    }
                }

                return (name, value: value ?? "", comment: string.IsNullOrWhiteSpace(comment) ? null : comment, root!);
            });

        // Read the ThisAssemblyNamespace property or default to null
        var right = context.AnalyzerConfigOptionsProvider
            .Select((c, t) => c.GlobalOptions.TryGetValue("build_property.ThisAssemblyNamespace", out var ns) && !string.IsNullOrEmpty(ns) ? ns : null)
            .Combine(context.ParseOptionsProvider);

        var inputs = files.Combine(right);
        // this is required to ensure status is registered properly independently of analyzer runs. 
        var options = context.GetStatusOptions();

        context.RegisterSourceOutput(inputs.Combine(options), GenerateConstant);
        //(spc, source) =>
        //{
        //    var status = Diagnostics.GetOrSetStatus(source.Right);
        //    var warn = IsEditor &&
        //        (status == Devlooped.Sponsors.SponsorStatus.Unknown || status == Devlooped.Sponsors.SponsorStatus.Expired);

        //    GenerateConstant(spc, source.Left, warn ? string.Format(
        //        CultureInfo.CurrentCulture, Resources.Editor_Disabled, Funding.Product, Funding.HelpUrl) : null);
        //});
    }

    void GenerateConstant(SourceProductionContext spc,
        (((string name, string value, string? comment, string root), (string? ns, ParseOptions parse)), StatusOptions options) args)
    {
        var (((name, value, comment, root), (ns, parse)), options) = args;
        var cs = (CSharpParseOptions)parse;

        if (!string.IsNullOrWhiteSpace(ns) &&
            cs.LanguageVersion < LanguageVersion.CSharp10)
        {
            spc.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("TA002", "ThisAssemblyNamespace requires C# 8.0 or higher",
                    "ThisAssemblyNamespace requires C# 8.0 or higher", "ThisAssembly", DiagnosticSeverity.Error, true),
                Location.None));
            return;
        }

        var rootArea = Area.Load([new(name, value, comment),], root);
        // For now, we only support C# though
        var file = parse.Language.Replace("#", "Sharp") + ".sbntxt";
        var template = Template.Parse(EmbeddedResource.GetContent(file), file);
        var model = new Model(rootArea, ns);
        if ((int)cs.LanguageVersion >= 1100)
            model.RawStrings = true;

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

        // Apply formatting since indenting isn't that nice in Scriban when rendering nested 
        // structures via functions.
        if (parse.Language == LanguageNames.CSharp)
        {
            output = SyntaxFactory.ParseCompilationUnit(output, options: cs)
                .NormalizeWhitespace()
                .GetText()
                .ToString();
        }

        spc.AddSource($"{root}.{name}.g.cs", SourceText.From(output, Encoding.UTF8));
    }
}
