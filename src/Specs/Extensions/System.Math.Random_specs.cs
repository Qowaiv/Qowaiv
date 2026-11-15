using Specs;
using System.Xml;

namespace Extensions.QowaivRandom_specs;

public class Next
{
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

	public class IBAN
	{
		[Test]
		public void For_country_with_checksum_at_start()
		{
			var rnd = Rnd.New();
			var ibans = Enumerable.Range(0, 10_000).Select(_ => rnd.NextInternationalBankAccountNumber(Country.NL)).ToArray();
			ibans.Should().OnlyHaveUniqueItems();
		}

        [Test]
        [Arguments("ME")]
        [Arguments("MR")]
        [Arguments("PT")]
        [Arguments("RS")]
        [Arguments("TN")]
        public void For_country_with_checksum_at_end(Country country)
        {
            var rnd = Rnd.New();
            var ibans = Enumerable.Range(0, 10_000).Select(_ => rnd.NextInternationalBankAccountNumber(country)).ToArray();
            ibans.Should().OnlyHaveUniqueItems();
        }

        [Test]
        public void For_country_without_IBAN()
        {
            var rnd = Rnd.New();
            var ibans = Enumerable.Range(0, 10_000).Select(_ => rnd.NextInternationalBankAccountNumber(Country.US)).ToArray();
            ibans.Should().OnlyHaveUniqueItems();
        }


        [Test]
        public void For_random_country()
        {
            var rnd = Rnd.New();
            var ibans = Enumerable.Range(0, 100_000).Select(_ => rnd.NextInternationalBankAccountNumber()).ToArray();
            ibans.Should().OnlyHaveUniqueItems();
        }

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
