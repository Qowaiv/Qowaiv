using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.Globalization;
using Xunit;

namespace Qowaiv.ComponentModel.Tests.DataAnnotations
{
	public class ForbiddenValuesAtttributeTest
	{
		[Fact]
		public void IsValid_Null_True()
		{
			var attr = new ForbiddenValuesAttribute("DE", "FR", "GB");
			var act = attr.IsValid(null);
			Assert.True(act);
		}

		[Fact]
		public void IsValid_GB_False()
		{
			var attr = new ForbiddenValuesAttribute("DE", "FR", "GB");
			var act = attr.IsValid(Country.GB);
			Assert.False(act);
		}

		[Fact]
		public void IsValid_TR_True()
		{
			var attr = new ForbiddenValuesAttribute("DE", "FR", "GB");
			var act = attr.IsValid(Country.TR);
			Assert.True(act);
		}
	}
}
