**C# 9.0 ONLY**

This package generates a static `ThisAssembly.Metadata` class with public constants exposing each [`AssemblyMetadata` attribute](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assemblymetadataattribute) either defined in code, or via [`AssemblyMetadata` MSBuild items](https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items#assemblymetadata) (if using .NET SDK 5.0 or a later version).

So for an attribute like:

```C#
[assembly:System.Reflection.AssemblyMetadata("Foo", "Bar")]
```

a corresponding `ThisAssembly.Metadata.Foo` constant with the value `Bar` is provided.

The same result can alternatively be achieved, if using .NET SDK 5.0 or a later version, by adding the following lines to the project file:

```xml
    <ItemGroup>
      <AssemblyMetadata Include="Foo" Value="Bar" />
    </ItemGroup>
```

The generated code will be the same in both cases:

```C#
partial class ThisAssembly
{
    public static partial class Metadata
    {
        public const string Foo = "Bar";
    }
}
```

---

**NOTE:** No constant will be generated for `AssemblyMetadata` attributes added by other source generators, as source generators, by design, are unable to see one another's outputs.
