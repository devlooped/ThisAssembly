using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

[DebuggerDisplay("Values = {RootArea.Values.Count}")]
record Model(Area RootArea)
{
    public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
}

[DebuggerDisplay("Name = {Name}")]
record Area(string Name)
{
    public Area? NestedArea { get; private set; } 
    public Resource? Resource { get; private set; }

    public static Area Load(Resource resource, string rootArea = "Resources")
    {
        var root = new Area(rootArea);

        //  Splits: ([area].)*[name]
        var area = root;
        var parts = resource.Name.Split(new[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts.AsSpan()[..^1])
        {
            area.NestedArea = new Area(part);
            area = area.NestedArea;
        }

        area.Resource = resource with { Name = Path.GetFileNameWithoutExtension(parts[^1]), Path = resource.Name, };
        return root;
    }
}

[DebuggerDisplay("{Name}")]
record Resource(string Name, string? Comment, bool IsText)
{
    public string? Path { get; set; }
};