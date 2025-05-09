namespace Qowaiv.TestTools;

/// <summary>Type converter builder to apply <see cref="TypeConverter.ConvertTo(object, Type)" />.</summary>
public class ConvertTo
{
    /// <summary>The type to convert to.</summary>
    protected Type To { get; }

    /// <summary>Initializes a new instance of the <see cref="ConvertTo" /> class.</summary>
    internal ConvertTo(Type to) => To = Guard.NotNull(to);

    /// <summary>Converts the value to the destination type, using the <see cref="TypeConverter" /> of the subject.</summary>
    [Pure]
    public bool From<TFrom>() => Converter<TFrom>().CanConvertTo(To);

    /// <summary>Creates a type converter for <typeparamref name="TFrom"/>.</summary>
    [Pure]
    protected static TypeConverter Converter<TFrom>() => TypeDescriptor.GetConverter(typeof(TFrom));
}
