namespace Qowaiv.Identifiers;

/// <summary>Injectable behavior for strongly typed identifiers (<see cref="Id{TIdentifier}" />).</summary>
/// <remarks>
/// The behavior is not called for <see cref="Id{TIdentifier}.Empty" />.
/// </remarks>
public interface IIdentifierBehavior : IEqualityComparer<object>, IComparer<object>, IComparer
{
    /// <summary>Returns a type converter for the type of the underlying value.</summary>
    TypeConverter Converter { get; }

    /// <summary>Returns the type of the underlying value.</summary>
    Type BaseType { get; }

    /// <summary>Compares the underlying values and returns a value indicating
    /// whether one is less than, equal to, or greater than the other.
    /// </summary>
    [Pure]
    new int Compare(object? x, object? y);

    /// <summary>Returns a <see cref="byte" /> that represents the underlying value of the identifier.</summary>
    [Pure]
    byte[] ToByteArray(object? obj);

    /// <summary>Returns the underlying value of the identifier represented by a <see cref="byte" /> array.</summary>
    [Pure]
    object? FromBytes(byte[] bytes);

    /// <summary>Returns a formatted <see cref="string" /> that represents the underlying value of the identifier.</summary>
    [Pure]
    string ToString(object? obj, string? format, IFormatProvider? formatProvider);

    /// <summary>Deserializes the underlying value from a JSON number.</summary>
    [Pure]
    object? FromJson(long obj);

    /// <summary>Serializes the underlying value to a JSON node.</summary>
    [Pure]
    object? ToJson(object? obj);

    /// <summary>Tries to parse the underlying value of the identifier.</summary>
    /// <param name="str">
    /// The <see cref="string" /> to parse.
    /// </param>
    /// <param name="id">
    /// The parsed identifier.
    /// </param>
    /// <returns>
    /// True if the <see cref="string" /> could be parsed.
    /// </returns>
    [Pure]
    bool TryParse(string? str, out object? id);

    /// <summary>Tries to create the underlying value of the identifier.</summary>
    /// <param name="obj">
    /// The <see cref="object" /> that could represent the underlying value.
    /// </param>
    /// <param name="id">
    /// The underlying value.
    /// </param>
    /// <returns>
    /// True if the <see cref="object" /> could represent a valid underlying value.
    /// </returns>
    bool TryCreate(object? obj, out object? id);

    /// <summary>Creates a new (random) underlying value.</summary>
    [Pure]
    object Next();
}
