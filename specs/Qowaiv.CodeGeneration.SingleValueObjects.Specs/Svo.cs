using Qowaiv.Customization;
using Qowaiv.OpenApi;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Specs;

public static class Svo
{
    public static readonly CustomSvo CustomSvo = CustomSvo.Parse("QOWAIV");
}

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
