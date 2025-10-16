<!-- include https://github.com/devlooped/.github/raw/main/osmf.md -->
## Overview
<!-- #resources -->

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
    <EmbeddedResourceStringExtensions>.txt|.cs|.sql|.json|.md</EmbeddedResourceStringExtensions>
  </PropertyGroup>
```

You can append additional file extensions to this list, or override it completely.
The list must be pipe-separated.

You can always use the provided `GetStream` and `GetBytes` for more advanced scenarios (or for 
non-text resources).

Optionally, you can specify the `Kind` metadata for a specific `EmbeddedResource` you want 
treated as a text file:

```xml
<ItemGroup>
    <EmbeddedResource Include="query.kql" Kind="Text" />
</ItemGroup>
```

You can also add a `Comment` item metadata attribute, which will be used as the `<summary>` XML 
doc for the generated member.

## Adding dynamic resources

You can also provide additional embedded resources dynamically, by running a target before 
`PrepareEmbeddedResources`:

```xml
  <Target Name="AddDynamicResources" BeforeTargets="PrepareEmbeddedResources">
    <ItemGroup>
      <EmbeddedResource Include="Content/Docs/$(Configuration).md" />
    </ItemGroup>
  </Target>
```

## Customizing the generated code

The following MSBuild properties can be used to customize the generated code:

| Property                | Description                                                                                          |
|-------------------------|------------------------------------------------------------------------------------------------------|
| ThisAssemblyNamespace   | Sets the namespace of the generated `ThisAssembly` root class. If not set, it will be in the global namespace. |
| ThisAssemblyVisibility  | Sets the visibility modifier of the generated `ThisAssembly` root class. If not set, it will be internal. |

<!-- #resources -->
<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
<!-- exclude -->