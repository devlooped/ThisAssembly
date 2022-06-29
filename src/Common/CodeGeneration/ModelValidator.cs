using System.Collections.Generic;

namespace CodeGeneration;

abstract partial class ModelValidator
{
    protected abstract IEqualityComparer<string> IdentifierComparer { get; }

    protected abstract bool IsValidIdentifier(string identifier);
}
