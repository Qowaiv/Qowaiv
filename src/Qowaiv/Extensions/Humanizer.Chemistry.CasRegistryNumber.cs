namespace Qowaiv.Chemistry;

/// <summary>Extensions to create <see cref="CasRegistryNumber"/>s, inspired by Humanizer.NET.</summary>
public static class NumberToCasRegistryNumberExtensions
{
    /// <summary>Interprets the <see cref="int"/> if it was written as a CAS Registry Number.</summary>
    [Pure]
    public static CasRegistryNumber CasNr(this int number) => CasRegistryNumber.Create(number);

    /// <summary>Interprets the <see cref="long"/> if it was written as a CAS Registry Number.</summary>
    [Pure]
    public static CasRegistryNumber CasNr(this long number) => CasRegistryNumber.Create(number);
}
