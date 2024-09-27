## Customizing the generated code

Set the `$(ThisAssemblyNamespace)` MSBuild property to set the namespace of the 
generated `ThisAssembly` root class. Otherwise, it will be generated in the global namespace.

All generated classes are partial and have no visibility modifier, so they can be extended 
manually with another partial that can add members or modify their visibility to make them 
public, if needed. The C# default for this case is for all classes to be internal.

```csharp
// makes the generated classes public
public partial ThisAssembly 
{
    public partial class Constants { }
}
```
