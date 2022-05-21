**C# 9.0 ONLY**

This package generates a static `ThisAssembly.Strings` class with public constants exposing string resources in `.resx` files, or methods with the right number of parameters for strings that use formatting parameters.

In addition, it groups constants and methods in nested classes, using the underscore character as a nesting separator in resource keys. For example, *User_InvalidCredentials* can be accessed as `ThisAssembly.Strings.User.InvalidCredentials` if it contains a simple string, or as a method with the right number of parameters if its value is a format string.

Finally, format strings can use named parameters too to get more friendly parameter names, such as "Hello \{name}".
