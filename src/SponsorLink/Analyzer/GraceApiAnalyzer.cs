using System.Collections.Immutable;
using System.Linq;
using Devlooped.Sponsors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using static Devlooped.Sponsors.SponsorLink;

namespace Analyzer;

/// <summary>
/// Links the sponsor status for the current compilation.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public class GraceApiAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        new DiagnosticDescriptor(
            "SL010", "Grace API usage", "Reports info for APIs that are in grace period", "Sponsors",
            DiagnosticSeverity.Info, true, helpLinkUri: Funding.HelpUrl),
        new DiagnosticDescriptor(
            "SL011", "Report Sponsoring Status", "Fake to get it to call us", "Sponsors",
            DiagnosticSeverity.Warning, true)
        );

#pragma warning disable RS1026 // Enable concurrent execution
    public override void Initialize(AnalysisContext context)
#pragma warning restore RS1026 // Enable concurrent execution
    {
#if !DEBUG
        // Only enable concurrent execution in release builds, otherwise debugging is quite annoying.
        context.EnableConcurrentExecution();
#endif
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        // Report info grace and expiring diagnostics.
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.InvocationExpression);
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.SimpleMemberAccessExpression);
    }

    void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var status = Diagnostics.GetOrSetStatus(() => context.Options);
        if (status != SponsorStatus.Grace)
            return;

        ReportGraceSymbol(context, context.Node.GetLocation(), context.SemanticModel.GetSymbolInfo(context.Node).Symbol);
    }

    void ReportGraceSymbol(SyntaxNodeAnalysisContext context, Location location, ISymbol? symbol)
    {
        if (symbol != null &&
            symbol.GetAttributes().Any(attr =>
            attr.AttributeClass?.ToDisplayString() == "System.ComponentModel.CategoryAttribute" &&
            attr.ConstructorArguments.Any(arg => arg.Value as string == "Sponsored")))
        {
            context.ReportDiagnostic(Diagnostic.Create(
                SupportedDiagnostics[0],
                location));
        }
    }
}
