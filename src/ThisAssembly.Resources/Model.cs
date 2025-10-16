using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ThisAssembly;

[DebuggerDisplay("Values = {RootArea.Values.Count}")]
record Model(Area RootArea, string? Namespace, bool IsPublic)
{
    public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
    public string Visibility => IsPublic ? "public " : "";
}

[DebuggerDisplay("Name = {Name}")]
record Area(string Name)
{
    Area? nestedArea = null;
    Area? parent = null;
    string? comment = null;

    public string? Comment
    {
        get => comment ?? $"Provides access to embedded resources under {Path}";
        set => comment = value;
    }

    public Area? NestedArea
    {
        get => nestedArea;
        set
        {
            nestedArea = value;
            if (nestedArea != null)
                nestedArea.parent = this;
        }
    }

    public string Path => parent == null ? Name : $"{parent.Path}/{Name}";

    public IEnumerable<Resource>? Resources { get; private set; }

    public static Area Load(string basePath, List<Resource> resources, string rootArea = "Resources", string comment = "Provides access to embedded resources.")
    {
        var root = new Area(rootArea) { Comment = comment };

        //  Splits: ([area].)*[name]
        var area = root;
        var parts = basePath.Split(new[] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries);
        var end = resources.Count == 1 ? ^1 : ^0;

        var parent = "Resources";
        foreach (var part in parts.AsSpan()[..end])
        {
            var partStr = PathSanitizer.Sanitize(part, parent);
            parent = partStr;
            area = area.NestedArea = new Area(partStr);
        }

        area.Resources = resources
            .Select(r => r with
            {
                Name = PathSanitizer.Sanitize(r.Name, parent),
            });
        return root;
    }
}

[DebuggerDisplay("{Name}")]
record Resource(string Name, string? Comment, bool IsText, string Path);