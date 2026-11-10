#if NET10_0_OR_GREATER

using System.Numerics;

namespace Extensions.Number_specs;

public class ZeroIfNull
{
    [Test]
    [Arguments(1.0)]
    [Arguments(3.14)]
    public void Value_if_not_null(Amount? amount)
        => amount.ZeroIfNull().Should().Be(amount!.Value);

    [Test]
    [Arguments((object?)null)]
    public void Zero_if_null(Amount? amount)
       => amount.ZeroIfNull().Should().Be(Amount.Zero);
}
#endif
