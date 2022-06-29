using System.Collections.Generic;
using System.Linq;

namespace CodeGeneration.Model;

sealed class Class : LanguageItem
{
    readonly List<Constant> _constants = new();
    readonly List<Class> _nestedClasses = new();

    public Class(string name, PartialTypeKind partialTypeKind)
        : base(name)
    {
        PartialTypeKind = partialTypeKind;
    }

    public PartialTypeKind PartialTypeKind{ get; }

    public string? XmlSummary { get; init; } = null;

    public IEnumerable<Constant> Constants
    {
        get => _constants;
        init => _constants.AddRange(value);
    }

    public IEnumerable<Class> NestedClasses
    {
        get => _nestedClasses;
        init => _nestedClasses.AddRange(value);
    }

    public void Add(Constant constant)
    {

        SetParentOf(constant);
        _constants.Add(constant);
    }

    public void Add(Class cls)
    {
        SetParentOf(cls);
        _nestedClasses.Add(cls);
    }
}
