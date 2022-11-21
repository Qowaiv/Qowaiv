namespace Qowaiv.Identifiers;

/// <summary>Implements <see cref="IIdentifierBehavior"/> for an identifier based on <see cref="int"/>.</summary>
[OpenApi.OpenApiDataType(description: "Int32 based identifier", example: 17, type: "integer", format: "identifier", nullable: true)]
public class Int32IdBehavior : IdentifierBehavior
{
    /// <summary>Creates a new instance of the <see cref="Int32IdBehavior"/> class.</summary>
    protected Int32IdBehavior() { }

    /// <summary>Returns the type of the underlying value (<see cref="int"/>).</summary>
    public sealed override Type BaseType => typeof(int);

    /// <inheritdoc/>
    [Pure]
    public override int Compare(object? x, object? y) => Id(x).CompareTo(Id(y));

    /// <inheritdoc/>
    [Pure]
    public override bool Equals(object? x, object? y) => Id(x).Equals(Id(y));

    /// <inheritdoc/>
    [Pure]
    public override int GetHashCode(object? obj) => Id(obj).GetHashCode();

    /// <inheritdoc/>
    [Pure]
    public override byte[] ToByteArray(object? obj)
        => obj is int num
        ? BitConverter.GetBytes(num)
        : Array.Empty<byte>();

    /// <inheritdoc/>
    [Pure]
    public override object? FromBytes(byte[] bytes) => BitConverter.ToInt32(bytes, 0);

    /// <inheritdoc/>
    [Pure]
    public override string ToString(object? obj, string? format, IFormatProvider? provider) => Id(obj).ToString(format, provider);

    /// <inheritdoc/>
    [Pure]
    public override object? FromJson(long obj)
    {
        if (obj == 0)
        {
            return null;
        }
        else if (obj > 0 && obj <= int.MaxValue)
        {
            return (int)obj;
        }
        else throw Exceptions.InvalidCast(typeof(int), typeof(Id<>).MakeGenericType(GetType()));
    }

    /// <inheritdoc/>
    [Pure]
    public override object? ToJson(object? obj) => Id(obj);

    /// <inheritdoc/>
    [Pure]
    public override bool TryParse(string? str, out object? id)
    {
        if (int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number) && number >= 0)
        {
            id = number == 0 ? null : (object)number;
            return true;
        }
        else
        {
            id = default;
            return false;
        }
    }

    /// <inheritdoc/>
    public override bool TryCreate(object? obj, out object? id)
    {
        if (obj is int num)
        {
            id = num == 0 ? null : (object)num;
            return true;
        }
        else if (TryParse(obj?.ToString(), out id))
        {
            return true;
        }
        else return false;
    }

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(int)
        || base.CanConvertFrom(context, sourceType);

    [Pure]
    private static int Id(object? obj) => obj is int number ? number : 0;
}
