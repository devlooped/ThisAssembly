using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using CodeGeneration;
using CodeGeneration.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ThisAssembly
{
    [Generator]
    public class ConstantsGenerator : ThisAssemblyGenerator
    {
        protected override string GeneratorName => "ThisAssembly.Constants";

        protected override void InitializeGenerator(IncrementalGeneratorInitializationContext context)
        {
            var constantDataProvider = context.AdditionalTextsProvider
                .Combine(context.AnalyzerConfigOptionsProvider)
                .Select((data, cancellationToken) =>
                {
                    var (file, optionsProvider) = data;
                    var name = Path.GetFileName(file.Path);
                    var options = optionsProvider.GetOptions(file);
                    _ = options.TryGetValue("build_metadata.AdditionalFiles.SourceItemType", out var itemType);
                    if (itemType != "Constant")
                    {
                        // The call to Where below will filter out null values;
                        // the ! here is for the sake of nullability analysis.
                        return null!;
                    }

                    _ = options.TryGetValue("build_metadata.Constant.Value", out var value);
                    _ = options.TryGetValue("build_metadata.Constant.Comment", out var comment);
                    var xmlSummary = $"{name} = {value}";
                    if (!string.IsNullOrWhiteSpace(comment))
                    {
                        xmlSummary = $"<p>{xmlSummary}</p>\n<p>{comment}</p>";
                    }

                    return new ConstantDefinition(name, value,  xmlSummary);
                })
                .Where(a => a is not null)
                .Collect();

            var provider = context.ParseOptionsProvider
                .Combine(OptionsProvider)
                .Combine(constantDataProvider);

            context.RegisterSourceOutput(provider, (ctx, data) =>
            {
                try
                {
                    var ((parseOptions, options), constantDefinitions) = data;
                    var constantsClass = new Class("Constants", PartialTypeKind.MainPart)
                    {
                        XmlSummary = "Provides access to MSBuild Constant items.",
                    };

                    foreach (var definition in constantDefinitions)
                    {
                        var names = definition.Path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                        if (names.Length == 0)
                        {
                            Diagnostics.ThrowNamelessConstant(definition.Path, definition.Value);
                        }

                        var cls = constantsClass;
                        for (var i = 0; i < names.Length - 1; i++)
                        {
                            var name = names[i];
                            var nestedClass = cls.NestedClasses.FirstOrDefault(static c => c.Name == name);
                            if (nestedClass is null)
                            {
                                nestedClass = new Class(name, PartialTypeKind.MainPart);
                                cls.Add(nestedClass);
                            }

                            cls = nestedClass;
                        }

                        cls.Add(new Constant(names[^1], definition.Value) { XmlSummary = definition.XmlSummary });
                    }

                    var model = new Class(options.ThisAssemblyClassName, PartialTypeKind.OtherPart);
                    model.Add(constantsClass);
                    var sourceText = CodeFactory.Build(model, options, parseOptions);
                    ctx.AddSource(options.ThisAssemblyClassName + ".Constants", sourceText);
                }
                catch (DiagnosticException e)
                {
                    e.Report(ctx);
                }
            });
        }
    }
}
