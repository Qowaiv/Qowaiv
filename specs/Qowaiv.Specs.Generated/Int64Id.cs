namespace Specs_Generated;

[Id<Behavior, long>]
public readonly partial struct Int64Id
{
    private sealed class Behavior : Int64IdBehavior
    {
        public override string ToString(long value, string? format, IFormatProvider? formatProvider)
            => string.Format(formatProvider, $"PREFIX{{0:{format}}}", value);

        public override bool TryTransform(string? str, IFormatProvider? formatProvider, out long id)
            => base.TryTransform(Trim(str), formatProvider, out id);

        public override object? ToJson(long value) => value is 0 ? null : value;

        private static string? Trim(string? str)
            => str is { Length: > 6 } && str[..6] == "PREFIX"
            ? str[6..]
            : str;
    }
}
