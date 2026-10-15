#if NET10_0_OR_GREATER

using System.Text.Json;

namespace Qowaiv.Json;

internal static class JsonReader
{
    extension(Utf8JsonReader reader)
    {
        public bool IsUknown => reader.ValueSpan is { Length: 1 } && reader.ValueSpan[0] is (byte)'?';

        [DoesNotReturn]
        public TSvo UnexpectedToken<TSvo>()
            => throw new JsonException($"Unexpected token parsing {typeof(TSvo).FullName}. {reader.TokenType} is not supported.");
    }
}
#endif
