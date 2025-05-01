using Qowaiv;
using Qowaiv.Customization;
using Qowaiv.OpenApi;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Specs;

public static class Svo
{
    public static readonly CustomSvo CustomSvo = CustomSvo.Parse("QOWAIV");

    public static readonly CustomGuid CustomGuid = CustomGuid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");

    public static readonly CustomUuid CustomUuid = CustomUuid.Parse("Qowaiv_SVOLibrary_GUIA");

    /// <summary>8a1a8c42-d2ff-e254-e26e-b6abcbf19420.</summary>
    public static readonly Guid Guid = Guid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");

    /// <summary>Qowaiv_SVOLibrary_GUIA.</summary>
    public static readonly Uuid Uuid = Uuid.Parse("Qowaiv_SVOLibrary_GUIA");
}

[Id<GuidBehavior, Guid>]
public readonly partial struct CustomGuid { }

[Id<UuidBehavior, Uuid>]
public readonly partial struct CustomUuid { }

[OpenApiDataType(description: "Custom SVO Example", type: "string", example: "QOWAIV", format: "custom")]
[Svo<Behavior>]
public readonly partial struct CustomSvo
{
    private sealed class Behavior : SvoBehavior
    {
        public override int MinLength => 3;
        public override int MaxLength => 16;
        public override Regex Pattern => new("^[A-Z]+$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(1));

        public override string NormalizeInput(string? str, IFormatProvider? formatProvider)
            => str?.Replace("-", "").ToUpper(formatProvider ?? CultureInfo.InvariantCulture) ?? string.Empty;

        public override string InvalidFormatMessage(string? str, IFormatProvider? formatProvider)
            => "Is not a valid CustomSvo";
    }
}
