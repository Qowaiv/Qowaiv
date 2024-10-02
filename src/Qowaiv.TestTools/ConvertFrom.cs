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

        if (Subject is { } && !converter.CanConvertFrom(Subject.GetType()))
        {
            throw new NotSupportedException($"Converter {converter} can not convert from {Subject}.");
        }
        else return typeof(TFrom) == typeof(string)
#nullable disable // should not be a problem here
            ? (To)converter.ConvertFromString(Subject as string)
            : (To)converter.ConvertFrom(Subject);
#nullable enable
    }

    [Pure]
    private static TypeConverter Converter<To>() => TypeDescriptor.GetConverter(typeof(To));
}
