using System.Collections.Generic;
using System.Linq;

namespace Utilities;

static class EnumerableExtensions
{
    public static IEnumerable<T> ConcatAll<T>(this IEnumerable<IEnumerable<T>> @this)
    {
        foreach (var enumerable in @this)
        {
            foreach (var item in enumerable)
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<string> TrimAll(this IEnumerable<string> @this)
        => @this.Select(s => s.Trim()).Where(s => s.Length > 0);
}