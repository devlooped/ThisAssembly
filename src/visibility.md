## Customizing the generated code

Set the `$(ThisAssemblyNamespace)` MSBuild property to set the namespace of the 
generated `ThisAssembly` root class. Otherwise, it will be generated in the global namespace.

The generated root `ThisAssembly` class is partial and has no visibility modifier by default, 
making it internal by default in C#.

You can set the `$(ThisAssemblyVisibility)` MSBuild property to `public` to make it public. 
This will also change all constants to be static readonly properties instead. 

Default:
```csharp
partial class ThisAssembly
{
    public partial class Constants
    {
        public const string Hello = "World";
    }
}
```

In this case, the compiler will inline the constants directly into the consuming code at 
the call site, which is optimal for performance for the common usage of constants.

Public:
```csharp
public partial class ThisAssembly
{
    public partial class Constants
    {
        public static string Hello => "World";
    }
}
```

This makes it possible for consuming code to remain unchanged and not require 
a recompile when the the values of `ThisAssembly` are changed in a referenced assembly.

If you want to keep the properties as constants, you can instead extend the generated 
code by defining a another partial that can modify its visibility as needed (or add 
new members). 

```csharp
// makes the generated class public
public partial ThisAssembly 
{
    // Nested classes are always public since the outer class 
    // already limits their visibility
    partial class Constants 
    {
        // add some custom constants
        public const string MyConstant = "This isn't configurable via MSBuild";

        // generated code will remain as constants
    }
}
```
