namespace Qowaiv.Financial;

internal static class IbanValidator
{
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
    public static bool Mod97(string iban)
    {
        var mod = 0;
        for (var i = 0; i < iban.Length; i++)
        {
            // Calculate the first 4 characters (country and checksum) last
            var ch = iban[(i + 4) % iban.Length];
            var index = Index(ch);
            mod *= index > 9 ? 100 : 10;
            mod += index;
            mod %= 97;
        }
        return mod == 1;

        static int Index(char ch)
            => ch <= '9'
                ? ch - '0'
                : ch - 'A' + 10;
    }
}
