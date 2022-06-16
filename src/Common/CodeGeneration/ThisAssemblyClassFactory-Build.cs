using System;
using CodeGeneration.CSharp;
using CodeGeneration.Model;
using CodeGeneration.VisualBasic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;

namespace CodeGeneration;

partial class ThisAssemblyClassFactory
{
    public static SourceText Build(
        ThisAssemblyClass thisAssemblyClass,
        ThisAssemblyClassFactoryOptions options,
        ParseOptions parseOptions)
    {
        var factory = parseOptions switch
        {
            CSharpParseOptions => new CSharpThisAssemblyClassFactory(options) as ThisAssemblyClassFactory,
            VisualBasicParseOptions => new VisualBasicThisAssemblyClassFactory(options),
            null => throw new ArgumentNullException(nameof(parseOptions)),
            _ => throw new ArgumentException($"Unsupported language '{parseOptions.Language}'.", nameof(parseOptions)),
        };

        factory.BuildThisAssemblyClass(thisAssemblyClass);
        return factory.GetSourceText(parseOptions);
    }

    protected void BuildThisAssemblyClass(ThisAssemblyClass thisAssemblyClass)
    {
        BeginSourceFile();
        BeginGlobalNamespace();
        BuildClass(
            Options.ThisAssemblyClassName,
            thisAssemblyClass.IsMainPart && Options.GeneratePublicClass,
            thisAssemblyClass.IsMainPart,
            thisAssemblyClass);
        EndGlobalNamespace();
    }

    void BuildClass(string name, bool isPublic, bool isStatic, ClassBase cls)
    {
        if (!string.IsNullOrWhiteSpace(cls.XmlSummary))
        {
            XmlSummary(cls.XmlSummary!);
        }

        BeginClass(name, isPublic, isStatic && Options.GenerateStaticClasses);
        foreach (var constant in cls.Constants)
        {
            BuildClassConstant(constant);
        }

        foreach (var nestedClass in cls.NestedClasses)
        {
            BuildClass(nestedClass.Name, true, true, nestedClass);
        }

        EndClass();
    }

    void BuildClassConstant(ClassConstant constant)
    {
        if (!string.IsNullOrWhiteSpace(constant.XmlSummary))
        {
            XmlSummary(constant.XmlSummary!);
        }

        if (Options.GenerateAllConstantsAsFields)
        {
            PublicStaticReadOnlyField(constant.Name, constant.Value);
        }
        else
        {
            PublicConstant(constant.Name, constant.Value);
        }
    }
}
