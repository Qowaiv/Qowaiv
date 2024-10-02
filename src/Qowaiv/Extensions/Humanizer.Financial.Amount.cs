namespace Qowaiv.Financial;

/// <summary>Extensions to create <see cref="Financial.Amount" />s, inspired by Humanizer.NET.</summary>
public static class NumberToAmountExtensions
{
    /// <summary>Converts the <see cref="decimal" /> to a <see cref="Financial.Amount" />.</summary>
    [Pure]
    public static Amount Amount(this decimal number) => Financial.Amount.Create(number);

    /// <summary>Converts the <see cref="double" /> to a <see cref="Financial.Amount" />.</summary>
    [Pure]
    public static Amount Amount(this double number) => Financial.Amount.Create(number);

    /// <summary>Converts the <see cref="double" /> to a <see cref="Financial.Amount" />.</summary>
    [Pure]
    public static Amount Amount(this int number) => Financial.Amount.Create((decimal)number);
}
