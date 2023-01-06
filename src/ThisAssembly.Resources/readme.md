
This package generates a static `ThisAssembly.Resources` class with public 
properties exposing typed APIs to retrieve the contents of embedded resources.


```xml
  <ItemGroup>
    <EmbeddedResource Include="Content/Docs/License.md" />
  </ItemGroup>
```

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Resources.png)

Since markdown files are text files, the API will expose a `Text` property property 
for it that will read its content once and cache it:

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Resources2.png)

The `$(EmbeddedResourceStringExtensions)` MSBuild property allows customizing which 
file extensions get treated as text files. By default, it's defined as:

```xml
  <PropertyGroup>
    <EmbeddedResourceStringExtensions>.txt|.cs|.sql|.json|.md;</EmbeddedResourceStringExtensions>
  </PropertyGroup>
```

You can always use the provided `GetStream` and `GetBytes` for more advanced scenarios (or for 
non-text resources).

<!-- #content -->
<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->