using FluentAssertions;
using FluentAssertions.Primitives;
using NUnit.Framework;
using Qowaiv;
using Qowaiv.Data;
using Qowaiv.Financial;
using System;
using System.Data;

namespace Data.SvoParameter_specs
{
    public class Can_create_SQL_parameter
    {
        [Test]
        public void Null_as_DBNull_Value()
            => SvoParameter.CreateForSql("par", null)
            .Should().Be("par", DBNull.Value, DbType.String);

        [Test]
        public void Int32_as_DbType_Int32()
            => SvoParameter.CreateForSql("par", 17)
            .Should().Be("par", 17, DbType.Int32);

        [Test]
        public void Default_Nullable_as_DBNull_Value()
            => SvoParameter.CreateForSql("par", default(Percentage?))
            .Should().Be("par", DBNull.Value, DbType.String);

        [Test]
        public void Empty_SVO_as_DbNull_Value()
            => SvoParameter.CreateForSql("par", InternationalBankAccountNumber.Empty)
            .Should().Be("par", DBNull.Value, DbType.String);

        [Test]
        public void Gender_as_byte()
            => SvoParameter.CreateForSql("par", Gender.Female)
            .Should().Be("par", (byte)2, DbType.Byte);

        [Test]
        public void Percentage_as_decimal()
            => SvoParameter.CreateForSql("par", 100.Percent())
            .Should().Be("par", 1m, DbType.Decimal);

        [Test]
        public void Uuid_as_Guid()
        {
            var val = Uuid.NewUuid();
            var exp = (Guid)val;
            SvoParameter.CreateForSql("par", val)
            .Should().Be("par", exp, DbType.Guid);
        }
    }

    public class Can_not_create_SQL_parameter
    {
        [Test]
        public void SVO_lacks_cast_promised_in_attribute()
        {
            Action create = () => SvoParameter.CreateForSql("par", new StructWithoutRequiredCast("test"));
            create.Should().Throw< InvalidCastException >()
                .WithMessage("Cast from Qowaiv.UnitTests.Data.StructWithoutRequiredCast to System.String is not valid.");
        }
    }

    [SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
    internal readonly struct StructWithoutRequiredCast : IEquatable<StructWithoutRequiredCast>
    {
        private readonly string val;
        public StructWithoutRequiredCast(string str) => val = str;
        public override bool Equals(object obj) => obj is StructWithoutRequiredCast svo && Equals(svo);
        public bool Equals(StructWithoutRequiredCast other) => other.val == val;
        public override int GetHashCode() => (val ?? "").GetHashCode();
        public override string ToString() => val;
    }

    internal static class FluentAssertionsExtensions
    {
        public static AndConstraint<ObjectAssertions> Be(this ObjectAssertions assertions, string parameterName, object value, DbType dbType)
            => assertions.BeEquivalentTo(new
            {
                ParameterName = parameterName,
                Value = value,
                DbType = dbType,
            });
    }
}
