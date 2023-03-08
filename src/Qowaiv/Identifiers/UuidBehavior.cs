namespace Qowaiv.Identifiers;

/// <summary>Implements <see cref="IIdentifierBehavior"/> for an identifier based on <see cref="Uuid"/>.</summary>
[OpenApi.OpenApiDataType(description: "UUID based identifier", example: "lmZO_haEOTCwGsCcbIZFFg", type: "string", format: "uuid-base64", nullable: true)]
public class UuidBehavior : GuidBehavior
{
    internal static new readonly UuidBehavior Instance = new();

    /// <summary>Initializes a new instance of the <see cref="UuidBehavior"/> class.</summary>
    protected UuidBehavior() { }

    /// <summary>Gets the default format used to represent the <see cref="System.Guid"/> as <see cref="string"/>.</summary>
    protected override string DefaultFormat => "S";
}
