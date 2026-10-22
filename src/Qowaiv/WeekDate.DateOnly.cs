#if !NETSTANDARD2_0

namespace Qowaiv;

public readonly partial struct WeekDate
{
    /// <summary>Casts a <see cref="WeekDate" /> implicitly to a <see cref="DateOnly" />.</summary>
    public static implicit operator WeekDate(DateOnly date) => Create(date);

    /// <inheritdoc cref="Create(Date)" />
    [Pure]
    public static WeekDate Create(DateOnly val) => Create((Date)val);
}

#endif
