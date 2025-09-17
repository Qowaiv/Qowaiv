namespace Qowaiv.TestTools;

/// <summary>Type converter builder to apply <see cref="TypeConverter.ConvertFrom(object)" />.</summary>
public sealed class ConvertFrom<TFrom>
{
    internal ConvertFrom(TFrom? subject) => Subject = subject;

    /// <summary>The subject that can be converted to a destination type.</summary>
    public TFrom? Subject { get; }

    private TypeConverter? Converter { get; init; }

    /// <summary>Defines the <see cref="TypeConverter"/> to use while converting.</summary>
    /// <typeparam name="TConverter">
    /// The type converter to use.
    /// </typeparam>
    [Pure]
    public ConvertFrom<TFrom> With<TConverter>() where TConverter : TypeConverter, new()
        => new(Subject) { Converter = new TConverter() };

    /// <summary>Converts the value to the destination type, using its <see cref="TypeConverter" />.</summary>
    [Pure]
    public To? To<To>()
    {
        var converter = Init<To>();

        return Subject switch
        {
            null => (To?)converter.ConvertFrom(null!),
            var s when !converter.CanConvertFrom(s.GetType()) => throw new NotSupportedException($"Converter {converter} can not convert from {Subject}."),
            string s => (To)converter.ConvertFromString(s)!,
            var s => (To)converter.ConvertFrom(s!)!,
        };
    }

    [Pure]
    private TypeConverter Init<To>() => Converter ?? TypeDescriptor.GetConverter(typeof(To));
}
