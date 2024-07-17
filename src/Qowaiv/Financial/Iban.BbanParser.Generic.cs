namespace Qowaiv.Financial;

internal sealed class BbanGenericParser : BbanParser
{
    public BbanGenericParser() : base("ZZnncccccccccccccccccccccccccccccccc") { }

    [Pure]
    protected override Chars Buffer(int id)
        => Chars.Init(Length)
        + (char)((id / 26) + 'A')
        + (char)((id % 26) + 'A');

    [Pure]
    protected override string? CheckLength(Chars iban) => iban.Length >= 12 ? iban.ToString() : null;
}
