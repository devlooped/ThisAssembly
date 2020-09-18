using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;

namespace ThisAssembly
{
    static class DesignTimeFixExtension
    {
        public static void ApplyDesignTimeFix(this GeneratorExecutionContext context, string content, string hintName, string language)
        {
            var includeFix = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.IncludeSourceGeneratorIntellisenseFix", out var raw) &&
                bool.TryParse(raw, out var value) &&
                value;

            if (includeFix)
            {
                if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ThisAssemblyIntellisenseFixExtension", out var extension))
                    extension = ".ta.g." + (language == LanguageNames.CSharp ? "cs" : language == LanguageNames.VisualBasic ? "vb" : "fs");

                if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.IntermediateOutputPath", out var intermediate))
                    throw new NotSupportedException();

                var path = Path.Combine(intermediate, hintName + extension);
                Directory.CreateDirectory(intermediate);
                File.WriteAllText(path, content, Encoding.UTF8);
            }
        }
    }
}
