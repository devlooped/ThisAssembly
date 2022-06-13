using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities;

static class StringBuilderExtensions
{
    public static StringBuilder AppendIf(this StringBuilder @this, bool condition, string str)
        => condition ? @this.Append(str) : @this;

    public static StringBuilder AppendLineIf(this StringBuilder @this, bool condition, string str)
        => condition ? @this.Append(str) : @this;

    public static StringBuilder If(this StringBuilder @this, bool condition, Func<StringBuilder, StringBuilder> func)
        => condition ? func(@this) : @this;

    public static StringBuilder If(this StringBuilder @this, bool condition, Func<StringBuilder, StringBuilder> thenFunc, Func<StringBuilder, StringBuilder> elseFunc)
        => (condition ? thenFunc : elseFunc)(@this);

    public static StringBuilder ForEach<T>(this StringBuilder @this, IEnumerable<T> items, Func<StringBuilder, T, StringBuilder> appender)
    {
        foreach (var item in items)
        {
            @this = appender(@this, item);
        }

        return @this;
    }
}
