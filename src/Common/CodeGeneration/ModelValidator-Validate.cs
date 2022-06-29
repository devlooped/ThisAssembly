using System;
using System.Collections.Generic;
using CodeGeneration.CSharp;
using CodeGeneration.Model;
using CodeGeneration.VisualBasic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.VisualBasic;

namespace CodeGeneration;

partial class ModelValidator
{
    public static void Validate(Class thisAssemblyClass, ParseOptions parseOptions)
    {
        var context = Create(parseOptions);
        context.ValidateThisAssemblyClass(thisAssemblyClass);
    }

    static ModelValidator Create(ParseOptions parseOptions)
        => parseOptions switch
        {
            CSharpParseOptions => new CSharpModelValidator(),
            VisualBasicParseOptions => new VisualBasicModelValidator(),
            null => throw new ArgumentNullException(nameof(parseOptions)),
            _ => throw new ArgumentException($"Unsupported language '{parseOptions.Language}'.", nameof(parseOptions)),
        };

    void ValidateThisAssemblyClass(Class thisAssemblyClass)
    {
        if (!IsValidIdentifier(thisAssemblyClass.Name))
        {
            Diagnostics.ThrowInvalidThisAssemblyClassName(thisAssemblyClass.Name);
        }

        ValidateClassMembers(thisAssemblyClass);
    }

    void ValidateNestedClass(Class nestedClass)
    {
        if (!IsValidIdentifier(nestedClass.Name))
        {
            Diagnostics.ThrowInvalidNestedClassName(nestedClass.Parent, nestedClass.Name);
        }

        ValidateClassMembers(nestedClass);
    }

    void ValidateClassMembers(Class cls)
    {
        var memberNames = new HashSet<string>(IdentifierComparer);
        foreach (var constant in cls.Constants)
        {
            ValidateConstant(constant);
            if (!memberNames.Add(constant.Name))
            {
                Diagnostics.ThrowDuplicateMemberName(cls, constant.Name);
            }
        }

        foreach (var nestedClass in cls.NestedClasses)
        {
            ValidateNestedClass(nestedClass);
            if (!memberNames.Add(nestedClass.Name))
            {
                Diagnostics.ThrowDuplicateMemberName(cls, nestedClass.Name);
            }
        }
    }

    void ValidateConstant(Constant constant)
    {
        if (!IsValidIdentifier(constant.Name))
        {
            Diagnostics.ThrowInvalidConstantName(constant.Parent, constant.Name);
        }
    }
}
