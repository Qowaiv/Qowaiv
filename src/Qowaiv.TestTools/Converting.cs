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
