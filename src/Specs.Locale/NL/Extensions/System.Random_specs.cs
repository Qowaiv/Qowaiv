using Qowaiv.Locale.NL.Extensions;
using Specs;

namespace Extensions.QowaivRandom_specs;
public class Next
{
	public class BurgerserviceNummer
	{
		[Test]
		public void Generates_valid_numbers()
			=> Rnd.New().NextSequence(r => r.NextBurgerservicenummer()).Take(100)
            .Should().OnlyHaveUniqueItems()
            .And.AllSatisfy(v => v.IsKnown.Should().BeTrue());
    }

	public class RSIN
	{
		[Test]
		public void Generates_valid_numbers()
			=> Rnd.New().NextSequence(r => r.NextRsin()).Take(100)
            .Should().OnlyHaveUniqueItems()
            .And.AllSatisfy(v => v.IsKnown.Should().BeTrue());
	}
}
