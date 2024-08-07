﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Scriban;

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

                return (
                    name: Path.GetFileName(x.Left.Path),
                    value: value ?? "",
                    comment: string.IsNullOrWhiteSpace(comment) ? null : comment,
                    root: string.IsNullOrWhiteSpace(root) ? "Constants" : root!);
            });

        // Read the ThisAssemblyNamespace property or default to null
        var right = context.AnalyzerConfigOptionsProvider
            .Select((c, t) => c.GlobalOptions.TryGetValue("build_property.ThisAssemblyNamespace", out var ns) && !string.IsNullOrEmpty(ns) ? ns : null)
            .Combine(context.ParseOptionsProvider);

        context.RegisterSourceOutput(
            files.Combine(right),
            GenerateConstant);

    }

    void GenerateConstant(SourceProductionContext spc, ((string name, string value, string? comment, string root), (string? ns, ParseOptions parse)) args)
    {
        var ((name, value, comment, root), (ns, parse)) = args;
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

        var rootArea = Area.Load(new List<Constant> { new Constant(name, value, comment), }, root);
        // For now, we only support C# though
        var file = parse.Language.Replace("#", "Sharp") + ".sbntxt";
        var template = Template.Parse(EmbeddedResource.GetContent(file), file);
        var model = new Model(rootArea, ns);
        if ((int)cs.LanguageVersion >= 1100)
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
