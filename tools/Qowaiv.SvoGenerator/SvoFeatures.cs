namespace Qowaiv.SvoGenerator;

[Flags]
public enum SvoFeatures
{
    Field = /*...............*/ 0x00001,
    GetHashCode = /*.........*/ 0x00002,
    IsEmpty = /*.............*/ 0x00004,
    IsUnknown = /*...........*/ 0x00008,
    IEquatable = /*..........*/ 0x00010,
    EqualsSvo = /*...........*/ 0x00020,
    ISerializable = /*.......*/ 0x00040,
    IJsonSerializable = /*...*/ 0x00080,
    IFormattable = /*........*/ 0x00100,
    IComparable = /*.........*/ 0x00200,
    ComparisonOperators = /*.*/ 0x00400,
    Parsing = /*.............*/ 0x00800,
    Validation = /*..........*/ 0x01000,
    Utf8 = /*................*/ 0x02000,
    All = Field
        | GetHashCode
        | IsEmpty
        | IsUnknown
        | IEquatable
        | EqualsSvo
        | ISerializable
        | IJsonSerializable
        | IFormattable
        | IComparable
        | ComparisonOperators
        | Parsing
        | Validation,

    Structure = Field | IsEmpty | IsUnknown,
    EqualsSvoAndGetHashCode = EqualsSvo | GetHashCode,
    IsEmptyOrUnknown = IsEmpty | IsUnknown,
    Default = All ^ ComparisonOperators ^ Validation,
    Continuous = All ^ IsEmpty ^ IsUnknown ^ Validation,
}
