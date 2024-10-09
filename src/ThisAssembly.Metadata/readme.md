<!-- include https://github.com/devlooped/.github/raw/main/sponsorlinkr.md -->
<!-- #metadata -->
This package provides a static `ThisAssembly.Metadata` class with public 
constants exposing each `[System.Reflection.AssemblyMetadata(..)]` defined in 
the project file as [supported by the .NET SDK](https://learn.microsoft.com/en-us/dotnet/standard/assembly/set-attributes-project-file#set-arbitrary-attributes).

The metadata attribute is declared using MSBuild syntax in the project 
(for .NET 5.0+ projects that have built-in support for `@(AssemblyMetadata)` items):

```xml
  <ItemGroup>
    <AssemblyMetadata Include="Foo" Value="Bar" />
  </ItemGroup>
```

And a corresponding `ThisAssembly.Metadata.Foo` constant with the value `Bar` is provided 
for this example.

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Metadata.png)

<!-- #metadata -->
<!-- include ../visibility.md -->
<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
<!-- exclude -->