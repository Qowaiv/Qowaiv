using Qowaiv.ComponentModel.Messages;
using Xunit;

namespace Qowaiv.ComponentModel.Tests
{
	public class ResultTest
	{
		internal static readonly ValidationMessage Error1 = ValidationMessage.Error("Error 1");
		internal static readonly ValidationMessage Error2 = ValidationMessage.Error("Error 2");
		internal static readonly ValidationMessage Warning1 = ValidationMessage.Warning("Warning 1");
		internal static readonly ValidationMessage Warning2 = ValidationMessage.Warning("Warning 2");
		internal static readonly ValidationMessage Info1 = ValidationMessage.Info("Info 1");
		internal static readonly ValidationMessage Info2 = ValidationMessage.Info("Info 2");
		internal static readonly ValidationMessage[] TestMessages =
		{
			Error1, Error2, Warning1, Warning2, Info1, Info2
		};

		[Fact]
		public void IsValid_TestMesages_False()
		{
			var result = new Result(TestMessages);
			var act = result.IsValid;
			Assert.False(act);
		}

		[Fact]
		public void IsValid_SomeNoneErrorsMesages_True()
		{
			var result = new Result(new[] { Warning1, Info1, Info2 });
			var act = result.IsValid;
			Assert.True(act);
		}

		[Fact]
		public void Errors_2Items()
		{
			var result = new Result(TestMessages);
			var act = result.Errors;
			var exp = new[] { Error1, Error2 };
			Assert.Equal(exp, act);
		}

		[Fact]
		public void Warnings_2Items()
		{
			var result = new Result(TestMessages);
			var act = result.Warnings;
			var exp = new[] { Warning1, Warning2 };
			Assert.Equal(exp, act);
		}

		[Fact]
		public void Infos_2Items()
		{
			var result = new Result(TestMessages);
			var act = result.Infos;
			var exp = new[] { Info1, Info2 };
			Assert.Equal(exp, act);
		}

		[Fact]
		public void Ctor_WithData_HasBeenSet()
		{
			var exp = 2;
			var result = new Result<int>(exp);
			var act = result.Data;

			Assert.Equal(exp, act);
		}

		[Fact]
		public void Implicit_Result_ToType()
		{
			var result = new Result<bool>(true);
			Assert.True(result);
		}

		[Fact]
		public void Implicit_Null_ToType()
		{
			Result<bool> result = null;
			Assert.False(result);
		}

		[Fact]
		public void Explicit_ToResultOfType()
		{
			var result =(Result<bool>)true;
			Assert.True(result.Data);
		}

		[Fact]
		public void WithError_IsNotValid()
		{
			var actual = new Result(new[] { ValidationMessage.Error("This should not be valid") });
			Assert.False(actual.IsValid);
		}

		[Fact]
		public void WithWarning_IsValid()
		{
			var actual = new Result(new[] { ValidationMessage.Warning("This should valid") });
			Assert.True(actual.IsValid);
		}

		[Fact]
		public void WithInfo_IsValid()
		{
			var actual = new Result(new[] { ValidationMessage.Info("This should be valid") });
			Assert.True(actual.IsValid);
		}
	}
}
