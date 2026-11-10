using Qowaiv.Financial;
using Qowaiv.IO;
using Qowaiv.Mathematics;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>'Validates if the item is negative.</summary>
[AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
[Validates(typeof(sbyte))]
[Validates(typeof(short))]
[Validates(typeof(int))]
[Validates(typeof(long))]
[Validates(typeof(float))]
[Validates(typeof(double))]
[Validates(typeof(decimal))]
[Validates(typeof(TimeSpan))]
#if NET8_0_OR_GREATER
[Validates(typeof(Int128))]
#endif
[Validates(typeof(Amount))]
[Validates(typeof(Fraction))]
[Validates(typeof(Money))]
[Validates(typeof(MonthSpan))]
[Validates(typeof(Percentage))]
[Validates(typeof(StreamSize))]
[Validates(typeof(YearSpan))]
[CLSCompliant(false)]
public sealed class NegativeAttribute() : SignAttribute(() => QowaivValidationMessages.NegativeAttribute_ValidationError)
{
    /// <inheritdoc />
    [Pure]
    protected override bool Is(int sign) => sign < 0;
}
