using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.Globalization;
using Xunit;

namespace Qowaiv.ComponentModel.Tests.DataAnnotations
{
	public class AllowedValuesAtttributeTest
	{
		[Fact]
		public void IsValid_Null_True()
		{
			var attr = new AllowedValuesAttribute("DE", "FR", "GB");
			var act = attr.IsValid(null);
			Assert.True(act);
		}

		[Fact]
		public void IsValid_GB_True()
		{
			var attr = new AllowedValuesAttribute("DE", "FR", "GB");
			var act = attr.IsValid(Country.GB);
			Assert.True(act);
		}

		[Fact]
		public void IsValid_TR_False()
		{
			var attr = new AllowedValuesAttribute("DE", "FR", "GB");
			var act = attr.IsValid(Country.TR);
			Assert.False(act);
		}
	}
}
