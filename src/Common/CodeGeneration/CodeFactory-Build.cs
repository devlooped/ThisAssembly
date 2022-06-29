using System;
using CodeGeneration.CSharp;
using CodeGeneration.Model;
using CodeGeneration.VisualBasic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;

namespace CodeGeneration;

partial class CodeFactory
{
    public static SourceText Build(
        Class thisAssemblyClass,
        ThisAssemblyOptions options,
        ParseOptions parseOptions)
    {
        ModelValidator.Validate(thisAssemblyClass, parseOptions);
        var factory = Create(options, parseOptions);
        factory.BuildThisAssemblyClass(thisAssemblyClass);
        return factory.GetSourceText(parseOptions);
    }

    static CodeFactory Create(ThisAssemblyOptions options, ParseOptions parseOptions)
        => parseOptions switch
        {
            CSharpParseOptions => new CSharpCodeFactory(options),
            VisualBasicParseOptions => new VisualBasicCodeFactory(options),
            null => throw new ArgumentNullException(nameof(parseOptions)),
            _ => throw new ArgumentException($"Unsupported language '{parseOptions.Language}'.", nameof(parseOptions)),
        };

    protected void BuildThisAssemblyClass(Class thisAssemblyClass)
    {
        BeginSourceFile();
        BeginGlobalNamespace();
        BuildClass(thisAssemblyClass, Options.GeneratePublicClass);
        EndGlobalNamespace();
    }

    void BuildClass(Class cls, bool isPublic)
    {
        if (cls.PartialTypeKind != PartialTypeKind.OtherPart && !string.IsNullOrWhiteSpace(cls.XmlSummary))
        {
            XmlSummary(cls.XmlSummary!);
        }

        BeginClass(
            cls.Name,
            cls.PartialTypeKind != PartialTypeKind.NotPartial,
            isPublic && cls.PartialTypeKind != PartialTypeKind.OtherPart,
            Options.GenerateStaticClasses && cls.PartialTypeKind != PartialTypeKind.OtherPart);

        foreach (var constant in cls.Constants)
        {
            BuildConstant(constant);
        }

        foreach (var nestedClass in cls.NestedClasses)
        {
            BuildClass(nestedClass, true);
        }

        EndClass();
    }


    void BuildConstant(Constant constant)
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
