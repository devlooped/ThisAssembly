![Icon](img/icon-32.png) ThisAssembly
============

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.svg?color=green)](https://www.nuget.org/packages/ThisAssembly)
[![License](https://img.shields.io/github/license/kzu/ThisAssembly.svg?color=blue)](https://github.com//kzu/ThisAssembly/blob/main/LICENSE)
[![Build](https://github.com/kzu/ThisAssembly/workflows/build/badge.svg?branch=main)](https://github.com/kzu/ThisAssembly/actions)


Exposes project and assembly level information as constants in the ThisAssembly 
class using source generators powered by Roslyn.

The main generated entry point type is `ThisAssembly` in the global namespace, 
and is declared as partial so you can extend it too with manually created members.

Each package in turn extends this partial class too to add their own constants.

The [ThisAssembly](https://nuget.org/packages/ThisAssembly) meta-package includes 
all the other packages for convenience.

> NOTE: as of .NET 5.0 RC1, only C# is supported for source generators
> so even if the generators here can emit proper VB and F#, no code will be generated 
> (yet) for those languages. When support is introduced for those languages,
> things will Just Work without the need to update these packages.

> NOTE: if intellisense isn't working properly, try closing and reopening 
> Visual Studio. If that doesn't work, please set the *$(IncludeSourceGeneratorIntellisenseFix)* 
> property to *true* in your project. This is a temporary workaround for a 
> [Roslyn issue](https://github.com/dotnet/roslyn/issues/44093) that should be 
> solved by the final release of .NET5.

## ThisAssembly.AssemblyInfo

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.AssemblyInfo.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.AssemblyInfo)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.AssemblyInfo.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.AssemblyInfo)

This package generates a static `ThisAssembly.Info` class with public 
constants exposing the following attribute values generated by default for SDK style projects:

* AssemblyConfigurationAttribute
* AssemblyCompanyAttribute
* AssemblyTitleAttribute
* AssemblyProductAttribute

* AssemblyVersionAttribute
* AssemblyInformationalVersionAttribute
* AssemblyFileVersionAttribute

If your project includes these attributes by other means, they will still be emitted properly 
on the `ThisAssembly.Info` class.

![](img/ThisAssembly.AssemblyInfo.png)

## ThisAssembly.Contracts

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Contracts.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Contracts)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Contracts.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Contracts)

This package generates a static `ThisAssembly.Constants` class with public
constants for `@(Constant)` MSBuild items in the project.

For example:

```xml
  <ItemGroup>
    <Constant Include="Foo.Bar" Value="Baz" Comment="Yay!" />
  <ItemGroup>
```

Results in a corresponding `ThisAssembly.Constants.Foo.Bar` constant with the value `Baz`:

![](img/ThisAssembly.Constants.png)

In addition to arbitrary constants, it's quite useful in particular in test projects, to generate constants for files in the project, so there's also a shorthand for those:

```xml
  <ItemGroup>
    <FileConstant Include="@(Content)" />
  <ItemGroup>
```

Which results in:

![](img/ThisAssembly.Constants2.png)


## ThisAssembly.Metadata

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Metadata.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Metadata)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Metadata.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Metadata)

This package provides a static `ThisAssembly.Metadata` class with public 
constants exposing each `[System.Reflection.AssemblyMetadata(..)]` defined for 
the project.

![](img/ThisAssembly.Metadata.png)

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

Generated code:

C#:

```csharp
  partial class ThisAssembly
  {
      public static partial class Metadata
      {
          public const string Foo = "Bar";
      }
  }
```

VB:
```vbnet
  Namespace Global
    Partial Class ThisAssembly
          Partial Class Metadata
              Public Const Foo = "Bar"
          End Class
      End Class
  End Namespace
```

F#:
```fsharp
  module internal ThisAssembly

  module public Metadata =
      [<Literal>]
      let public Foo = @"Bar"
```

## ThisAssembly.Project

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Project.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Project)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Project.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Project)

This package generates a static `ThisAssembly.Project` class with public 
constants exposing project properties that have been opted into this mechanism by adding 
them as `ThisAssemblyProject` MSBuild items in project file, such as:

```xml
  <PropertyGroup>
    <Foo>Bar</Foo>
  </PropertyGroup>
  <ItemGroup>
    <ThisAssemblyProject Include="Foo" />
  </ItemGroup>
```

![](img/ThisAssembly.Project.png)

Generated code:

C#:

```csharp
  partial class ThisAssembly
  {
      public static partial class Project
      {
          public const string Foo = "Bar";
      }
  }
```

VB:
```vbnet
  Namespace Global
    Partial Class ThisAssembly
          Partial Class Project
              Public Const Foo = "Bar"
          End Class
      End Class
  End Namespace
```

F#:
```fsharp
  module internal ThisAssembly

  module public Project =
      [<Literal>]
      let public Foo = @"Bar"
```

## ThisAssembly.Strings

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Strings.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Strings)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Strings.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Strings)

![](img/ThisAssembly.Strings.gif)

This package generates a static `ThisAssembly.Strings` class with public 
constants exposing string resources in .resx files or methods with the right number of 
parameters for strings that use formatting parameters. 

In addition, it groups constants and methods in nested classes according to an optional 
underscore separator to organize strings. For example, *User_InvalidCredentials* can be
accessed with *ThisAssembly.Strings.User.InvalidCredentials* if it contains a simple string, 
or as a method with the right number of parametres if its value has a format string.

Given the following Resx file:

| Name                          | Value                                 | Comment           |   |   |
|-------------------------------|---------------------------------------|-------------------|---|---|
| Infrastructure_MissingService | Service {0} is required.              | For logging only! |   |   |
| Shopping_NoShipping           | We cannot ship {0} to {1}.            |                   |   |   |
| Shopping_OutOfStock           | Product is out of stock at this time. |                   |   |   |

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

# Dogfooding

[![CI Version](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/Stunts/main&label=nuget.ci&color=brightgreen)](https://pkg.kzu.io/index.json)
[![GH CI Status](https://github.com/kzu/stunts/workflows/build/badge.svg?branch=main)](https://github.com/kzu/stunts/actions?query=branch%3Amain+workflow%3Abuild+)
[![AzDO CI Status](https://dev.azure.com/kzu/oss/_apis/build/status/stunts?branchName=main)](http://build.azdo.io/kzu/oss/45)

We also produce CI packages from branches and pull requests so you can dogfood builds as quickly as they are produced. 

The CI feed is `https://pkg.kzu.io/index.json`. 

The versioning scheme for packages is:

- PR builds: *42.42.42-pr*`[NUMBER]`.`[COMMITS]`
- Branch builds: *42.42.42-*`[BRANCH]`.`[COMMITS]`


