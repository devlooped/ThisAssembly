using System;
using System.Collections.Generic;

namespace Utilities;

sealed class KeyComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>>
    where TKey : IEquatable<TKey>
{
    readonly IEqualityComparer<TKey> _comparer;

    public KeyComparer(IEqualityComparer<TKey>? comparer = null)
    {
        _comparer = comparer ?? EqualityComparer<TKey>.Default;
    }

    public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) => _comparer.Equals(x.Key, y.Key);

    public int GetHashCode(KeyValuePair<TKey, TValue> obj) => _comparer.GetHashCode(obj.Key);
}
