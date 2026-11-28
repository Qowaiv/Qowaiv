using Specs;

namespace Extensions.QowaivRandom_specs;

public class Next
{
    public class Sequence
    {
        [Test]
        public void Iterates()
        {
            int[] thousand = [.. Rnd.New().NextSequence(r => r.Next()).Take(1000)];

            thousand.Should().HaveCount(1000)
                .And.OnlyHaveUniqueItems();
        }
    }

	public class Char
	{
		[Test]
		public void In_range()
		{
			var distribution = Enumerable.Range('A', 26).ToDictionary(c => (char)c, c => 0);

			var rnd = Rnd.New();

			for (var i = 0; i < 26 * 100_000; i++)
				distribution[rnd.NextChar('A', 'Z')]++;

			distribution.Should().AllSatisfy(kvp => kvp.Value.Should().BeInRange(95_000, 105_000));
		}

		[Test]
		public void Of_values()
		{
			var values = "0123456789ABCDEF";
			var distribution = values.ToDictionary(c => c, c => 0);

			var rnd = Rnd.New();

			for (var i = 0; i < values.Length * 100_000; i++)
				distribution[rnd.NextChar(values)]++;

			distribution.Should().AllSatisfy(kvp => kvp.Value.Should().BeInRange(95_000, 105_000));
		}
	}

    public class Fractions
    {
        [Test]
        [Arguments(-1)]
        [Arguments(+0)]
        [Arguments(+1)]
        public void Does_not_allow_denominators_below_2(int maxDenominator)
            => maxDenominator.Invoking(d => Rnd.Shared.NextFraction(Fraction.Zero, Fraction.One, maxDenominator))
            .Should().Throw<ArgumentOutOfRangeException>().WithMessage("The Max denominator should be [2, ..].*");

        [Test]
        public void Does_not_allow_unrepresentable_ranges()
            => Rnd.Shared.Invoking(r => r.NextFraction(1.DividedBy(3), 1.DividedBy(2), 2))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("No fraction in [1/3, 1/2) can be represented with a denominator up to 2.*");

        [Test]
        [Arguments("-1/12", "23/12")]
        [Arguments("-1000/12", "-50/12")]
        [Arguments("0", "1000")]
        [Arguments("1/2024", "1/3")]
        public void InRange(Fraction min, Fraction max) => Rnd.New()
            .NextSequence(r => r.NextFraction(min, max, 1024)).Take(1000)
            .Should().AllSatisfy(f => f.Should().BeInRange(min, max));

        [Test]
        public void Unique() => Rnd.New()
            .NextSequence(r => r.NextFraction()).Take(1000)
            .Should().OnlyHaveUniqueItems();
    }

	public class IBAN
	{
		[Test]
		public void For_country_with_checksum_at_start() => Rnd.New()
            .NextSequence(r => r.NextInternationalBankAccountNumber(Country.NL)).Take(10_000)
            .Should().OnlyHaveUniqueItems();

        [Test]
        [Arguments("ME")]
        [Arguments("MR")]
        [Arguments("PT")]
        [Arguments("RS")]
        [Arguments("TN")]
        public void For_country_with_checksum_at_end(Country country) => Rnd.New()
            .NextSequence(r => r.NextInternationalBankAccountNumber(country)).Take(10_000)
            .Should().OnlyHaveUniqueItems();

        [Test]
        public void For_country_without_IBAN() => Rnd.New()
            .NextSequence(r => r.NextInternationalBankAccountNumber(Country.US)).Take(10_000)
            .Should().OnlyHaveUniqueItems();

        [Test]
        public void For_random_country() => Rnd.New()
            .NextSequence(r => r.NextInternationalBankAccountNumber()).Take(10_000)
            .Should().OnlyHaveUniqueItems();

        [Test]
        [Arguments("", "empty")]
        [Arguments("?", "unknown")]
        [Arguments("SUHH", "former")]
        public void Does_not_support(Country country, string because)
            => Rnd.Shared.Invoking(r => r.NextInternationalBankAccountNumber(country))
            .Should().Throw<ArgumentOutOfRangeException>(because)
            .WithMessage("Only existing countries are supported.*");
    }
}
