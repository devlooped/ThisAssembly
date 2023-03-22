using Devlooped;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ThisAssembly;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
class SponsorLinker : SponsorLink
{
    public SponsorLinker() : base(SponsorLinkSettings.Create(
        "devlooped", "ThisAssembly",
        packageId: "ThisAssembly.Constants",
        version: typeof(SponsorLinker).Assembly.GetName().Version.ToString(3)
#if DEBUG
        , quietDays: 0
#endif
        ))
    { }
}
