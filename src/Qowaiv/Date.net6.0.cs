#if NET6_0_OR_GREATER

namespace Qowaiv;

public readonly partial struct Date
{
    /// <summary>Casts a <see cref="Date"/> implicitly to a <see cref="DateOnly"/>.</summary>
    public static implicit operator DateOnly(Date date) => new(date.Year, date.Month, date.Day);

    /// <summary>Casts a <see cref="DateOnly"/> explicitly to a <see cref="Date"/>.</summary>
    public static explicit operator Date(DateOnly date) => new(date.Year, date.Month, date.Day);
}
#endif
