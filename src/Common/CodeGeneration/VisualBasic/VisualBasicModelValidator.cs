using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;

namespace CodeGeneration.VisualBasic;

sealed class VisualBasicModelValidator : ModelValidator
{
    protected override IEqualityComparer<string> IdentifierComparer => CaseInsensitiveComparison.Comparer;

    protected override bool IsValidIdentifier(string identifier)
        => SyntaxFacts.IsValidIdentifier(identifier) && SyntaxFacts.GetKeywordKind(identifier) == SyntaxKind.None;
}
