#if !NETSTANDARD2_0


namespace Specs_Generated;

/// <summary>Simple 'smart' enum.</summary>
[OpenApiDataType(description: "ABC enum", type: "string", example: "C", format: "ABC")]
[Svo<Behavior>]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1210:\"Equals\" and the comparison operators should be overridden when implementing \"IComparable\"", Justification = "Sorting is sufficient")]
public readonly partial struct AbcEnum : IEnumerated<AbcEnum>
{
    /// <inheritdoc />
    public static IReadOnlyCollection<AbcEnum> DefinedValues => [new("A"), new("B"), new("C")];

    private sealed class Behavior : SvoBehavior
    {
        public override bool IsValid(string str, IFormatProvider? formatProvider, out string validated)
        {
            if (str is "A" or "B" or "C")
            {
                validated = str;
                return true;
            }
            else
            {
                validated = default!;
                return false;
            }
        }
    }
}
#endif
