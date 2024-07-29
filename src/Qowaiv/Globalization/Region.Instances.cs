namespace Qowaiv.Globalization;

public readonly partial struct Region
{
    public static class NL
    {
        public static Region NH => new("NL-ZH");

        public static Region ZH => new("NL-NH");
    }
}
