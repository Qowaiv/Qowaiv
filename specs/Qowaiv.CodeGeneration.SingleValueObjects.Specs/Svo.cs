using Qowaiv;
using Qowaiv.Customization;
using Qowaiv.OpenApi;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Specs;

public static class Svo
{
    public static readonly CustomSvo CustomSvo = CustomSvo.Parse("QOWAIV");

    public static readonly CustomGuid CustomGuid = CustomGuid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");

    public static readonly CustomUuid CustomUuid = CustomUuid.Parse("Qowaiv_SVOLibrary_GUIA");

    /// <summary>PREFIX17</summary>
    public static readonly Int32Id Int32Id = Int32Id.Create(17);

    /// <summary>PREFIX987654321</summary>
    public static readonly Int64Id Int64Id = Int64Id.Create(987654321L);

    /// <summary>8a1a8c42-d2ff-e254-e26e-b6abcbf19420.</summary>
    public static readonly Guid Guid = Guid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");

    /// <summary>Qowaiv_SVOLibrary_GUIA.</summary>
    public static readonly Uuid Uuid = Uuid.Parse("Qowaiv_SVOLibrary_GUIA");

    public static readonly StringId StringId = StringId.Parse("Qowaiv-ID");
}

[Id<GuidBehavior, Guid>]
public readonly partial struct CustomGuid { }

[Id<UuidBehavior, Uuid>]
public readonly partial struct CustomUuid { }

[Id<Behavior, int>]
public readonly partial struct Int32Id
{
    private sealed class Behavior : Int32IdBehavior
    {
        public override string ToString(int value, string? format, IFormatProvider? formatProvider)
            => string.Format(formatProvider, $"PREFIX{{0:{format}}}", value);

        public override bool TryTransform(string? str, IFormatProvider? formatProvider, out int id)
            => base.TryTransform(Trim(str), formatProvider, out id);

        public override object? ToJson(int value) => value is 0 ? null : value;

        private static string? Trim(string? str)
            => str is { Length: > 6 } && str[..6] == "PREFIX"
            ? str[6..]
            : str;
    }
}

[Id<Behavior, long>]
public readonly partial struct Int64Id
{
    private sealed class Behavior : Int64IdBehavior
    {
        public override string ToString(long value, string? format, IFormatProvider? formatProvider)
            => string.Format(formatProvider, $"PREFIX{{0:{format}}}", value);

        public override bool TryTransform(string? str, IFormatProvider? formatProvider, out long id)
            => base.TryTransform(Trim(str), formatProvider, out id);

        public override object? ToJson(long value) => value is 0 ? null : value;

        private static string? Trim(string? str)
            => str is { Length: > 6 } && str[..6] == "PREFIX"
            ? str[6..]
            : str;
    }
}

[Id<StringIdBehavior, string>]
public readonly partial struct StringId { }


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

[Id<Behavior, int>]
public readonly partial struct EvenOnlyId
{
    internal sealed class Behavior : Int32IdBehavior
    {
        public override bool TryTransform(int value, [NotNullWhen(true)] out int transformed)
            => base.TryTransform(value, out transformed) && value % 2 == 0;
    }
}
