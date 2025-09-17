namespace Qowaiv.TestTools;

/// <summary>Type converter builder to apply <see cref="TypeConverter.ConvertTo(object, System.Type)" />.</summary>
public sealed class ConvertTo<To> : ConvertTo
{
    internal ConvertTo() : base(typeof(To)) { }

    private TypeConverter? Converter { get; init; }

    /// <summary>Defines the <see cref="TypeConverter"/> to use while converting.</summary>
    /// <typeparam name="TConverter">
    /// The type converter to use.
    /// </typeparam>
    [Pure]
    public ConvertTo<To> With<TConverter>() where TConverter : TypeConverter, new()
        => new() { Converter = new TConverter() };

    /// <summary>Converts the value to the destination type, using the <see cref="TypeConverter" /> of the subject.</summary>
    [Pure]
    public To? From<TFrom>(TFrom subject) => (To?)(Converter ?? Converter<TFrom>()).ConvertTo(subject, typeof(To));
}
