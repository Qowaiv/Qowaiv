﻿namespace Obsolete_code;

[Obsolete("Will be dropped when the next major version is released.")]
public class Will_be_dropped
{
    [Test]
    public void Date_Today() => Date.Today.Should().Be(Clock.Today());

    [Test]
    public void Date_Yesterday() => Date.Yesterday.Should().Be(Clock.Yesterday());

    [Test]
    public void Date_Tomorrow() => Date.Tomorrow.Should().Be(Clock.Tomorrow());

    [Test]
    public void Amount_Addition_with_implicit_cast() => (50.Amount() + 49m).Should().Be(99.Amount());

    [Test]
    public void Amount_Subtraction_with_implicit_cast() => (90.Amount() - 20m).Should().Be(70.Amount());

    [Test]
    public void Amount_Implictly_from_decimal()
    {
        Percentage casted = 0.1751m;
        casted.Should().Be(Svo.Percentage);
    }
}

[Obsolete("Will become private when the next major version is released.")]
public class Will_become_private { }
