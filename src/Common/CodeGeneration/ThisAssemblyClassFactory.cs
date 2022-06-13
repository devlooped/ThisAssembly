using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CodeGeneration;

abstract partial class ThisAssemblyClassFactory
{
    protected ThisAssemblyClassFactory(ThisAssemblyClassFactoryOptions options)
    {
        Text = new StringBuilder(4096);
        Options = options;
    }

    protected ThisAssemblyClassFactoryOptions Options { get; }
    
    protected StringBuilder Text { get; }

    protected abstract void BeginSourceFile();

    protected abstract void BeginGlobalNamespace();

    protected abstract void EndGlobalNamespace();

    protected abstract void XmlSummary(string text);

    protected abstract void BeginClass(string name, bool isPublic, bool isStatic);

    protected abstract void EndClass();

    protected abstract void PublicConstant(string name, string? value);

    protected abstract void PublicStaticReadOnlyField(string name, string? value);

    protected abstract SourceText GetSourceText(ParseOptions parseOptions);
}
