namespace Specs_Generated;

[Id<Behavior, int>]
public readonly partial struct Int32Id
{
    private sealed class Behavior : Int32IdBehavior
    {
        public override string ToString(int value, string? format, IFormatProvider? formatProvider)
            => string.Format(formatProvider, $"PREFIX{{0:{format}}}", value);

        public override bool TryTransform(string? str, IFormatProvider? formatProvider, out int id)
            => base.TryTransform(Trim(str), formatProvider, out id);

        public override object? ToJson(int value) => value is 0 ? null : value;

        private static string? Trim(string? str)
            => str is { Length: > 6 } && str[..6] == "PREFIX"
            ? str[6..]
            : str;
    }
}
