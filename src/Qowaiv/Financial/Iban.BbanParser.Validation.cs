using System.Runtime.CompilerServices;

namespace Qowaiv.Financial;

internal partial class BbanParser
{
    [Pure]
    private static bool Mod97(string iban)
    {
        var mod = 0;

        // Calculate the first 4 characters (country and checksum) last
        for (var i = 4; i < iban.Length; i++)
        {
            mod = Mod(mod, iban[i]);
        }
        for (var i = 0; i < 4; i++)
        {
            mod = Mod(mod, iban[i]);
        }
        return mod == 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int Mod(int mod, char ch)
        {
            var index = Mod97(ch);
            mod *= index > 9 ? 100 : 10;
            mod += index;
            return mod % 97;
        }
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Mod97(char ch)
        => ch <= '9'
            ? ch - '0'
            : ch - 'A' + 10;
}
