using Qowaiv.ComponentModel.DataAnnotations;
using System;
using Xunit;

namespace Qowaiv.ComponentModel.Tests.DataAnnotations
{
	public class MandatoryAttributeTest
	{
		[Fact]
		public void IsValid_NewGuid_True()
		{
			var attr = new MandatoryAttribute();
			var act = attr.IsValid(Guid.NewGuid());
			Assert.True(act);
		}

		[Fact]
		public void IsValid_GuidEmpty_False()
		{
			var attr = new MandatoryAttribute();
			var act = attr.IsValid(Guid.Empty);
			Assert.False(act);
		}

		[Fact]
		public void IsValid_SomeEmailAddress_True()
		{
			var attr = new MandatoryAttribute();
			var act = attr.IsValid(EmailAddress.Parse("test@exact.com"));
			Assert.True(act);
		}

		[Fact]
		public void IsValid_EmailAddressEmpty_False()
		{
			var attr = new MandatoryAttribute();
			var act = attr.IsValid(EmailAddress.Empty);
			Assert.False(act);
		}
	}
}
