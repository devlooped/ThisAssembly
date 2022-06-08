using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace ThisAssembly
{
    [Generator]
    public class AssemblyInfoGenerator : ISourceGenerator
    {
        readonly HashSet<string> attributes = new HashSet<string>(new[]
        {
            nameof(AssemblyConfigurationAttribute),
            nameof(AssemblyCompanyAttribute),
            nameof(AssemblyCopyrightAttribute),
            nameof(AssemblyTitleAttribute),
            nameof(AssemblyProductAttribute),
            nameof(AssemblyVersionAttribute),
            nameof(AssemblyInformationalVersionAttribute),
            nameof(AssemblyFileVersionAttribute),
        });

        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            var metadata = context.Compilation.Assembly.GetAttributes()
                .Where(x => !string.IsNullOrEmpty(x.AttributeClass?.Name) && attributes.Contains(x.AttributeClass!.Name))
                .Where(x => x.ConstructorArguments.Length == 1)
                .Select(x => new KeyValuePair<string, string?>(x.AttributeClass!.Name.Substring(8).Replace("Attribute", ""), (string?)x.ConstructorArguments[0].Value))
                .Distinct(new KeyPairComparer())
                .ToDictionary(x => x.Key, x => x.Value ?? "");

            var model = new Model(metadata);
            var language = context.ParseOptions.Language;
            var file = language.Replace("#", "Sharp") + ".sbntxt";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);
            var output = template.Render(model, member => member.Name);

            context.AddSource("ThisAssembly.Info", SourceText.From(output, Encoding.UTF8));
        }

        class KeyPairComparer : IEqualityComparer<KeyValuePair<string, string?>>
        {
            public bool Equals(KeyValuePair<string, string?> x, KeyValuePair<string, string?> y) => x.Key == y.Key;

            public int GetHashCode(KeyValuePair<string, string?> obj) => obj.Key.GetHashCode();
        }
    }
}
