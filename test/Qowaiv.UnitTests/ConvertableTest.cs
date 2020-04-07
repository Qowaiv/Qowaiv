using NUnit.Framework;
using Qowaiv.Reflection;
using System;
using System.Globalization;
using System.Linq;

namespace Qowaiv.Tests
{
    public class ConvertableTest
    {
        internal static readonly Type[] Svos = typeof(Date).Assembly
            .GetTypes()
            .Where(tp => QowaivType.IsSingleValueObject(tp))
            .OrderBy(tp => tp.Namespace)
            .ThenBy(tp => tp.Name)
            .ToArray();
        
        internal static readonly Type[] Convertibles = Svos
            .Except(new[] 
            { 
                typeof(DateSpan),
            })
            .ToArray();

        [TestCase(typeof(Date), TypeCode.DateTime)]
        [TestCase(typeof(EmailAddress), TypeCode.String)]
        [TestCase(typeof(Gender), TypeCode.Int32)]
        [TestCase(typeof(HouseNumber), TypeCode.Int32)]
        [TestCase(typeof(LocalDateTime), TypeCode.DateTime)]
        [TestCase(typeof(Month), TypeCode.Byte)]
        [TestCase(typeof(MonthSpan), TypeCode.Int32)]
        [TestCase(typeof(Percentage), TypeCode.Decimal)]
        [TestCase(typeof(PostalCode), TypeCode.String)]
        [TestCase(typeof(Uuid), TypeCode.String)]
        [TestCase(typeof(WeekDate), TypeCode.DateTime)]
        [TestCase(typeof(Year), TypeCode.Int16)]
        [TestCase(typeof(YesNo), TypeCode.Boolean)]
        [TestCase(typeof(Security.Cryptography.CryptographicSeed), TypeCode.String)]
        [TestCase(typeof(Statistics.Elo), TypeCode.Double)]
        [TestCase(typeof(Financial.Amount), TypeCode.Decimal)]
        [TestCase(typeof(Financial.BusinessIdentifierCode), TypeCode.String)]
        [TestCase(typeof(Financial.Currency), TypeCode.String)]
        [TestCase(typeof(Financial.InternationalBankAccountNumber), TypeCode.String)]
        [TestCase(typeof(Financial.Money), TypeCode.Decimal)]
        [TestCase(typeof(IO.StreamSize), TypeCode.Int64)]
        [TestCase(typeof(Globalization.Country), TypeCode.String)]
        [TestCase(typeof(Web.InternetMediaType), TypeCode.String)]
        public void HasTypeCode(Type type, TypeCode expected)
        {
            var instance = (IConvertible)Activator.CreateInstance(type);
            var typeCode = instance.GetTypeCode();
            Assert.AreEqual(expected, typeCode);
        }

        [TestCase(typeof(Gender), 0.0)]
        [TestCase(typeof(HouseNumber), 0.0)]
        [TestCase(typeof(Month), 0.0)]
        [TestCase(typeof(Percentage), 0.0)]
        [TestCase(typeof(Year), 0.0)]
        [TestCase(typeof(YesNo), 0.0)]
        [TestCase(typeof(Statistics.Elo), 0.0)]
        [TestCase(typeof(Financial.Amount), 0.0)]
        [TestCase(typeof(Financial.Money), 0.0)]
        [TestCase(typeof(IO.StreamSize), 0.0)]
        public void ToDecimal(Type type, decimal expected)
        {
            var instance = (IConvertible)Activator.CreateInstance(type);
            var converted = instance.ToDecimal(CultureInfo.InvariantCulture);
            Assert.AreEqual(expected, converted);
        }

        [TestCase(typeof(Date))]
        [TestCase(typeof(EmailAddress))]
        [TestCase(typeof(LocalDateTime))]
        [TestCase(typeof(PostalCode))]
        [TestCase(typeof(Uuid))]
        [TestCase(typeof(WeekDate))]
        [TestCase(typeof(Security.Cryptography.CryptographicSeed))]
        [TestCase(typeof(Financial.BusinessIdentifierCode))]
        [TestCase(typeof(Financial.Currency))]
        [TestCase(typeof(Financial.InternationalBankAccountNumber))]
        [TestCase(typeof(Globalization.Country))]
        [TestCase(typeof(Web.InternetMediaType))]
        public void ToDecimal_Throws(Type type)
        {
            var instance = (IConvertible)Activator.CreateInstance(type);
            var exeception = Assert.Catch(()=> instance.ToDecimal(CultureInfo.InvariantCulture));
            var x = exeception.GetType();
            Assert.IsTrue(x == typeof(FormatException) || x == typeof(InvalidCastException), "Actual: {0}", exeception);
        }
    }
}
