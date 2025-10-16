<!-- include https://github.com/devlooped/.github/raw/main/osmf.md -->
<!-- #strings -->

This package generates a static `ThisAssembly.Strings` class with public 
constants exposing string resources in .resx files or methods with the right number of 
parameters for strings that use formatting parameters. 

![](https://raw.githubusercontent.com/devlooped/ThisAssembly/main/img/ThisAssembly.Strings.gif)

In addition, it groups constants and methods in nested classes according to an optional 
underscore separator to organize strings. For example, *User_InvalidCredentials* can be
accessed with *ThisAssembly.Strings.User.InvalidCredentials* if it contains a simple string, 
or as a method with the right number of parametres if its value has a format string.

Given the following Resx file:

| Name                          | Value                                 | Comment           |
|-------------------------------|---------------------------------------|-------------------|
| Infrastructure_MissingService | Service {0} is required.              | For logging only! |
| Shopping_NoShipping           | We cannot ship {0} to {1}.            |                   |
| Shopping_OutOfStock           | Product is out of stock at this time. |                   |
| Shopping_AvailableOn          | Product available on {date:yyyy-MM}.  |                   |

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

            /// <summary>
            /// Product available on {date:yyyy-MM}.
            /// </summary>
            public static string AvailableOn(object date) 
                => string.Format(CultureInfo.CurrentCulture, 
                    Strings.GetResourceManager("ThisAssemblyTests.Resources").GetString("WithNamedFormat").Replace("{date:yyyy-MM}", "{0}"), 
                    ((IFormattable)date).ToString("yyyy-MM", CultureInfo.CurrentCulture));
        }
    }
}
```

## Customizing the generated code

The following MSBuild properties can be used to customize the generated code:

| Property                | Description                                                                                          |
|-------------------------|------------------------------------------------------------------------------------------------------|
| ThisAssemblyNamespace   | Sets the namespace of the generated `ThisAssembly` root class. If not set, it will be in the global namespace. |
| ThisAssemblyVisibility  | Sets the visibility modifier of the generated `ThisAssembly` root class. If not set, it will be internal. |

<!-- #strings -->
<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
<!-- exclude -->