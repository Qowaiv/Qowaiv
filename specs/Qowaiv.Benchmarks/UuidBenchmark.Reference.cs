#pragma warning disable CS8605 // Unboxing a possibly null value.

using Qowaiv;
using Qowaiv.Identifiers;

namespace Benchmarks;

public partial class UuidBenchmark
{
    static readonly OldUidBehavior behavior = new();

    public static partial class Reference
    {
        public static Uuid Convert_FromBase64(string s)
        {
            if (behavior.TryParse(s, out var guid))
            {
                return (Uuid)(Guid)guid;
            }
            else throw new FormatException();
        }

        public static UuidVersion GetVersion(Guid guid)
        {
            var bytes = guid.ToByteArray();
            var version = bytes[7] >> 4;
            return (UuidVersion)version;
        }

        public static string ToString(Guid guid, string format = "S")
            => behavior.ToString(guid, format, null);
    }

    private sealed partial class OldUidBehavior : UuidBehavior
    {
        [GeneratedRegex(@"^[a-zA-Z0-9_-]{22}(=){0,2}$")]
        private static partial Regex GetPattern();

        public override bool TryParse(string? str, out object? id)
        {
            if (str is not { Length: > 0 })
            {
                id = default;
                return true;
            }
            else if (Guid.TryParse(str, out Guid guid))
            {
                id = NullIfEmpty(guid);
                return true;
            }
            else if (GetPattern().IsMatch(str))
            {
                id = NullIfEmpty(GuidFromBase64(str));
                return true;
            }
            else
            {
                id = default;
                return false;
            }

            static object? NullIfEmpty(Guid guid) => guid == Guid.Empty ? null : guid;

            static Guid GuidFromBase64(string s)
            {
                var base64 = new char[24];
                for (int i = 0; i < 22; i++)
                {
                    base64[i] = s[i] switch { '-' => '+', '_' => '/', _ => s[i] };
                }
                base64[22] = '=';
                base64[23] = '=';
                return new Guid(Convert.FromBase64String(new string(base64)));
            }
        }

        public override string ToString(object? obj, string? format, IFormatProvider? formatProvider)
            => obj is Guid guid
            ? ToString(guid, format):
            base.ToString(obj, format, formatProvider);

        private static string ToString(Guid guid, string? format)
            => format switch
            {
                "S" or "s" => ToBase64(guid),
                _ => guid.ToString(format),
            };

        private static string ToBase64(Guid guid)
            => Convert.ToBase64String(guid.ToByteArray()).Replace('+', '-').Replace('/', '_')[..22];
    }
}
