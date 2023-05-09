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

namespace ThisAssembly
{
    [Generator]
    public class AssemblyInfoGenerator : IIncrementalGenerator
    {
        static readonly HashSet<string> attributes = new()
        {
            nameof(AssemblyConfigurationAttribute),
            nameof(AssemblyCompanyAttribute),
            nameof(AssemblyCopyrightAttribute),
            nameof(AssemblyTitleAttribute),
            nameof(AssemblyDescriptionAttribute),
            nameof(AssemblyProductAttribute),
            nameof(AssemblyVersionAttribute),
            nameof(AssemblyInformationalVersionAttribute),
            nameof(AssemblyFileVersionAttribute),
        };

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var metadata = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => s is AttributeSyntax,
                    transform: static (ctx, token) => GetSemanticTargetForGeneration(ctx, token))
                .Where(static m => m is not null)
                .Select(static (m, _) => m!.Value)
                .Collect();

            context.RegisterSourceOutput(
                metadata.Combine(context.CompilationProvider.Select((s, _) => s.Language)),
                GenerateSource);
        }

        static KeyValuePair<string, string>? GetSemanticTargetForGeneration(GeneratorSyntaxContext ctx, CancellationToken token)
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

            var key = attributeType.Name[8..^9];
            var expr = attributeNode.ArgumentList!.Arguments[0].Expression;
            var value = ctx.SemanticModel.GetConstantValue(expr, token).ToString();
            return new KeyValuePair<string, string>(key, value);
        }

        static void GenerateSource(SourceProductionContext spc, (ImmutableArray<KeyValuePair<string, string>> attributes, string language) arg2)
        {
            var (attributes, language) = arg2;

            var model = new Model(attributes.ToList());
            var file = language.Replace("#", "Sharp") + ".sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);
            var output = template.Render(model, member => member.Name);

            spc.AddSource(
                "ThisAssembly.AssemblyInfo.g.cs",
                SourceText.From(output, Encoding.UTF8));
        }
    }
}
