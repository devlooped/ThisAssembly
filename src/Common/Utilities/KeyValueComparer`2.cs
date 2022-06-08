using System;
using System.Collections.Generic;

namespace Utilities;

sealed class KeyValueComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>>
    where TKey : IEquatable<TKey>
    where TValue : IEquatable<TValue>
{
    readonly IEqualityComparer<TKey> _keyComparer;
    readonly IEqualityComparer<TValue> _valueComparer;

    public KeyValueComparer(
        IEqualityComparer<TKey>? keyComparer = null,
        IEqualityComparer<TValue>? valueComparer = null)
    {
        _keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
        _valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;
    }

    public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) => x.Key.Equals(y.Key) && x.Value.Equals(y.Value);

    public int GetHashCode(KeyValuePair<TKey, TValue> obj)
        => new HashCode()
            .Combine(_keyComparer.GetHashCode(obj.Key))
            .Combine(_valueComparer.GetHashCode(obj.Value))
            .ToHashCode();
}
