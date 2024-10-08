﻿using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Devlooped.Sponsors;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using static Devlooped.Sponsors.SponsorLink;

namespace Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public class StatusReportingAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        new DiagnosticDescriptor(
            "SL001", "Report Sponsoring Status", "Reports sponsoring status determined by SponsorLink", "Sponsors",
            DiagnosticSeverity.Info, true),
        new DiagnosticDescriptor(
            "SL002", "Report Sponsoring Status", "Fake to get it to call us", "Sponsors",
            DiagnosticSeverity.Warning, true)
        );

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        context.RegisterCompilationAction(c =>
        {
            var installed = c.Options.AdditionalFiles.Where(x =>
            {
                var options = c.Options.AnalyzerConfigOptionsProvider.GetOptions(x);
                // In release builds, we'll have a single such item, since we IL-merge the analyzer.
                return options.TryGetValue("build_metadata.Analyzer.ItemType", out var itemType) &&
                       options.TryGetValue("build_metadata.Analyzer.NuGetPackageId", out var packageId) &&
                       itemType == "Analyzer" &&
                       packageId == "SponsorableLib";
            }).Select(x => File.GetLastWriteTime(x.Path)).OrderByDescending(x => x).FirstOrDefault();

            var status = Diagnostics.GetOrSetStatus(() => c.Options);

            var location = Location.None;
            if (c.Options.AnalyzerConfigOptionsProvider.GlobalOptions.TryGetValue("build_property.MSBuildProjectFullPath", out var value))
                location = Location.Create(value, new TextSpan(), new LinePositionSpan());

            c.ReportDiagnostic(Diagnostic.Create(SupportedDiagnostics[0], location, status.ToString()));

            if (installed != default)
                Tracing.Trace($"Status: {status}, Installed: {(DateTime.Now - installed).Humanize()} ago");
            else
                Tracing.Trace($"Status: {status}, unknown install time");
        });
    }
}