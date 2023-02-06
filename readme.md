![Icon](img/icon-32.png) ThisAssembly
============

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.svg?color=green)](https://www.nuget.org/packages/ThisAssembly)
[![License](https://img.shields.io/github/license/devlooped/ThisAssembly.svg?color=blue)](https://github.com//devlooped/ThisAssembly/blob/main/LICENSE)
[![Build](https://github.com/devlooped/ThisAssembly/workflows/build/badge.svg?branch=main)](https://github.com/devlooped/ThisAssembly/actions)


Exposes project and assembly level information as constants in the ThisAssembly 
class using source generators powered by Roslyn.

The main generated entry point type is `ThisAssembly` in the global namespace, 
and is declared as partial so you can extend it too with manually created members.

Each package in turn extends this partial class to add their own nestes types 
and members.

The [ThisAssembly](https://nuget.org/packages/ThisAssembly) meta-package includes 
all the other packages for convenience.

> NOTE: for now, ThisAssembly only generates C# code.

## ThisAssembly.AssemblyInfo

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.AssemblyInfo.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.AssemblyInfo)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.AssemblyInfo.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.AssemblyInfo)

<!-- include src/ThisAssembly.AssemblyInfo/readme.md#content -->
<!-- #content -->
This package generates a static `ThisAssembly.Info` class with public 
constants exposing the following attribute values generated by default for SDK style projects:

* AssemblyConfigurationAttribute
* AssemblyCompanyAttribute
* AssemblyTitleAttribute
* AssemblyDescriptionAttribute
* AssemblyProductAttribute
* AssemblyCopyrightAttribute

* AssemblyVersionAttribute
* AssemblyInformationalVersionAttribute
* AssemblyFileVersionAttribute

If your project includes these attributes by other means, they will still be emitted properly 
on the `ThisAssembly.Info` class.

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.AssemblyInfo.png)

<!-- #content -->
<!-- src/ThisAssembly.AssemblyInfo/readme.md#content -->

## ThisAssembly.Constants

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Constants.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Constants)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Constants.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Constants)

<!-- include src/ThisAssembly.Constants/readme.md#content -->
<!-- #content -->
This package generates a static `ThisAssembly.Constants` class with public
constants for `@(Constant)` MSBuild items in the project.

```xml
  <ItemGroup>
    <Constant Include="Foo.Bar" Value="Baz" Comment="Yay!" />
    <Constant Include="Foo.Hello" Value="World" Comment="Comments make everything better 😍" />
  </ItemGroup>
```


![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Constants.png)

In addition to arbitrary constants via `<Constant ...>`, it's quite useful (in particular in test projects) 
to generate constants for files in the project, so there's also a shorthand for those:

```xml
  <ItemGroup>
    <FileConstant Include="@(Content)" />
  </ItemGroup>
```

Which results in:

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Constants2.png)

<!-- #content -->
<!-- src/ThisAssembly.Constants/readme.md#content -->

## ThisAssembly.Git

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Git.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Git)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Git.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Git)

<!-- include src/ThisAssembly.Git/readme.md#content -->
<!-- #content -->
This package generates a static `ThisAssembly.Git` class with constants 
for the following Git properties from the current project:

* Commit
* Sha (first 9 chars from Commit)
* Root (normalized to forward slashes)
* Url (if PublishRepositoryUrl=true)
* Branch (from CI environment variables)

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Git.png)

This package relies on your project's installed
[Microsoft.SourceLink.*](https://www.nuget.org/packages?q=Microsoft.SourceLink) 
package reference according to your specific Git-based source control server 
(such as GitHub, Azure DevOps, BitBucket, etc).

The `Branch` property is populated from environment variables provided 
by the currently supported CI systems: GitHub Actions, Azure DevOps, 
AppVeyor, TeamCity, Travis CI, Circle CI, GitLab CI, Buddy, and Jenkins.

Whenever the CI system provides a pull request number, the branch name is 
`pr[NUMBER]`, such as `pr123`. This makes it easy to use it as a semver 
metadata label.

> Note: by default, the values of these constants are populated during 
"real" builds (that is, not IDE/design-time builds used to populate 
intellisense). This is to avoid negatively affecting the editor's 
performance. This means, however, that the properties will seem to 
always be empty when inspecting them in the IDE (although never at 
run-time). If you want to force population of these values for 
design-time builds, set the `EnableSourceControlManagerQueries` property to `true`. 
This property is defined and documented by 
[dotnet/sourcelink](https://github.com/dotnet/sourcelink/blob/main/src/SourceLink.Common/build/Microsoft.SourceLink.Common.props#L14).

At the MSBuild level, targets can take a dependency on the provided 
`InitializeGitInformation` target, which sets the equivalent properties
named:

* RepositoryCommit
* RepositorySha
* RepositoryRoot
* RepositoryUrl
* RepositoryBranch

The names of these properties were chosen on purpose to match the 
properties used by [nuget pack](https://learn.microsoft.com/en-us/nuget/reference/msbuild-targets#pack-target) 
and [nugetizer](https://github.com/devlooped/nugetizer) to populate
the relevant package metadata. 

So if you have a GitHub repository, installing these three packages 
will ensure you have the proper metadata out of the box and the simplest 
packaging experience possible:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.SourceLink.GitHub" />
    <PackageReference Include="ThisAssembly.Git" />
    <PackageReference Include="NuGetizer" />
  </ItemGroup>
</Project>
```


<!-- #content -->
<!-- src/ThisAssembly.Git/readme.md#content -->

## ThisAssembly.Metadata

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Metadata.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Metadata)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Metadata.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Metadata)

<!-- include src/ThisAssembly.Metadata/readme.md#content -->
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
<!-- src/ThisAssembly.Metadata/readme.md#content -->

## ThisAssembly.Project

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Project.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Project)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Project.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Project)

<!-- include src/ThisAssembly.Project/readme.md#content -->
<!-- #content -->
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
    <ProjectProperty Include="Foo" />
  </ItemGroup>
```

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Project.png)

<!-- #content -->
<!-- src/ThisAssembly.Project/readme.md#content -->

## ThisAssembly.Resources

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Resources.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Resources)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Resources.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Resources)

This package generates a static `ThisAssembly.Resources` class with public 
properties exposing shortcuts to retrieve the contents of embedded resources.

<!-- include src/ThisAssembly.Resources/readme.md#content -->
<!-- #content -->

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

<!-- #content -->
<!-- src/ThisAssembly.Resources/readme.md#content -->

## ThisAssembly.Strings

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Strings.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Strings)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Strings.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Strings)

