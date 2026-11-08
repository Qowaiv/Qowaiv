#pragma warning disable S3898 // Value types should implement "IEquatable<T>"

namespace Qowaiv.Json.UnitTests.Models;

internal struct SvoWithFromJsonStringOnly
{
    public static SvoWithFromJsonStringOnly FromJson(string _) => default;
}
