[![EULA](https://img.shields.io/badge/EULA-OSMF-blue?labelColor=black&color=C9FF30)](osmfeula.txt)
[![OSS](https://img.shields.io/github/license/devlooped/oss.svg?color=blue)](license.txt) 
<!-- #constants -->
This package generates a static `ThisAssembly.Constants` class with public
constants for `@(Constant)` MSBuild items in the project.

```xml
  <ItemGroup>
    <Constant Include="Foo.Bar" Value="Baz" Comment="Yay!" />
    <Constant Include="Foo.Hello" Value="World" Comment="Comments make everything better 😍" />
  </ItemGroup>
```

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Constants.png)

These constants can use values from MSBuild properties, making compile-time values configurable 
via environment variables or command line arguments. For example:

```xml
  <PropertyGroup>
    <HttpDefaultTimeoutSeconds>10</HttpDefaultTimeoutSeconds>
  </PropertyGroup>
  <ItemGroup>
    <Constant Include="Http.TimeoutSeconds" 
              Value="$(HttpDefaultTimeoutSeconds)" 
              Type="int" 
              Comment="Default timeout in seconds for HTTP requests" />
  </ItemGroup>
```

The C# code could consume this constant as follows:

```csharp
public HttpClient CreateHttpClient(string name, int? timeout = default)
{
    HttpClient client = httpClientFactory.CreateClient(name);
    client.Timeout = TimeSpan.FromSeconds(timeout ?? ThisAssembly.Constants.Http.TimeoutSeconds);
    return client;
}
```

Note how the constant is typed to `int` as specified in the `Type` attribute in MSBuild. 
The generated code uses the specified `Type` as-is, as well as the `Value` attribute in that 
case, so it's up to the user to ensure they match and result in valid C# code. For example, 
you can emit a boolean, long, double, etc.. If no type is provided, `string` is assumed. Values 
can also be multi-line and will use [C# raw string literals](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/raw-string) 
if supported by the target language version (11+).

In this example, you could trivially change how your product behaves by setting the environment 
variable `HttpDefaultTimeoutSeconds` in CI. This is particularly useful for test projects, 
where you can easily change the behavior of the system under test without changing the code.


In addition to arbitrary constants via `<Constant ...>`, it's quite useful (in particular in test projects) 
to generate constants for files in the project, so there's also a shorthand for those:

```xml
  <ItemGroup>
    <FileConstant Include="@(Content)" />
  </ItemGroup>
```

Which results in:

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Constants2.png)

<!-- #constants -->
<!-- include ../visibility.md -->
<!-- include https://github.com/devlooped/.github/raw/main/osmf.md -->
<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
<!-- exclude -->