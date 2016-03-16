using NUnit.Framework;
using Qowaiv.Data;
using Qowaiv.UnitTests.TestTools;
using System;
using System.Data;

namespace Qowaiv.UnitTests.Data
{
	[TestFixture]
	public class SvoParameterTest
	{
		[Test]
		public void CreateForSql_Null_DBNullValue()
		{
			var act = SvoParameter.CreateForSql("par", null);
			SqlParameterAssert.AreEqual("par", DBNull.Value, DbType.String, act);
		}

		[Test]
		public void CreateForSql_Int32ValueWithRetry_17()
		{
			SvoParameter.CreateForSql("par", 17);
			var act = SvoParameter.CreateForSql("par", 17);
			SqlParameterAssert.AreEqual("par", 17, DbType.Int32, act);
		}

		[Test]
		public void CreateForSql_NullablePercentage_DbNullValue()
		{
			var act = SvoParameter.CreateForSql("par", new Percentage?());
			SqlParameterAssert.AreEqual("par", DBNull.Value, DbType.String, act);
		}

		[Test]
		public void CreateForSql_IbanEmptyValue_DbNullValue()
		{
			var act = SvoParameter.CreateForSql("par", InternationalBankAccountNumber.Empty);
			SqlParameterAssert.AreEqual("par", DBNull.Value, DbType.String, act);
		}

		[Test]
		public void CreateForSql_GenderFemale_2()
		{
			var act = SvoParameter.CreateForSql("par", Gender.Female);
			SqlParameterAssert.AreEqual("par", 2, DbType.Byte, act);
		}

		[Test]
		public void CreateForSql_100Percent_1Decimal()
		{
			var act = SvoParameter.CreateForSql("par", Percentage.Hundred);
			SqlParameterAssert.AreEqual("par", 1m, DbType.Decimal, act);
		}

		[Test]
		public void Create_Nullable100Percent_1Decimal()
		{
			Percentage? val = 1m;
			var act = SvoParameter.CreateForSql("par", val);
			SqlParameterAssert.AreEqual("par", 1m, DbType.Decimal, act);
		}

		[Test]
		public void CreateForSql_UuidNew_GuidNew()
		{
			var val = Uuid.NewUuid();
			var exp = (Guid)val;
			var act = SvoParameter.CreateForSql("par", val);
			SqlParameterAssert.AreEqual("par", exp, DbType.Guid, act);
		}

		[Test]
		public void CreateForSql_StructWithoutCast_ThrowsInvalidCastException()
		{
			var act = Assert.Throws<InvalidCastException>(() =>
			{
				var val = new StructWithoutRequiredCast("test");
				SvoParameter.CreateForSql("par", val);
			});
			Assert.AreEqual("Cast from Qowaiv.UnitTests.Data.StructWithoutRequiredCast to System.String is not valid.", act.Message);
		}
	}

	[SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
	internal struct StructWithoutRequiredCast
	{
		private readonly string val;

		public StructWithoutRequiredCast(string str)
		{
			val = str;
		}
		public override string ToString() { return val; }
	}
}
