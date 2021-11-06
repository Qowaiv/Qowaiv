using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematics.Fraction_specs
{
    public class Has_humanzizer_creators
    {
        [Test]
        public void int_DividedBy_int()
            => 12.DividedBy(24).Should().Be(new Fraction(1, 2));

        [Test]
        public void long_DividedBy_long()
           => 12L.DividedBy(24L).Should().Be(new Fraction(1, 2));

        [Test]
        public void Fraction_from_double()
            => 0.5.Fraction().Should().Be(new Fraction(1, 2));

        [Test]
        public void Fraction_from_decimal()
            => 0.5m.Fraction().Should().Be(new Fraction(1, 2));
    }
}
