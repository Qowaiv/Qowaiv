namespace Qowaiv.Json.UnitTests.Models;

internal readonly struct SvoWithFromJson : System.IEquatable<SvoWithFromJson>
{
    public SvoWithFromJson(object? value) => Value = value;
    public object? Value { get; }

    /// <summary>Convention based factory method.</summary>
    public static SvoWithFromJson FromJson(string json) => new(json);

    /// <summary>Convention based factory method.</summary>
    public static SvoWithFromJson FromJson(double json) => new(json);

    /// <summary>Convention based factory method.</summary>
    public static SvoWithFromJson FromJson(long json) => new(json);

    /// <summary>Convention based factory method.</summary>
    public static SvoWithFromJson FromJson(bool json) => new(json);

    /// <summary>Convention based serialization method.</summary>
    public object? ToJson() => Value;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is SvoWithFromJson other && Equals(other);
    
    /// <inheritdoc />
    public bool Equals(SvoWithFromJson other) => Equals(Value, other.Value);
    
    /// <inheritdoc />
    public override int GetHashCode() => Value is null ? 0 : Value.GetHashCode();

    /// <inheritdoc />
    public override string ToString() => Value?.ToString() ?? string.Empty;
}