<!-- include src/ThisAssembly.Strings/readme.md#content -->
<!-- #content -->

This package generates a static `ThisAssembly.Strings` class with public 
constants exposing string resources in .resx files or methods with the right number of 
parameters for strings that use formatting parameters. 

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Strings.gif)

In addition, it groups constants and methods in nested classes according to an optional 
underscore separator to organize strings. For example, *User_InvalidCredentials* can be
accessed with *ThisAssembly.Strings.User.InvalidCredentials* if it contains a simple string, 
or as a method with the right number of parametres if its value has a format string.

Given the following Resx file:

| Name                          | Value                                 | Comment           |
|-------------------------------|---------------------------------------|-------------------|
| Infrastructure_MissingService | Service {0} is required.              | For logging only! |
| Shopping_NoShipping           | We cannot ship {0} to {1}.            |                   |
| Shopping_OutOfStock           | Product is out of stock at this time. |                   |

The following code would be generated:

```csharp
partial class ThisAssembly
{
    public static partial class Strings
    {
        public static partial class Infrastructure
        {
            /// <summary>
            /// For logging only!
            /// => "Service {0} is required."
            /// </summary>
            public static string MissingService(object arg0)
                => string.Format(CultureInfo.CurrentCulture, 
                    Strings.GetResourceManager("ThisStore.Properties.Resources").GetString("MissingService"), 
                    arg0);
        }

        public static partial class Shopping
        {
            /// <summary>
            /// => "We cannot ship {0} to {1}."
            /// </summary>
            public static string NoShipping(object arg0, object arg1)
                => string.Format(CultureInfo.CurrentCulture, 
                    Strings.GetResourceManager("ThisStore.Properties.Resources").GetString("NoShipping"), 
                    arg0, arg1);

            /// <summary>
            /// => "Product is out of stock at this time."
            /// </summary>
            public static string OutOfStock
                => Strings.GetResourceManager("ThisStore.Properties.Resources").GetString("OutOfStock");
        }
    }
}
```

<!-- #content -->
<!-- src/ThisAssembly.Strings/readme.md#content -->

# Dogfooding

[![CI Version](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/Stunts/main&label=nuget.ci&color=brightgreen)](https://pkg.kzu.io/index.json)
[![Build](https://github.com/devlooped/ThisAssembly/workflows/build/badge.svg?branch=main)](https://github.com/devlooped/ThisAssembly/actions)

We also produce CI packages from branches and pull requests so you can dogfood builds as quickly as they are produced. 

The CI feed is `https://pkg.kzu.io/index.json`. 

The versioning scheme for packages is:

- PR builds: *42.42.42-pr*`[NUMBER]`
- Branch builds: *42.42.42-*`[BRANCH]`.`[COMMITS]`


<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
# Sponsors 

<!-- sponsors.md -->
[![Clarius Org](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/clarius.png "Clarius Org")](https://github.com/clarius)
[![Christian Findlay](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/MelbourneDeveloper.png "Christian Findlay")](https://github.com/MelbourneDeveloper)
[![C. Augusto Proiete](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/augustoproiete.png "C. Augusto Proiete")](https://github.com/augustoproiete)
[![Kirill Osenkov](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/KirillOsenkov.png "Kirill Osenkov")](https://github.com/KirillOsenkov)
[![MFB Technologies, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/MFB-Technologies-Inc.png "MFB Technologies, Inc.")](https://github.com/MFB-Technologies-Inc)
[![SandRock](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/sandrock.png "SandRock")](https://github.com/sandrock)
[![Eric C](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/eeseewy.png "Eric C")](https://github.com/eeseewy)
[![Andy Gocke](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/agocke.png "Andy Gocke")](https://github.com/agocke)


<!-- sponsors.md -->

[![Sponsor this project](https://raw.githubusercontent.com/devlooped/sponsors/main/sponsor.png "Sponsor this project")](https://github.com/sponsors/devlooped)
&nbsp;

[Learn more about GitHub Sponsors](https://github.com/sponsors)

<!-- https://github.com/devlooped/sponsors/raw/main/footer.md -->
