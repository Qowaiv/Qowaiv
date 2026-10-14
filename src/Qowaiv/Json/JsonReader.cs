#if NET10_0_OR_GREATER

using System.Text.Json;

namespace Qowaiv.Json;

internal static class JsonReader
{
    extension(ref Utf8JsonReader reader)
    {
        [DoesNotReturn]
        public TSvo UnexpectedToken<TSvo>()
            => throw new JsonException($"Unexpected token parsing {typeof(TSvo).FullName}. {reader.TokenType} is not supported.");
    }
}
#endif
