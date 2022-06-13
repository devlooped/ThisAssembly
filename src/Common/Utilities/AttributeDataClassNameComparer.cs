using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Utilities;

sealed class AttributeDataClassNameComparer : IEqualityComparer<AttributeData>
{
    public static readonly AttributeDataClassNameComparer Instance = new();

    public bool Equals(AttributeData x, AttributeData y)
        => string.Equals(x.AttributeClass?.Name ?? string.Empty, y.AttributeClass?.Name ?? string.Empty, StringComparison.OrdinalIgnoreCase);

    public int GetHashCode(AttributeData obj) => StringComparer.OrdinalIgnoreCase.GetHashCode(obj.AttributeClass?.Name ?? string.Empty);
}
