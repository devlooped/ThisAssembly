﻿using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace ThisAssembly
{
    [Generator]
    public class StringsGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            context.CheckDebugger("ThisAssemblyStrings");

            var resourceFiles = context.AdditionalFiles
                    .Where(f => context.AnalyzerConfigOptions
                        .GetOptions(f)
                        .TryGetValue("build_metadata.AdditionalFiles.SourceItemType", out var itemType)
                        && itemType == "EmbeddedResource");

            if (!resourceFiles.Any())
                return;

            var language = context.ParseOptions.Language;
            var file = language.Replace("#", "Sharp") + ".sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);

            foreach (var resourceFile in resourceFiles)
            {
                var options = context.AnalyzerConfigOptions.GetOptions(resourceFile);
                if (!options.TryGetValue("build_metadata.AdditionalFiles.ManifestResourceName", out var resourceName) ||
                    string.IsNullOrEmpty(resourceName))
                    continue;

                var rootArea = ResourceFile.Load(resourceFile.Path, "Strings");
                var model = new Model(rootArea, resourceName);
                model = model with { ResourceName = resourceName };

                var output = template.Render(model, member => member.Name);

                // Apply formatting since indenting isn't that nice in Scriban when rendering nested 
                // structures via functions.
                if (language == LanguageNames.CSharp)
                {
                    output = Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseCompilationUnit(output)
                        .NormalizeWhitespace()
                        .GetText()
                        .ToString();
                }
                //else if (language == LanguageNames.VisualBasic)
                //{
                //    output = Microsoft.CodeAnalysis.VisualBasic.SyntaxFactory.ParseCompilationUnit(output)
                //        .NormalizeWhitespace()
                //        .GetText()
                //        .ToString();
                //}

                context.AddSource(resourceName, SourceText.From(output, Encoding.UTF8));
            }

            var extension = language == LanguageNames.CSharp ? "cs" : language == LanguageNames.VisualBasic ? "vb" : "fs";
            var strings = EmbeddedResource.GetContent("ThisAssembly.Strings." + extension);

            context.AddSource("ThisAssembly.Strings", SourceText.From(strings, Encoding.UTF8));
        }
    }
}
