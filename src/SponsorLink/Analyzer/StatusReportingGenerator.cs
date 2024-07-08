using Devlooped.Sponsors;
using Microsoft.CodeAnalysis;
using static Devlooped.Sponsors.SponsorLink;

namespace Analyzer;

[Generator]
public class StatusReportingGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            // this is required to ensure status is registered properly independently 
            // of analyzer runs.
            context.GetSponsorAdditionalFiles().Combine(context.AnalyzerConfigOptionsProvider),
            (spc, source) =>
            {
                var (manifests, options) = source;
                var status = Diagnostics.GetOrSetStatus(manifests, options);
                spc.AddSource("StatusReporting.cs", $"// Status: {status}");
            });
    }
}
