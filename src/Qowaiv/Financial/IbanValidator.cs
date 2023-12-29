namespace Qowaiv.Financial;

internal static class IbanValidator
{
    /// <summary>Gets the two digit checksum.</summary>
    /// <param name="iban">
    /// The IBAN string.
    /// </param>
    /// <param name="start">
    /// The start index of the checksum within the IBAN.
    /// </param>
    [Pure]
    public static int Checksum(string iban, int start)
       => ASCII.Digit(iban[start]) * 10
       + ASCII.Digit(iban[start + 1]);

    [Pure]
    public static int Weighted(string iban, params int[] weights)
    {
        var sum = 0;
        for (var i = 0; i < weights.Length; i++)
        {
            sum += ASCII.Digit(iban[i]) * weights[i];
        }
        return sum;
    }

    [Pure]
    public static int Mod97(string iban, int start, int end)
    {
        var mod = 0;
        for (var i = start; i < end; i++)
        {
            var index = Mod97(iban[i]);
            mod *= index > 9 ? 100 : 10;
            mod += index;
            mod %= 97;
        }
        return mod;
    }

    [Pure]
    public static bool Mod97(string iban)
    {
        var mod = 0;
        for (var i = 0; i < iban.Length; i++)
        {
            // Calculate the first 4 characters (country and checksum) last
            var ch = iban[(i + 4) % iban.Length];
            var index = Mod97(ch);
            mod *= index > 9 ? 100 : 10;
            mod += index;
            mod %= 97;
        }
        return mod == 1;
    }

    [Pure]
    static int Mod97(char ch)
        => ch <= '9'
            ? ch - '0'
            : ch - 'A' + 10;
}
