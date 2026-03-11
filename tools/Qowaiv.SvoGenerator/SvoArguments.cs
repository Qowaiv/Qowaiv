namespace Qowaiv.SvoGenerator;

public sealed record SvoArguments
{
    public SvoFeatures Features { get; init; } = SvoFeatures.Default;
    public Type Underlying { get; init; } = typeof(string);
    public string? Name { get; init; }
    public string? FullName { get; init; }
    public string Namespace { get; init; } = "Qowaiv";
    public string? FormatExceptionMessage { get; init; }
}
