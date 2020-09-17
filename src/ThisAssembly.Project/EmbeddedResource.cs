using System;
using System.IO;
using System.Reflection;

namespace ThisAssembly
{
    static class EmbeddedResource
    {
        public static string GetContent(string relativePath)
        {
            var baseName = Assembly.GetExecutingAssembly().GetName().Name;
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(baseName + "." +
                relativePath.TrimStart('.').Replace(Path.DirectorySeparatorChar, '.').Replace(Path.AltDirectorySeparatorChar, '.'));

            if (stream == null)
                throw new NotSupportedException();

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
