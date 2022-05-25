**C# 9.0 ONLY**

This package generates a static `ThisAssembly.Info` class with public constants exposing the following attribute values generated by default for SDK style projects:

- AssemblyConfigurationAttribute
- AssemblyCompanyAttribute
- AssemblyTitleAttribute
- AssemblyProductAttribute
- AssemblyVersionAttribute
- AssemblyInformationalVersionAttribute
- AssemblyFileVersionAttribute

If your project includes these attributes by other means, they will still be emitted properly on the `ThisAssembly.Info` class.