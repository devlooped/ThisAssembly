using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
namespace System;

/// <summary>
/// Represent a range that has start and end indexes.
/// </summary>
/// <remarks>
/// Range is used by the C# compiler to support the range syntax.
/// <code>
/// int[] someArray = new int[5] { 1, 2, 3, 4, 5 };
/// int[] subArray1 = someArray[0..2]; // { 1, 2 }
/// int[] subArray2 = someArray[1..^0]; // { 2, 3, 4, 5 }
/// </code>
/// </remarks>
public readonly struct Range : IEquatable<Range>
{
    /// <summary>
    /// Represents the inclusive start index of the Range.
    /// </summary>
    public Index Start { get; }

    /// <summary>
    /// Represents the exclusive end index of the Range.
    /// </summary>
    public Index End { get; }

    /// <summary>
    /// Construct a Range object using the start and end indexes.
    /// </summary>
    /// <param name="start">The inclusive start index of the range.</param>
    /// <param name="end">The exclusive end index of the range.</param>
    public Range(Index start, Index end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Indicates whether the current Range object is equal to another object of the same type.
    /// </summary>
    /// <param name="obj">An object to compare with this object.</param>
    /// <returns><see langword="true"/> if the objects are equal;
    /// otherwise, <see langword="false"/>.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Range other && other.Start.Equals(Start) && other.End.Equals(End);

    /// <summary>
    /// Indicates whether the current Range object is equal to another Range object.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><see langword="true"/> if the objects are equal;
    /// otherwise, <see langword="false"/>.</returns>
    public bool Equals(Range other) => other.Start.Equals(Start) && other.End.Equals(End);

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
        => new HashCode()
            .Combine(Start.GetHashCode())
            .Combine(End.GetHashCode())
            .ToHashCode();

    /// <summary>
    /// Converts the value of the current Range object to its equivalent string representation.
    /// </summary>
    /// <returns>A string representation of the current instance.</returns>
    public override string ToString() => Start + ".." + End;

    /// <summary>
    /// Create a Range object starting from start index to the end of the collection.
    /// </summary>
    /// <param name="start">The inclusive start index of the range.</param>
    /// <returns>A newly-created Range.</returns>
    public static Range StartAt(Index start) => new(start, Index.End);

    /// <summary>
    /// Create a Range object starting from first element in the collection to the end Index.
    /// </summary>
    /// <param name="end">The exclusive end index of the range.</param>
    /// <returns>A newly-created Range.</returns>
    public static Range EndAt(Index end) => new(Index.Start, end);

    /// <summary>
    /// Create a Range object starting from first element to the end.
    /// </summary>
    /// <returns>A newly-created Range.</returns>
    public static Range All => new(Index.Start, Index.End);

    /// <summary>
    /// Calculate the start offset and length of range object using a collection length.
    /// </summary>
    /// <param name="length">The length of the collection that the range will be used with. This has to be a positive value.</param>
    /// <returns>A <see cref="ValueTuple"/> containing the start offset and length of the range.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The range exceeds the boundaries of the collection.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int Offset, int Length) GetOffsetAndLength(int length)
    {
        var startIndex = Start;
        var start = startIndex.IsFromEnd ? length - startIndex.Value : startIndex.Value;
        var endIndex = End;
        var end = endIndex.IsFromEnd ? length - endIndex.Value : endIndex.Value;
        return (uint)end > (uint)length || (uint)start > (uint)end
            ? throw new ArgumentOutOfRangeException(nameof(length))
            : ((int Offset, int Length))(start, end - start);
    }
}
