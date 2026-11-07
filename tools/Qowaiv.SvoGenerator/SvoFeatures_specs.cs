using AwesomeAssertions;
using Qowaiv.SvoGenerator;

namespace SvoFeatures_specs;

public class Values
{
    [Test]
    [Arguments(SvoFeatures.Field)]
    [Arguments(SvoFeatures.GetHashCode)]
    [Arguments(SvoFeatures.IsEmpty)]
    [Arguments(SvoFeatures.IsUnknown)]
    [Arguments(SvoFeatures.IEquatable)]
    [Arguments(SvoFeatures.EqualsSvo)]
    [Arguments(SvoFeatures.ISerializable)]
    [Arguments(SvoFeatures.IJsonSerializable)]
    [Arguments(SvoFeatures.IFormattable)]
    [Arguments(SvoFeatures.IComparable)]
    [Arguments(SvoFeatures.ComparisonOperators)]
    [Arguments(SvoFeatures.Parsing)]
    [Arguments(SvoFeatures.Utf8)]
    public void Has_single_flag(SvoFeatures feature)
        => HasSingleBit((ulong)feature).Should().BeTrue();

    [Test]
    public void All_unique()
        => Enum.GetValues(typeof(SvoFeatures)).Cast<object>().Should().OnlyHaveUniqueItems();

    private static bool HasSingleBit(ulong value) => value != 0 && (value & (value - 1)) == 0;
}

