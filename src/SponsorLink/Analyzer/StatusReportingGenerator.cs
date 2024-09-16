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
            context.GetStatusOptions(),
            (spc, source) =>
            {
                var status = Diagnostics.GetOrSetStatus(source);
                spc.AddSource("StatusReporting.cs",
                    $"""
                    // Status: {status}
                    // DesignTimeBuild: {source.GlobalOptions.IsDesignTimeBuild()}
                    """);
            });
    }
}
