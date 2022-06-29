using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGeneration.CSharp;

sealed class CSharpModelValidator : ModelValidator
{
    protected override IEqualityComparer<string> IdentifierComparer => StringComparer.Ordinal;

    protected override bool IsValidIdentifier(string identifier)
        => SyntaxFacts.IsValidIdentifier(identifier) && SyntaxFacts.GetKeywordKind(identifier) == SyntaxKind.None;
}
