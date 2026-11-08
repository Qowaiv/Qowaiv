namespace Qowaiv.Json.UnitTests.Models;

internal struct SvoWithFromJsonBoolOnly
{
    public bool ToJson() => this is { };

    public static SvoWithFromJsonBoolOnly FromJson(bool _) => default;
}
