using System.Runtime.CompilerServices;

namespace Qowaiv.Financial;

internal partial class BbanParser
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
        => Digit(iban[start]) * 10
        + Digit(iban[start + 1]);

    /// <summary>Returns the digit value of the char.</summary>
    /// <remarks>
    /// Assumes that the char represents a digit.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static int Digit(char c) => c - '0';

    [Pure]
    protected static int Mod97(string iban, int start, int end)
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
    protected static bool Mod97(string iban)
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

    /// <summary>Returns (mod - value) % mod.</summary>
    [Pure]
    protected static int MinMod(int value, int mod) => (mod - value) % mod;

    [Pure]
    protected static int Weighted(string iban, int start, int mod, params int[] weights)
    {
        var sum = 0;
        for (var i = 0; i < weights.Length; i++)
        {
            sum += ASCII.Digit(iban[start + i]) * weights[i];
        }
        return sum % mod;
    }

    [Pure]
    private static int Mod97(char ch)
        => ch <= '9'
            ? ch - '0'
            : ch - 'A' + 10;
}
