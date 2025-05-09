namespace Qowaiv.TestTools;

/// <summary>Type converter builder to apply <see cref="TypeConverter.ConvertTo(object, System.Type)" />.</summary>
public sealed class ConvertTo<To> : ConvertTo
{
    internal ConvertTo() : base(typeof(To)) { }

    /// <summary>Converts the value to the destination type, using the <see cref="TypeConverter" /> of the subject.</summary>
    [Pure]
    public To? From<TFrom>(TFrom subject) => (To?)Converter<TFrom>().ConvertTo(subject, typeof(To));
}
