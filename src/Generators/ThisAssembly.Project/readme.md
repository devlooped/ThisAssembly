**C# 9.0 ONLY**

This package generates a static `ThisAssembly.Project` class with public constants exposing project properties that have been opted into this mechanism by adding them as `ProjectProperty` MSBuild items.

For example, given the following code in a `.csproj` file:

```xml
  <PropertyGroup>
    <Foo>Bar</Foo>
  </PropertyGroup>

  <ItemGroup>
    <ProjectProperty Include="Foo" />
  </ItemGroup>
```

a corresponding `ThisAssembly.Project.Foo` constant with the value `Bar` is provided. 

The generated code is as follows:

```C#
partial class ThisAssembly
{
    public static partial class Project
    {
        public const string Foo = "Bar";
    }
}
```
