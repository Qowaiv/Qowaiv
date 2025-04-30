namespace Qowaiv.CodeGeneration.SingleValueObjects;

/// <summary>The parameters for the SVO generator.</summary>
public sealed record SvoParameters : BaseParameters
{
    /// <summary>The (full name) of the SVO behavior.</summary>
    public required string Behavior { get; init; }
}
