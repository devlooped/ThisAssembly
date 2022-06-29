using System;
using System.Collections.Generic;

namespace CodeGeneration.Model;
abstract class LanguageItem
{
    LanguageItem? _parent = null;

    protected LanguageItem(string name) => Name = name;

    public string Name { get; }

    public LanguageItem? Parent => _parent;

    public string FullName
    {
        get
        {
            var hierarchy = new Stack<string>();
            var item = this;
            while (item is not null)
            {
                hierarchy.Push(item.Name);
                item = item.Parent;
            }

            return string.Join(".", hierarchy);
        }
    }

    protected void SetParentOf(LanguageItem item)
    {
        if (item._parent is not null)
        {
            throw new InvalidOperationException($"{item.GetType().Name} '{item.Name}' already has a parent.");
        }

        item._parent = this;
    }
}
