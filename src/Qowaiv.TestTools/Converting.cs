namespace Qowaiv.TestTools;

/// <summary>Helper method to convert objects using <see cref="TypeConverter"/>s.</summary>
public static class Converting
{
    /// <summary>Initializes a new instance of the <see cref="ConvertFrom{From}"/> class.</summary>
    [Pure]
    public static ConvertFrom<TFrom> FromNull<TFrom>() where TFrom : class => new(null);

    /// <summary>Initializes a new instance of the <see cref="ConvertFrom{From}"/> class.</summary>
    [Pure]
    public static ConvertFrom<TFrom> From<TFrom>(TFrom subject) => new(subject);

    /// <summary>Initializes a new instance of the <see cref="ConvertTo{To}"/> class.</summary>
    [Pure]
    public static ConvertTo<To> To<To>() => new();

    /// <summary>Initializes a new instance of the <see cref="ConvertTo{To}"/> class.</summary>
    [Pure]
    public static new ConvertTo<string> ToString() => new();
}

/// <summary>Type converter builder to apply <see cref="TypeConverter.ConvertFrom(object)"/>.</summary>
public sealed class ConvertFrom<TFrom>
{
    internal ConvertFrom(TFrom? subject) => Subject = subject;

    /// <summary>The subject that can be converted to a destination type.</summary>
    public TFrom? Subject { get; }

    /// <summary>Converts the value to the destination type, using its <see cref="TypeConverter"/>.</summary>
    [Pure]
    public To? To<To>()
#nullable disable // should not be a problem here
        => typeof(TFrom) == typeof(string)
        ? (To)Converter<To>().ConvertFromString(Subject as string)
        : (To)Converter<To>().ConvertFrom(Subject);
#nullable enable

    [Pure]
    private static TypeConverter Converter<To>() => TypeDescriptor.GetConverter(typeof(To));
}

/// <summary>Type converter builder to apply <see cref="TypeConverter.ConvertTo(object, System.Type)"/>.</summary>
public sealed class ConvertTo<To>
{
    internal ConvertTo() { }

    /// <summary>Converts the value to the destination type, using the <see cref="TypeConverter"/> of the subject.</summary>
    [Pure]
    public To? From<TFrom>(TFrom subject) => (To?)Converter<TFrom>().ConvertTo(subject, typeof(To));

    [Pure]
    private static TypeConverter Converter<TFrom>() => TypeDescriptor.GetConverter(typeof(TFrom));
}
