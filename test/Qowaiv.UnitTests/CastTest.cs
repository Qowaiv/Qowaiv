using NUnit.Framework;
using Qowaiv.Financial;
using System;
using System.Diagnostics;

namespace Qowaiv.UnitTests
{
    public class CastTest
    {
        private static readonly string InvalidString = "$%$!@$%$D!@@!!!!!";
        private static readonly int InvalidInt = int.MinValue + 17;

        [Test]
        public void InvalidCast_DoubleMaxValueToPercentage() => AssertInvalidCast(() => (Percentage)double.MaxValue);
        
        [Test]
        public void InvalidCast_DoubleMinValueToPercentage() => AssertInvalidCast(() => (Percentage)double.MinValue);

        [Test]
        public void InvalidCast_DoubleNaNToPercentage() => AssertInvalidCast(() => (Percentage)double.NaN);

        [Test]
        public void InvalidCast_DoubleNegativeInfinityToPercentage() => AssertInvalidCast(() => (Percentage)double.NegativeInfinity);

        [Test]
        public void InvalidCast_DoublePositiveInfinityToPercentage() => AssertInvalidCast(() => (Percentage)double.PositiveInfinity);

        [Test]
        public void InvalidCast_DoubleToAmount() => AssertInvalidCast(() => (Amount)double.MaxValue);

        [Test]
        public void InvalidCast_IntToHouseNumber() => AssertInvalidCast(() => (HouseNumber)InvalidInt);

        [Test]
        public void InvalidCast_IntToGender() => AssertInvalidCast(() => (Gender)InvalidInt);

        [Test]
        public void InvalidCast_IntToMonth() => AssertInvalidCast(() => (Month)InvalidInt);

        [Test]
        public void InvalidCast_IntToYear() => AssertInvalidCast(() => (Year)InvalidInt);

        [Test]
        public void InvalidCast_StringToDate() => AssertInvalidCast(() => (Date)InvalidString);

        [Test]
        public void InvalidCast_StringToEmail() => AssertInvalidCast(() => (EmailAddress)InvalidString);

        [Test]
        public void InvalidCast_StringToGender() => AssertInvalidCast(() => (Gender)InvalidString);

        [Test]
        public void InvalidCast_StringToHouseNumber() => AssertInvalidCast(() => (HouseNumber)InvalidString);

        [Test]
        public void InvalidCast_StringToLocalDateTime() => AssertInvalidCast(() => (LocalDateTime)InvalidString);

        [Test]
        public void InvalidCast_StringToMonth() => AssertInvalidCast(() => (Month)InvalidString);

        [Test]
        public void InvalidCast_StringToPercentage() => AssertInvalidCast(() => (Percentage)InvalidString);

        [Test]
        public void InvalidCast_StringToPostalCode() => AssertInvalidCast(() => (PostalCode)InvalidString);

        [Test]
        public void InvalidCast_StringToUuid() => AssertInvalidCast(() => (Uuid)InvalidString);
        
        [Test]
        public void InvalidCast_StringToWeekDate() => AssertInvalidCast(() => (WeekDate)InvalidString);

        [Test]
        public void InvalidCast_StringToYear() => AssertInvalidCast(() => (Year)InvalidString);

        [Test]
        public void InvalidCast_StringToYesNo() => AssertInvalidCast(() => (YesNo)InvalidString);

        /// <remarks>A cast can not be applied unless you assign it, therefore a <see cref="Func{TResult}"/>.</remarks>
        [DebuggerStepThrough]
        private static void AssertInvalidCast<TSvo>(Func<TSvo> cast)
        {
            var x = Assert.Catch<InvalidCastException>(() => cast());
            StringAssert.IsMatch($"Cast from .+ to {typeof(TSvo)} is not valid.", x.Message);
        }
    }
}
