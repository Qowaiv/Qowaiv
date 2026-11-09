namespace Qowaiv.Json.UnitTests.Models;

internal readonly struct SvoWithFromJsonBoolOnly
{
    public bool ToJson() => this is { };

    public static SvoWithFromJsonBoolOnly FromJson(bool _) => default;
}
