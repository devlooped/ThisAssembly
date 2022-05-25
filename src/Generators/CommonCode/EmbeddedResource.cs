using System;
using System.IO;
using System.Linq;
using System.Reflection;

static class EmbeddedResource
{
    static readonly string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    public static string GetContent(string relativePath)
    {
        var filePath = Path.Combine(baseDir, Path.GetFileName(relativePath));
        if (File.Exists(filePath))
            return File.ReadAllText(filePath);

        var baseName = Assembly.GetExecutingAssembly().GetName().Name;
        var resourceName = relativePath
            .TrimStart('.')
            .Replace(Path.DirectorySeparatorChar, '.')
            .Replace(Path.AltDirectorySeparatorChar, '.');

        var manifestResourceName = Assembly.GetExecutingAssembly()
            .GetManifestResourceNames().FirstOrDefault(x => x.EndsWith(resourceName));

        if (string.IsNullOrEmpty(manifestResourceName))
            throw new InvalidOperationException($"Did not find required resource ending in '{resourceName}' in assembly '{baseName}'.");

        using var stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(manifestResourceName);

        if (stream == null)
            throw new InvalidOperationException($"Did not find required resource '{manifestResourceName}' in assembly '{baseName}'.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}