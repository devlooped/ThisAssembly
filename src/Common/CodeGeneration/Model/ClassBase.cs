using System.Collections.Generic;
using System.Collections.Immutable;

namespace CodeGeneration.Model;

abstract record ClassBase
{
    public string? XmlSummary { get; init; } = null;

    public ImmutableArray<ClassConstant> Constants { get; init; } = ImmutableArray<ClassConstant>.Empty;

    public List<ClassConstant> ConstantList
    {
        init => Constants = value.ToImmutableArray();
    }

    public ImmutableArray<NestedClass> NestedClasses { get; init; } = ImmutableArray<NestedClass>.Empty;

    public List<NestedClass> NestedClassList
    {
        init => NestedClasses = value.ToImmutableArray();
    }
}
