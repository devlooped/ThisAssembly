[![EULA](https://img.shields.io/badge/EULA-OSMF-blue?labelColor=black&color=C9FF30)](osmfeula.txt)
[![OSS](https://img.shields.io/github/license/devlooped/oss.svg?color=blue)](license.txt) 
<!-- #vsix -->
Allows consuming VSIX manifest properties from code and MSBuild. 

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Vsix.png)

In addition to making the [VSIX manifest metadata](https://learn.microsoft.com/en-us/visualstudio/extensibility/vsix-extension-schema-2-0-reference?view=vs-2022#metadata-element) 
properties available as constants, the package also provides targets for those properties 
with sensible defaults from project properties so that the manifest can leverage 
[placeolder syntax](https://learn.microsoft.com/en-us/visualstudio/extensibility/vsix-extension-schema-2-0-reference?view=vs-2022#metadata-element) 
and avoid duplication in the `source.extension.vsixmanifest`:

```xml
<PackageManifest Version="2.0.0" ...>
  <Metadata>
    <!-- You can use the |ProjectName;TargetName| syntax throughout this manifest, BTW -->
    <Identity Id="|%CurrentProject%;VsixId|" Version="|%CurrentProject%;VsixVersion|" Language="|%CurrentProject%;VsixLanguage|" Publisher="|%CurrentProject%;VsixPublisher|" />
    <DisplayName>|%CurrentProject%;VsixDisplayName|</DisplayName>
    <Description>|%CurrentProject%;VsixDescription|</Description>
  </Metadata>
  ...
</PackageManifest>
```

The available properties and their default values are:

| Name              | Default Value                       |
|-------------------|-------------------------------------|
| VsixID            | `$(PackageId)` or `$(AssemblyName)` |
| VsixVersion       | `$(Version)`                        |
| VsixDisplayName   | `$(Title)`                          |
| VsixDescription   | `$(Description)`                    |
| VsixProduct       | `$(Product)`                        |
| VsixPublisher     | `$(Company)`                        |
| VsixLanguage      | `$(NeutralLanguage)` or 'en-US'     |

As shown in the example above, the syntax for using these properties from the `source.extension.vsixmanifest` is 
`|%CurrentProject%;[PROPERTY]|`. This is because the package defines a corresponding target to 
retrieve each of the above properties. You can provide a different value for each property via 
MSBuild as usual, of course.

Since the `$(PackageId)` property can be used as the VSIX ID, the `Pack` target is redefined to 
mean `CreateVsixManifest`, so "packing" the VSIX is just a matter of right-clicking the VSIX 
project and selecting "Pack".

<!-- #vsix -->
<!-- include ../visibility.md -->
<!-- include https://github.com/devlooped/.github/raw/main/osmf.md -->
<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
<!-- exclude -->