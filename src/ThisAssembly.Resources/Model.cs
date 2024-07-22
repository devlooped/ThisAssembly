using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ThisAssembly;

[DebuggerDisplay("Values = {RootArea.Values.Count}")]
record Model(Area RootArea, string? Namespace)
{
    public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
}

[DebuggerDisplay("Name = {Name}")]
record Area(string Name)
{
    public Area? NestedArea { get; private set; }
    public IEnumerable<Resource>? Resources { get; private set; }

    public static Area Load(string basePath, List<Resource> resources, string rootArea = "Resources")
    {
        var root = new Area(rootArea);

        //  Splits: ([area].)*[name]
        var area = root;
        var parts = basePath.Split(new[] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries);
        var end = resources.Count == 1 ? ^1 : ^0;

        var parent = "Resources";
        foreach (var part in parts.AsSpan()[..end])
        {
            var partStr = PathSanitizer.Sanitize(part, parent);
            parent = partStr;

            area.NestedArea = new Area(partStr);
            area = area.NestedArea;
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