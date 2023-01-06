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

## ThisAssembly.Constants

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Constants.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Constants)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Constants.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Constants)

<!-- include src/ThisAssembly.Constants/readme.md#content -->

## ThisAssembly.Metadata

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Metadata.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Metadata)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Metadata.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Metadata)

<!-- include src/ThisAssembly.Metadata/readme.md#content -->

## ThisAssembly.Project

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Project.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Project)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Project.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Project)

<!-- include src/ThisAssembly.Project/readme.md#content -->

## ThisAssembly.Resources

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Resources.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Resources)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Resources.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Resources)

This package generates a static `ThisAssembly.Resources` class with public 
properties exposing shortcuts to retrieve the contents of embedded resources.

<!-- include src/ThisAssembly.Resources/readme.md#content -->

## ThisAssembly.Strings

[![Version](https://img.shields.io/nuget/vpre/ThisAssembly.Strings.svg?color=royalblue)](https://www.nuget.org/packages/ThisAssembly.Strings)
[![Downloads](https://img.shields.io/nuget/dt/ThisAssembly.Strings.svg?color=green)](https://www.nuget.org/packages/ThisAssembly.Strings)

<!-- include src/ThisAssembly.Strings/readme.md#content -->

# Dogfooding

[![CI Version](https://img.shields.io/endpoint?url=https://shields.kzu.io/vpre/Stunts/main&label=nuget.ci&color=brightgreen)](https://pkg.kzu.io/index.json)
[![Build](https://github.com/devlooped/ThisAssembly/workflows/build/badge.svg?branch=main)](https://github.com/devlooped/ThisAssembly/actions)

We also produce CI packages from branches and pull requests so you can dogfood builds as quickly as they are produced. 

The CI feed is `https://pkg.kzu.io/index.json`. 

The versioning scheme for packages is:

- PR builds: *42.42.42-pr*`[NUMBER]`
- Branch builds: *42.42.42-*`[BRANCH]`.`[COMMITS]`


<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->