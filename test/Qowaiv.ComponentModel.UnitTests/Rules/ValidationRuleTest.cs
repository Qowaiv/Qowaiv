using Qowaiv.Globalization;
using Xunit;

namespace Qowaiv.ComponentModel.UnitTests.Rules
{
	public class ValidationRuleTest
    {
		[Fact]
		public void Validate_DummyRule_CustomizedDutchMessage()
		{
			using (new CultureInfoScope("nl-NL"))
			{
				var rule = new DummyRule();
				var act = rule.Validate(null).ToString();
				var exp = "Dummy is fout.";
				Assert.Equal(exp, act);
			}
		}
    }
}
