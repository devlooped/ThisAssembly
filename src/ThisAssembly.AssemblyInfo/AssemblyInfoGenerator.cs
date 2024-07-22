using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace ThisAssembly;

[Generator(LanguageNames.CSharp)]
public class AssemblyInfoGenerator : IIncrementalGenerator
{
    static readonly HashSet<string> attributes =
    [
        nameof(AssemblyConfigurationAttribute),
        nameof(AssemblyCompanyAttribute),
        nameof(AssemblyCopyrightAttribute),
        nameof(AssemblyTitleAttribute),
        nameof(AssemblyDescriptionAttribute),
        nameof(AssemblyProductAttribute),
        nameof(AssemblyVersionAttribute),
        nameof(AssemblyInformationalVersionAttribute),
        nameof(AssemblyFileVersionAttribute),
    ];

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var metadata = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => s is AttributeSyntax,
                transform: static (ctx, token) => GetAttributes(ctx, token))
            .Where(static m => m is not null)
            .Select(static (m, _) => m!.Value)
            .Collect();

        // Read the ThisAssemblyNamespace property or default to null
        var right = context.AnalyzerConfigOptionsProvider
            .Select((c, t) => c.GlobalOptions.TryGetValue("build_property.ThisAssemblyNamespace", out var ns) && !string.IsNullOrEmpty(ns) ? ns : null)
            .Combine(context.ParseOptionsProvider);

        context.RegisterSourceOutput(
            metadata.Combine(right),
            GenerateSource);
    }

    static KeyValuePair<string, string>? GetAttributes(GeneratorSyntaxContext ctx, CancellationToken token)
    {
        var attributeNode = (AttributeSyntax)ctx.Node;

        if (attributeNode.ArgumentList?.Arguments.Count != 1)
            return null;

        if (ctx.SemanticModel.GetSymbolInfo(attributeNode, token).Symbol is not IMethodSymbol ctor)
            return null;

        var attributeType = ctor.ContainingType;
        if (attributeType == null)
            return null;

        if (!attributes.Contains(attributeType.Name))
            return null;

        // Remove the "Assembly" prefix and "Attribute" suffix.
        var key = attributeType.Name[8..^9];
        var expr = attributeNode.ArgumentList!.Arguments[0].Expression;
        var value = ctx.SemanticModel.GetConstantValue(expr, token).ToString();
        // KeyValuePair is a struct and properly equatable for optimal caching in the generator.
        return new KeyValuePair<string, string>(key, value);
    }

    static void GenerateSource(SourceProductionContext spc,
        (ImmutableArray<KeyValuePair<string, string>> attributes, (string? ns, ParseOptions parse)) arg)
    {
        var (attributes, (ns, parse)) = arg;

        var model = new Model(attributes.ToList(), ns);
        if (parse is CSharpParseOptions cs && (int)cs.LanguageVersion >= 1100)
            model.RawStrings = true;

        var file = parse.Language.Replace("#", "Sharp") + ".sbntxt";
        var template = Template.Parse(EmbeddedResource.GetContent(file), file);
        var output = template.Render(model, member => member.Name);

        spc.AddSource(
            "ThisAssembly.AssemblyInfo.g.cs",
            SourceText.From(output, Encoding.UTF8));
    }
}
