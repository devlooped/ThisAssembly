# ThisAssembly

Exposes project and assembly level information as constants in the ThisAssembly 
class using .NET 5.0+ source generators powered by Roslyn.

The main generated entry point type is `ThisAssembly` in the global namespace, 
and is declared as partial so you can extend it too with manually created members.

Each package in turn extends this partial class too to add their own constants.

The [ThisAssembly](https://nuget.org/packages/ThisAssembly] meta-package includes 
all the other packages for convenience.


## ThisAssembly.Metadata

This package provides a static `ThisAssembly.Metadata` class with public 
constants exposing each `[System.Reflection.AssemblyMetadata(..)]` defined for 
the project.

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