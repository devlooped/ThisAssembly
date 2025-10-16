using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace ThisAssembly;

[DebuggerDisplay("Values = {RootArea.Values.Count}")]
record Model(Area RootArea, string? Namespace, bool IsPublic)
{
    public bool RawStrings { get; set; } = false;
    public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
    public string Visibility => IsPublic ? "public " : "";
    public string Modifier => IsPublic ? "static" : "const";
    public string Lambda => IsPublic ? ">" : "";
}

[DebuggerDisplay("Name = {Name}, NestedAreas = {NestedAreas.Count}, Values = {Values.Count}")]
record Area(string Name, string Prefix)
{
    string? comment = null;
    Area? parent = null;

    public string Comment
    {
        get => comment ?? $"Provides access to constants under {Path}";
        set => comment = value;
    }

    public string Path => parent == null ? Name : $"{parent.Path}/{Name}";

    public List<Area> NestedAreas { get; init; } = [];
    public List<Constant> Values { get; init; } = [];

    static string EscapeIdentifier(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
        {
            return "_";
        }

        var replaced = identifier
            .Select(c => SyntaxFacts.IsIdentifierPartCharacter(c) ? c : '_')
            .ToArray();

        var result = Regex.Replace(new string(replaced), "(_)+", "_");

        if (!SyntaxFacts.IsIdentifierStartCharacter(result[0]))
        {
            result = "_" + result;
        }

        return result;
    }

    public static Area Load(List<Constant> constants, string rootArea = "Constants", string comment = "Provides access project-defined constants.")
    {
        var root = new Area(rootArea, "") { Comment = comment };

        foreach (var constant in constants)
        {
            //  Splits: ([area].)*[name]
            var parts = constant.Name.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries)
                .Select(EscapeIdentifier)
                .ToArray();

            if (parts.Length <= 1)
            {
                root.Values.Add(constant with { Name = EscapeIdentifier(constant.Name) });
            }
            else
            {
                var area = GetArea(root, parts.Take(parts.Length - 1));
                var value = constant with { Name = parts.Skip(parts.Length - 1).First() };

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

                existing = new Area(areaName, currentArea.Prefix + areaName + ".")
                {
                    parent = currentArea
                };
                currentArea.NestedAreas.Add(existing);
            }

            currentArea = existing;
        }

        return currentArea;
    }
}

[DebuggerDisplay("{Name} = {Value}")]
record Constant(string Name, string? Value, string? Comment, string Type = "string")
{
    public string? EscapedValue => Value == null ? null : new XText(Value).ToString();
    public bool IsText => Type.Equals("string", StringComparison.OrdinalIgnoreCase);
}
