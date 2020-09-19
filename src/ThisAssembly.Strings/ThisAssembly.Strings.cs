using System.Collections.Concurrent;
using System.Resources;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides access to the current assembly information as pure constants, 
//  without requiring reflection.
/// </summary>
partial class ThisAssembly
{
    /// <summary>
    /// Access the strings provided by resource files in the project.
    /// </summary>
    [CompilerGenerated]
    public static partial class Strings
    {
        static ConcurrentDictionary<string, ResourceManager> resourceManagers = new ConcurrentDictionary<string, ResourceManager>();

        static ResourceManager GetResourceManager(string resourceName)
            => resourceManagers.GetOrAdd(resourceName, name => new ResourceManager(name, typeof(Strings).Assembly));
    }
}