#if NET8_0_OR_GREATER

using System.Numerics;

namespace Extensions.Number_specs;

public class ZeroIfNull
{
    [TestCase(1.0)]
    [TestCase(3.14)]
    public void Value_if_not_null(Amount? amount)
        => amount.ZeroIfNull().Should().Be(amount!.Value);

    [TestCase(null)]
    public void Zero_if_null(Amount? amount)
       => amount.ZeroIfNull().Should().Be(Amount.Zero);
}
#endif
