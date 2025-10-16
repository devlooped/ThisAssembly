<!-- include https://github.com/devlooped/.github/raw/main/osmf.md -->
<!-- #project -->
This package generates a static `ThisAssembly.Project` class with public 
constants exposing project properties that have been opted into this mechanism by adding 
them as `ProjectProperty` MSBuild items in the project file, such as:

```xml
  <PropertyGroup>
    <!-- Some arbitrary MSBuild property declared somewhere -->
    <Foo>Bar</Foo>
  </PropertyGroup>
  <ItemGroup>
    <!-- Opt-in to emitting that property value as a constant in ThisAssembly.Project -->
    <ProjectProperty Include="Foo" Comment="This comment replaces the default comment :)" />
  </ItemGroup>
```

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Project.png)

<!-- #project -->
<!-- include ../visibility.md -->
<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
<!-- exclude -->