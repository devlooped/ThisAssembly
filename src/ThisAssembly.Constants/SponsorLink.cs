using Devlooped;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ThisAssembly;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[Generator]
class SponsorLinker : SponsorLink
{
    public SponsorLinker() : base(SponsorLinkSettings.Create("devlooped", "ThisAssembly", "ThisAssembly.Constants"))
    { }
}
