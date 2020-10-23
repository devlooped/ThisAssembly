using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

[DebuggerDisplay("Values = {RootArea.Values.Count}")]
record Model(Area RootArea)
{
    public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
}

[DebuggerDisplay("Name = {Name}, NestedAreas = {NestedAreas.Count}, Values = {Values.Count}")]
record Area(string Name, string Prefix)
{
    public List<Area> NestedAreas { get; init; } = new();
    public List<Constant> Values { get; init; } = new();

    public static Area Load(List<Constant> constants, string rootArea = "Constants")
    {
        var root = new Area(rootArea, "");

        foreach (var constant in constants)
        {
            //  Splits: ([area].)*[name]
            var parts = constant.Name.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length <= 1)
            {
                root.Values.Add(new Constant(constant.Name, constant.Value, constant.Comment));
            }
            else
            {
                var area = GetArea(root, parts.Take(parts.Length - 1));
                var value = new Constant(parts.Skip(parts.Length - 1).First(), constant.Value, constant.Comment);

                area.Values.Add(value);
            }
        }

        SortArea(root);
        return root;
    }

    static void SortArea(Area area)
    {
        area.Values.Sort((left, right) => left.Name.CompareTo(right.Name));
        foreach (var nested in area.NestedAreas)
            SortArea(nested);
    }

    static Area GetArea(Area area, IEnumerable<string> areaPath)
    {
        var currentArea = area;
        foreach (var areaName in areaPath)
        {
            var existing = currentArea.NestedAreas.FirstOrDefault(a => a.Name == areaName);
            if (existing == null)
            {
                if (currentArea.Values.Any(v => v.Name == areaName))
                    throw new ArgumentException(string.Format(
                        "Area name '{0}' is already in use as a value name under area '{1}'.",
                        areaName, currentArea.Name));

                existing = new Area(areaName, currentArea.Prefix + areaName + ".");
                currentArea.NestedAreas.Add(existing);
            }

            currentArea = existing;
        }

        return currentArea;
    }
}

[DebuggerDisplay("{Name} = {Value}")]
record Constant(string Name, string Value, string Comment);