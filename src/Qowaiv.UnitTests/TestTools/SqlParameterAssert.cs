using NUnit.Framework;
using System.Data;
using System.Data.SqlClient;

namespace Qowaiv.UnitTests.TestTools
{
	public static class SqlParameterAssert
	{
		public static void AreEqual(string parameterName, object value, DbType dbType, SqlParameter actual)
		{
			Assert.IsNotNull(actual, "Parameter should not be null.");
			Assert.AreEqual(parameterName, actual.ParameterName, "ParameterName");
			Assert.AreEqual(value, actual.Value, "Value");
			Assert.AreEqual(dbType, actual.DbType, "DbType");
		}
	}
}
