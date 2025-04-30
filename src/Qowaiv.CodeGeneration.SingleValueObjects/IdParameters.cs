namespace Qowaiv.CodeGeneration.SingleValueObjects;

public sealed record IdParameters : BaseParameters
{
    /// <summary>The (full name) of the Id behavior.</summary>
    public required string Behavior { get; init; }

    public required string Value { get; init; }
}
