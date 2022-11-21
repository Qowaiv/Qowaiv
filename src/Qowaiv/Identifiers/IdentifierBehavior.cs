namespace Qowaiv.Identifiers;

/// <summary>Base implementation for
/// <see cref="GuidBehavior"/>,
/// <see cref="Int32IdBehavior"/>,
/// <see cref="Int64IdBehavior"/>, and
/// <see cref="StringIdBehavior"/>
/// solving the type conversion part.
/// </summary>
public abstract class IdentifierBehavior : TypeConverter, IIdentifierBehavior
{
    /// <inheritdoc />
    public virtual TypeConverter Converter => this;

    /// <inheritdoc/>
    public abstract Type BaseType { get; }

    /// <inheritdoc/>
    [Pure]
    public abstract int Compare(object? x, object? y);

    /// <inheritdoc/>
    [Pure]
    public new abstract bool Equals(object? x, object? y);

    /// <inheritdoc/>
    [Pure]
    public abstract object? FromBytes(byte[] bytes);

    /// <inheritdoc/>
    [Pure]
    public virtual object? FromJson(long obj) => throw new NotSupportedException();

    /// <inheritdoc/>
    [Pure]
    public abstract int GetHashCode(object? obj);

    /// <inheritdoc/>
    [Pure]
    public virtual object Next() => throw new NotSupportedException();

    /// <inheritdoc/>
    [Pure]
    public abstract byte[] ToByteArray(object? obj);

    /// <inheritdoc/>
    [Pure]
    public abstract object? ToJson(object? obj);

    /// <inheritdoc/>
    [Pure]
    public abstract string ToString(object? obj, string? format, IFormatProvider? provider);

    /// <inheritdoc/>
    [Pure]
    public abstract bool TryCreate(object? obj, out object? id);

    /// <inheritdoc/>
    [Pure]
    public abstract bool TryParse(string? str, out object? id);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public sealed override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        if (value is null) return null;
        else if (TryCreate(value, out var result)) return result;
        else throw Exceptions.InvalidCast(value.GetType(), typeof(Id<>).MakeGenericType(GetType()));
    }
}
