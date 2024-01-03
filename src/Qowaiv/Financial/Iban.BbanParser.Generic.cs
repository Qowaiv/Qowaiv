namespace Qowaiv.Financial;

internal sealed class BbanGenericParser : BbanParser
{
    public BbanGenericParser() : base("ZZnncccccccccccccccccccccccccccccccc") { }

    [Pure]
    protected override char[] Buffer(int id)
    {
        var buffer = new char[Length];
        buffer[0] = (char)((id / 26) + 'A');
        buffer[1] = (char)((id % 26) + 'A');
        return buffer;
    }

    [Pure]
    protected override string? CheckLength(char[] iban, int length)
        => length >= 12
        ? new string(iban, 0, length)
        : null;
}
