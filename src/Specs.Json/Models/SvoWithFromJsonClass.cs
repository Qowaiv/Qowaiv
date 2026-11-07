namespace Qowaiv.Json.UnitTests.Models;

internal class SvoWithFromJsonClass
{
    public SvoWithFromJsonClass(object? value) => Value = value;
    public object? Value { get; }
    public static SvoWithFromJsonClass FromJson(string json) => new(json);
    public static SvoWithFromJsonClass FromJson(double json) => new(json);
    public static SvoWithFromJsonClass FromJson(long json) => new(json);
    public static SvoWithFromJsonClass FromJson(bool json) => new(json);
    public object? ToJson() => Value;
}
