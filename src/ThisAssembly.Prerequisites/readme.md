**This package is not meant to be referenced directly by third-party projects.**

Use one or more of the other `ThisAssembly.*` packages, or reference `ThisAssembly` as a shortcut to include all of them.

---

This package ensures that referencing projects satisfy the prerequisites for `ThisAssembly`, namely:

- **MSBuild 16.8+**: contains the Roslyn support for source generators
- **C# 9.0+**: it's the only language supported by Roslyn source generators at the moment.
