#if !NETSTANDARD2_0

namespace Qowaiv;

public readonly partial struct LocalDateTime
{
    /// <summary>Casts a <see cref="LocalDateTime" /> implicitly to a <see cref="DateOnly" />.</summary>
    public static explicit operator DateOnly(LocalDateTime date) => new(date.Year, date.Month, date.Day);

    /// <summary>Casts a <see cref="DateOnly" /> explicitly to a <see cref="Date" />.</summary>
    public static explicit operator LocalDateTime(DateOnly date) => new(date.Year, date.Month, date.Day);
}

#endif
