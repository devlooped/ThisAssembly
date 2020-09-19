using System;
using System.IO;
using System.Reflection;

namespace ThisAssembly
{
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

            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(baseName + "." + resourceName);

            if (stream == null)
                throw new NotSupportedException();

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
