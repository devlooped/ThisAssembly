using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

[DebuggerDisplay("Values = {RootArea.Values.Count}")]
record Model(Area RootArea)
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

        foreach (var part in parts.AsSpan()[..end])
        {
            var partStr = PathSanitizer.Sanitize(part);
            area.NestedArea = new Area(partStr);
            area = area.NestedArea;
        }

        area.Resources = resources;
        return root;
    }
}

[DebuggerDisplay("{Name}")]
record Resource(string Name, string? Comment, bool IsText)
{
    public string? Path { get; set; }
};