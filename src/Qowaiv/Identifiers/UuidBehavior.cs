namespace Qowaiv.Identifiers;

/// <summary>Implements <see cref="IIdentifierBehavior"/> for an identifier based on <see cref="Uuid"/>.</summary>
[OpenApiDataType(description: "UUID based identifier", example: "lmZO_haEOTCwGsCcbIZFFg", type: "string", format: "uuid-base64", nullable: true)]
public class UuidBehavior : GuidBehavior
{
    internal static new readonly UuidBehavior Instance = new();

    /// <summary>Initializes a new instance of the <see cref="UuidBehavior"/> class.</summary>
    protected UuidBehavior() { }

    /// <summary>Gets the default format used to represent the <see cref="Guid"/> as <see cref="string"/>.</summary>
    protected override string DefaultFormat => "S";

    /// <inheritdoc />
    [Pure]
    public override object? ToJson(object? obj)
        => obj is Guid id
            ? Base64.ToString(id)
            : null;

    /// <inheritdoc />
    public override bool TryParse(string? str, out object? id)
    {
        id = null;
        if (str is not { Length: > 0 })
        {
            return true;
        }
        else if (GuidParser.TryBase64(str, out var base64))
        {
            id = NullIfEmpty(base64);
            return true;
        }
        else if (Guid.TryParse(str, out var guid))
        {
            id = NullIfEmpty(guid);
            return true;
        }
        else if (GuidParser.TryBase32(str, out var base32))
        {
            id = NullIfEmpty(base32);
            return true;
        }
        else return false;

        static object? NullIfEmpty(Guid guid) => guid == Guid.Empty ? null : guid;
    }
}
