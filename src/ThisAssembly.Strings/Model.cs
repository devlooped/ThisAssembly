using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

[DebuggerDisplay("ResourceName = {ResourceName}, Values = {RootArea.Values.Count}")]
record Model(ResourceArea RootArea, string ResourceName, string? Namespace, bool IsPublic)
{
    public string? Version => Assembly.GetExecutingAssembly().GetName().Version?.ToString(3);
    public string Visibility => IsPublic ? "public " : "";
}

static class ResourceFile
{
    static readonly Regex FormatExpression = new("{(?<arg>[^:{}]+)(?::(?<format>[^{}]+))?}", RegexOptions.Compiled);
    internal static readonly Regex NameReplaceExpression = new(@"\||:|;|\>|\<", RegexOptions.Compiled);

    /// <summary>
    /// Resource name used as an underscore-separated base for another name (issue #493).
    /// </summary>
    public static readonly DiagnosticDescriptor BaseNameCollision = new(
        "TA003",
        "Cannot use a resource name as the base name for another",
        "You cannot use a resource name as the base name for another. Rename '{0}' to '{0}_Title' to be able to use '{1}'.",
        "ThisAssembly",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static ResourceArea Load(string fileName, string rootArea)
    {
        return Load(
            XDocument.Load(fileName)
                .Root!.Elements("data")
                .Where(e => e.Attribute("type") == null),
            rootArea);
    }

    public static ResourceArea LoadText(string resourceText, string rootArea)
    {
        return Load(
            XDocument.Parse(resourceText)
                .Root!.Elements("data")
                .Where(e => e.Attribute("type") == null),
            rootArea);
    }

    public static IReadOnlyList<Diagnostic> GetDiagnostics(ResourceArea root) =>
        [.. root.Collisions.Select(c => Diagnostic.Create(BaseNameCollision, Location.None, c.Name, c.NestedName))];

    public static void ReportDiagnostics(ResourceArea root, Action<Diagnostic> report)
    {
        foreach (var diagnostic in GetDiagnostics(root))
            report(diagnostic);
    }

    public static ResourceArea Load(IEnumerable<XElement> data, string rootArea)
    {
        var root = new ResourceArea(rootArea, "");
        var entries = new List<(string Name, string Id, string Value, string Comment)>();

        foreach (var element in data)
        {
            //  Splits: ([resouce area]_)*[resouce name]
            var nameAttribute = element.Attribute("name")?.Value;
            if (nameAttribute == null)
                continue;

            var id = NameReplaceExpression.Replace(nameAttribute, "_");
            var valueElement = element.Element("value")?.Value;
            if (valueElement == null)
                continue;

            // Make sure we trim newlines and replace them with spaces for comments.
            var comment = (element.Element("comment")?.Value ?? valueElement)
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\r\n", " ").Replace("\n", " ");

            entries.Add((nameAttribute, id, valueElement, comment));
        }

        foreach (var entry in entries)
        {
            foreach (var other in entries)
            {
                if (other.Id.StartsWith(entry.Id + "_", StringComparison.Ordinal))
                    root.Collisions.Add(new ResourceNameCollision(entry.Name, other.Name));
            }
        }

        var baseIds = new HashSet<string>(
            root.Collisions.Select(c => NameReplaceExpression.Replace(c.Name, "_")),
            StringComparer.Ordinal);

        foreach (var (nameAttribute, id, valueElement, comment) in entries)
        {
            // Skip names that are also an underscore base for another resource so
            // generated code does not emit both a member and a nested class (CS0102).
            if (baseIds.Contains(id))
                continue;

            var areaParts = id.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            if (areaParts.Length <= 1)
            {
                if (root.NestedAreas.Any(a => a.Id == id))
                    continue;

                root.Values.Add(GetValue(id, nameAttribute, valueElement) with { Comment = comment });
            }
            else
            {
                var area = GetArea(root, areaParts.Take(areaParts.Length - 1));
                var value = GetValue(areaParts.Skip(areaParts.Length - 1).First(), nameAttribute, valueElement) with { Comment = comment };
                if (area.NestedAreas.Any(a => a.Id == value.Id))
                    continue;

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
            var existing = currentArea.NestedAreas.FirstOrDefault(a => a.Id == areaName);
            if (existing == null)
            {
                // Drop a colliding value so we never emit both a member and a nested class.
                currentArea.Values.RemoveAll(v => v.Id == areaName);

                existing = new ResourceArea(areaName, currentArea.Prefix + areaName + "_");
                currentArea.NestedAreas.Add(existing);
            }

            currentArea = existing;
        }

        return currentArea;
    }

    static ResourceValue GetValue(string resourceId, string resourceName, string resourceValue)
    {
        var value = new ResourceValue(resourceId, resourceName, resourceValue);

        // Parse parameter names
        if (FormatExpression.IsMatch(resourceValue))
        {
            value.Format.AddRange(FormatExpression
                    .Matches(resourceValue)
                    .OfType<Match>()
                    .Select(match =>
                    {
                        var arg = match.Groups["arg"].Value;
                        var format = match.Groups["format"].Value;
                        return new ArgFormat(match.Value, arg, string.IsNullOrWhiteSpace(format) ? null : format);
                    })
                    .Distinct());
        }

        return value;
    }
}

[DebuggerDisplay("Id = {Id}, NestedAreas = {NestedAreas.Count}, Values = {Values.Count}")]
record ResourceArea(string Id, string Prefix)
{
    public List<ResourceArea> NestedAreas { get; init; } = [];
    public List<ResourceValue> Values { get; init; } = [];
    public List<ResourceNameCollision> Collisions { get; init; } = [];
}

record ResourceNameCollision(string Name, string NestedName);

[DebuggerDisplay("{Id} = {Value}")]
record ResourceValue(string Id, string Name, string? Raw)
{
    public string? Value => Raw?.Replace(Environment.NewLine, "")?.Replace("<", "&lt;")?.Replace(">", "&gt;");
    public string? Comment { get; init; }
    public bool HasFormat => Format.Count > 0;
    public bool HasArgFormat => Format.Any(x => x.Format != null);
    // We either have *all* named or all indexed. Can't mix. We'll skip generating 
    // methods for mixed ones and report as an analyzer error on the Resx.
    public bool IsNamedFormat => HasFormat && Format.All(x => !int.TryParse(x.Arg, out _));
    public bool IsIndexedFormat => HasFormat && Format.All(x => int.TryParse(x.Arg, out _));
    public List<ArgFormat> Format { get; } = [];
    public HashSet<string> Args => new(Format.Select(x => x.Arg));
}

record ArgFormat(string Value, string Arg, string? Format);