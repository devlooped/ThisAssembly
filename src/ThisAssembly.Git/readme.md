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

<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
# Sponsors 

<!-- sponsors.md -->
[![Clarius Org](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/clarius.png "Clarius Org")](https://github.com/clarius)
[![C. Augusto Proiete](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/augustoproiete.png "C. Augusto Proiete")](https://github.com/augustoproiete)
[![Kirill Osenkov](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/KirillOsenkov.png "Kirill Osenkov")](https://github.com/KirillOsenkov)
[![MFB Technologies, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/MFB-Technologies-Inc.png "MFB Technologies, Inc.")](https://github.com/MFB-Technologies-Inc)
[![SandRock](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/sandrock.png "SandRock")](https://github.com/sandrock)
[![Andy Gocke](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/agocke.png "Andy Gocke")](https://github.com/agocke)
[![Stephen Shaw](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/decriptor.png "Stephen Shaw")](https://github.com/decriptor)
[![Torutek](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/torutek-gh.png "Torutek")](https://github.com/torutek-gh)


<!-- sponsors.md -->

[![Sponsor this project](https://raw.githubusercontent.com/devlooped/sponsors/main/sponsor.png "Sponsor this project")](https://github.com/sponsors/devlooped)
&nbsp;

[Learn more about GitHub Sponsors](https://github.com/sponsors)

<!-- https://github.com/devlooped/sponsors/raw/main/footer.md -->
