using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using Scriban.Parsing;

namespace ThisAssembly
{
    [Generator]
    public class MetadataGenerator : IIncrementalGenerator
    {
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
                metadata.Combine(context.ParseOptionsProvider),
                GenerateSource);
        }

        static KeyValuePair<string, string>? GetSemanticTargetForGeneration(GeneratorSyntaxContext ctx, CancellationToken token)
        {
            var attributeNode = (AttributeSyntax)ctx.Node;

            if (attributeNode.ArgumentList?.Arguments.Count != 2)
                return null;

            if (ctx.SemanticModel.GetSymbolInfo(attributeNode, token).Symbol is not IMethodSymbol ctor)
                return null;

            var attributeType = ctor.ContainingType;
            if (attributeType == null)
                return null;

            if (attributeType.Name != nameof(System.Reflection.AssemblyMetadataAttribute))
                return null;

            var keyExpr = attributeNode.ArgumentList!.Arguments[0].Expression;
            var key = ctx.SemanticModel.GetConstantValue(keyExpr, token).ToString();
            var valueExpr = attributeNode.ArgumentList!.Arguments[1].Expression;
            var value = ctx.SemanticModel.GetConstantValue(valueExpr, token).ToString();
            return new KeyValuePair<string, string>(key, value);
        }

        void GenerateSource(SourceProductionContext spc, (ImmutableArray<KeyValuePair<string, string>> attributes, ParseOptions parse) arg)
        {
            var (attributes, parse) = arg;

            var model = new Model(attributes.ToList());
            if (parse is CSharpParseOptions cs && (int)cs.LanguageVersion >= 11)
                model.RawStrings = true;

            var file = parse.Language.Replace("#", "Sharp") + ".sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);
            var output = template.Render(model, member => member.Name);

            spc.AddSource(
                "ThisAssembly.Metadata.g.cs",
                SourceText.From(output, Encoding.UTF8));
        }
    }
}
