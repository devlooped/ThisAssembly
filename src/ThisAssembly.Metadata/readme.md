<!-- #content -->
This package provides a static `ThisAssembly.Metadata` class with public 
constants exposing each `[System.Reflection.AssemblyMetadata(..)]` defined for 
the project.

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Metadata.png)

For an attribute declared (i.e. in *AssemblyInfo.cs*) like:

```csharp
[assembly: System.Reflection.AssemblyMetadataAttribute("Foo", "Bar")]
```

A corresponding `ThisAssembly.Metadata.Foo` constant with the value `Bar` is provided. 
The metadata attribute can alternatively be declared using MSBuild syntax in the project 
(for .NET 5.0+ projects that have built-in support for `@(AssemblyMetadata)` items):

```xml
  <ItemGroup>
    <AssemblyMetadata Include="Foo" Value="Bar" />
  </ItemGroup>
```

<!-- #content -->
<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->