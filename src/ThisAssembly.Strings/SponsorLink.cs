using Devlooped;
using Microsoft.CodeAnalysis;

namespace ThisAssembly;

[Generator]
class Generator : IIncrementalGenerator
{
    readonly SponsorLink link;

    public Generator() => link = new SponsorLink("devlooped", "ThisAssembly.Strings");

    public void Initialize(IncrementalGeneratorInitializationContext context) => link.Initialize(context);
}