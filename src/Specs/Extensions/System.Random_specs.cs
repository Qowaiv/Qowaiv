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
        public void InRange() => Rnd.New()
            .NextSequence(r => r.NextFraction(-23.DividedBy(7), 84.DividedBy(13), 1024)).Take(1000)
            .Should().AllSatisfy(f => f.Should().BeInRange(-23.DividedBy(7), 84.DividedBy(13)));

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
