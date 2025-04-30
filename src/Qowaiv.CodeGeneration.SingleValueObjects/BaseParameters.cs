namespace Qowaiv.CodeGeneration.SingleValueObjects;

/// <summary>The parameters for the SVO generator.</summary>
[Inheritable]
public record BaseParameters
{
    /// <summary>The (type) name of the SVO.</summary>
    public required string Svo { get; init; }

    /// <summary>Namespace of the SVO.</summary>
    public required Namespace Namespace { get; init; }
}
