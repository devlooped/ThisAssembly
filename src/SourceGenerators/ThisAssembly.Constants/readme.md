**C# 9.0 ONLY**

This package generates a static `ThisAssembly.Constants` class with public constants for each `Constant` MSBuild item in the project.

For example, the MSBuild snippet below

```XML
  <ItemGroup>
    <Constant Include="Foo.Bar" Value="Baz" />
  </ItemGroup>
```

results in a corresponding `ThisAssembly.Constants.Foo.Bar` constant with the value `"Baz"`.

The corresponding generated code is as follows:

```C#
partial class ThisAssembly
{
    public static partial class Constants
    {
        public static partial class Foo
        {
            public const string Bar = "Baz";
        }
    }
}
```
