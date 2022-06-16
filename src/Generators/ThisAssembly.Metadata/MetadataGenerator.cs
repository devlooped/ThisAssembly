﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using Utilities;

namespace ThisAssembly
{
    [Generator]
    public class MetadataGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            var metadata = context.Compilation.Assembly.GetAttributes()
                .Where(x => x.AttributeClass?.Name == nameof(System.Reflection.AssemblyMetadataAttribute) &&
                    Microsoft.CodeAnalysis.CSharp.SyntaxFacts.IsValidIdentifier((string)x.ConstructorArguments[0].Value!))
                .Select(x => new KeyValuePair<string, string>((string)x.ConstructorArguments[0].Value!, (string)x.ConstructorArguments[1].Value!))
                .Distinct(new KeyValueComparer<string,string>())
                .ToDictionary(x => x.Key, x => x.Value);

            var model = new Model(metadata);
            var language = context.ParseOptions.Language;
            var file = language.Replace("#", "Sharp") + ".sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);
            var output = template.Render(model, member => member.Name);

            context.AddSource("ThisAssembly.Metadata", SourceText.From(output, Encoding.UTF8));
        }
    }
}