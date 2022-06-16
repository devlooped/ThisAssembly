using System.Collections.Generic;

namespace CodeGeneration.Model;

abstract record ClassBase
{
    public string? XmlSummary { get; init; } = null;

    public List<ClassConstant> Constants { get; init; } = new();

    public List<NestedClass> NestedClasses { get; init; } = new();
}
