using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

[DebuggerDisplay("ResourceName = {ResourceName}, Values = {RootArea.Values.Count}")]
record Model(ResourceArea RootArea, string ResourceName)
{
    public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
}

static class ResourceFile
{
    static readonly Regex FormatExpression = new Regex("{(?<name>[^{}]+)}", RegexOptions.Compiled);

    public static ResourceArea Load(string fileName, string rootArea)
    {
        return Load(
            XDocument.Load(fileName)
                .Root.Elements("data")
                .Where(e => e.Attribute("type") == null),
            rootArea);
    }

    public static ResourceArea Load(IEnumerable<XElement> data, string rootArea)
    {
        var root = new ResourceArea(rootArea, "");

        foreach (var element in data)
        {
            //  Splits: ([resouce area]_)*[resouce name]
            var nameAttribute = element.Attribute("name").Value;
            var valueElement = element.Element("value").Value;
            var comment = element.Element("comment")?.Value?.Replace("<", "&lt;").Replace(">", "&gt;");
            var areaParts = nameAttribute.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            if (areaParts.Length <= 1)
            {
                root.Values.Add(GetValue(nameAttribute, valueElement) with { Comment = comment });
            }
            else
            {
                var area = GetArea(root, areaParts.Take(areaParts.Length - 1));
                var value = GetValue(areaParts.Skip(areaParts.Length - 1).First(), valueElement) with { Comment = comment };

                area.Values.Add(value);
            }
        }

        SortArea(root);
        return root;
    }

    static void SortArea(ResourceArea area)
    {
        area.Values.Sort((left, right) => left.Name.CompareTo(right.Name));
        foreach (var nested in area.NestedAreas)
            SortArea(nested);
    }

    static ResourceArea GetArea(ResourceArea area, IEnumerable<string> areaPath)
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

                existing = new ResourceArea(areaName, currentArea.Prefix + areaName + "_");
                currentArea.NestedAreas.Add(existing);
            }

            currentArea = existing;
        }

        return currentArea;
    }

    static ResourceValue GetValue(string resourceName, string resourceValue)
    {
        var value = new ResourceValue(resourceName, resourceValue);

        // Parse parameter names
        if (FormatExpression.IsMatch(resourceValue))
        {
            value.Format.AddRange(FormatExpression
                    .Matches(resourceValue)
                    .OfType<Match>()
                    .Select(match => match.Groups["name"].Value)
                    .Distinct());
        }

        return value;
    }
}

[DebuggerDisplay("Name = {Name}, NestedAreas = {NestedAreas.Count}, Values = {Values.Count}")]
record ResourceArea(string Name, string Prefix)
{
    public List<ResourceArea> NestedAreas { get; init; } = new List<ResourceArea>();
    public List<ResourceValue> Values { get; init; } = new List<ResourceValue>();
}

[DebuggerDisplay("{Name} = {Value}")]
record ResourceValue(string Name, string? Raw)
{
    public string? Value => Raw?.Replace(Environment.NewLine, "")?.Replace("<", "&lt;")?.Replace(">", "&gt;");
    public string? Comment { get; init; }
    public bool HasFormat => Format != null && Format.Count > 0;
    // We either have *all* named or all indexed. Can't mix. We'll skip generating 
    // methods for mixed ones and report as an analyzer error on the Resx.
    public bool IsNamedFormat => HasFormat && Format.All(x => !int.TryParse(x, out _));
    public bool IsIndexedFormat => HasFormat && Format.All(x => int.TryParse(x, out _));
    public List<string> Format { get; } = new List<string>();
}