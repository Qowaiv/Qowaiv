namespace Qowaiv.TestTools;

/// <summary>Type converter builder to apply <see cref="TypeConverter.ConvertFrom(object)" />.</summary>
public sealed class ConvertFrom<TFrom>
{
    internal ConvertFrom(TFrom? subject) => Subject = subject;

    /// <summary>The subject that can be converted to a destination type.</summary>
    public TFrom? Subject { get; }

    /// <summary>Converts the value to the destination type, using its <see cref="TypeConverter" />.</summary>
    [Pure]
    public To? To<To>()
    {
        var converter = Converter<To>();

        return Subject switch
        {
            null => (To?)converter.ConvertFrom(null!),
            var s when !converter.CanConvertFrom(s.GetType()) => throw new NotSupportedException($"Converter {converter} can not convert from {Subject}."),
            string s => (To)converter.ConvertFromString(s)!,
            var s => (To)converter.ConvertFrom(s!)!,
        };
    }

    [Pure]
    private static TypeConverter Converter<To>() => TypeDescriptor.GetConverter(typeof(To));
}
