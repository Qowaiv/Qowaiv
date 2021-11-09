using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Statistics;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.Statistics
{
    /// <summary>Tests the Elo SVO.</summary>
    public class EloTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Elo TestStruct = 1732.4;

        #region Elo const tests

        /// <summary>Elo.Zero should be equal to the default of Elo.</summary>
        [Test]
        public void Zero_None_EqualsDefault()
        {
            Assert.AreEqual(default(Elo), Elo.Zero);
        }
        [Test]
        public void MinValue_None_DoubleMinValue()
        {
            Assert.AreEqual((Elo)Double.MinValue, Elo.MinValue);
        }
        [Test]
        public void MaxValue_None_DoubleMaxValue()
        {
            Assert.AreEqual((Elo)Double.MaxValue, Elo.MaxValue);
        }

        #endregion

        #region NaN and +oo and -oo

        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void Create_Throws(double dbl)
        {
            var x = Assert.Catch<ArgumentOutOfRangeException>(() => Elo.Create(dbl));
            StringAssert.StartsWith("The number can not represent an Elo. ", x.Message);
        }

        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void Parse_Throws(double dbl)
        {
            Assert.Throws<FormatException>(() => Elo.Parse(dbl.ToString(CultureInfo.InvariantCulture)));
        }

        #endregion

        #region Methods

        [Test]
        public void GetZScore_Delta100_0Dot64()
        {
            var act = Elo.GetZScore(1600, 1500);
            var exp = 0.64;

            Assert.AreEqual(exp, act, 0.001);
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should not be valid.</summary>
        [Test]
        public void TryParse_Null_IsNotValid()
        {
            string str = null;
            Assert.IsFalse(Elo.TryParse(str, out _), "Valid");
        }

        /// <summary>TryParse string.Empty should not be valid.</summary>
        [Test]
        public void TryParse_StringEmpty_IsNotValid()
        {
            string str = string.Empty;
            Assert.IsFalse(Elo.TryParse(str, out _));
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TryParse_StringValue_IsValid()
        {
            string str = "1400";
            Assert.IsTrue(Elo.TryParse(str, out Elo val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.Catch<FormatException>
                (() =>
                {
                    Elo.Parse("InvalidInput");
                },
                "Not a valid Elo");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = TestStruct;
                var act = Elo.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void from_invalid_as_null_with_TryParse()
            => Elo.TryParse("invalid input").Should().BeNull();

        #endregion

        #region (XML) (De)serialization tests

        [Test]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = TestStruct;
            var info = new SerializationInfo(typeof(Elo), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual(1732.4000000000001, info.GetDouble("Value"));
        }

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = EloTest.TestStruct;
            var exp = EloTest.TestStruct;
            var act = SerializeDeserialize.Binary(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = EloTest.TestStruct;
            var exp = EloTest.TestStruct;
            var act = SerializeDeserialize.DataContract(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlSerialize_TestStruct_AreEqual()
        {
            var act = Serialize.Xml(TestStruct);
            var exp = "1732.4";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act =Deserialize.Xml<Elo>("1732.4");
            Assert.AreEqual(TestStruct, act);
        }

        [Test]
        public void SerializeDeserialize_EloSerializeObject_AreEqual()
        {
            var input = new EloSerializeObject
            {
                Id = 17,
                Obj = EloTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EloSerializeObject
            {
                Id = 17,
                Obj = EloTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.Binary(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_EloSerializeObject_AreEqual()
        {
            var input = new EloSerializeObject
            {
                Id = 17,
                Obj = EloTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EloSerializeObject
            {
                Id = 17,
                Obj = EloTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.Xml(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void DataContractSerializeDeserialize_EloSerializeObject_AreEqual()
        {
            var input = new EloSerializeObject
            {
                Id = 17,
                Obj = EloTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EloSerializeObject
            {
                Id = 17,
                Obj = EloTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.DataContract(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void SerializeDeserialize_Zero_AreEqual()
        {
            var input = new EloSerializeObject
            {
                Id = 17,
                Obj = Elo.Zero,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EloSerializeObject
            {
                Id = 17,
                Obj = Elo.Zero,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializeDeserialize.Binary(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void GetSchema_None_IsNull()
        {
            IXmlSerializable obj = TestStruct;
            Assert.IsNull(obj.GetSchema());
        }

        #endregion

        #region JSON (De)serialization tests

        [TestCase("Invalid input")]
        [TestCase("2017-06-11")]
        public void FromJson_Invalid_Throws(object json)
        {
            Assert.Catch<FormatException>(() => JsonTester.Read<Elo>(json));
        }
        [TestCase(1600, "1600*")]
        [TestCase(1700, "1700")]
        [TestCase(1234, 1234L)]
        [TestCase(1258.9, 1258.9)]
        public void FromJson(Elo expected, object json)
        {
            var actual = JsonTester.Read<Elo>(json);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = 1732.4000000000001;
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("00000", FormatProvider.CustomFormatter);
            var exp = "Unit Test Formatter, value: '01732', format: '00000'";

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString_TestStruct_ComplexPattern()
        {
            using (new CultureInfoScope(CultureInfo.InvariantCulture))
            {
                var act = TestStruct.ToString("");
                var exp = "1732.4";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_ValueDutchBelgium_AreEqual()
        {
            using (TestCultures.Nl_BE.Scoped())
            {
                var act = Elo.Parse("1600,1").ToString();
                var exp = "1600,1";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_ValueEnglishGreatBritain_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var act = Elo.Parse("1600.1").ToString();
                var exp = "1600.1";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueDutchBelgium_AreEqual()
        {
            using (TestCultures.Nl_BE.Scoped())
            {
                var act = Elo.Parse("800").ToString("0000");
                var exp = "0800";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueEnglishGreatBritain_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var act = Elo.Parse("800").ToString("0000");
                var exp = "0800";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueSpanishEcuador_AreEqual()
        {
            var act = Elo.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
            var exp = "01700,0";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IEquatable tests

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = Elo.Parse("1600", CultureInfo.InvariantCulture);
            var r = Elo.Parse("1,600.00*", CultureInfo.InvariantCulture);

            Assert.IsTrue(l.Equals(r));
        }

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of Elos ascending.</summary>
        [Test]
        public void OrderBy_Elo_AreEqual()
        {
            Elo item0 = 1601;
            Elo item1 = 2371;
            Elo item2 = 2416;
            Elo item3 = 2601;

            var inp = new List<Elo> { Elo.Zero, item3, item2, item0, item1, Elo.Zero };
            var exp = new List<Elo> { Elo.Zero, Elo.Zero, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of Elos descending.</summary>
        [Test]
        public void OrderByDescending_Elo_AreEqual()
        {
            Elo item0 = 1601;
            Elo item1 = 2371;
            Elo item2 = 2416;
            Elo item3 = 2601;

            var inp = new List<Elo> { Elo.Zero, item3, item2, item0, item1, Elo.Zero };
            var exp = new List<Elo> { item3, item2, item1, item0, Elo.Zero, Elo.Zero };
            var act = inp.OrderByDescending(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Compare with a to object casted instance should be fine.</summary>
        [Test]
        public void CompareTo_ObjectTestStruct_0()
        {
            object other = TestStruct;

            var exp = 0;
            var act = TestStruct.CompareTo(other);

            Assert.AreEqual(exp, act);
        }

        /// <summary>Compare with null should return 1.</summary>
        [Test]
        public void CompareTo_null_1()
        {
            object @null = null;
            Assert.AreEqual(1, TestStruct.CompareTo(@null));
        }

        /// <summary>Compare with a random object should throw an exception.</summary>
        [Test]
        public void CompareTo_newObject_ThrowsArgumentException()
        {
            Func<int> compare = () => TestStruct.CompareTo(new object());
            compare.Should().Throw<ArgumentException>();
        }

        [Test]
        public void LessThan_17LT19_IsTrue()
        {
            Elo l = 17;
            Elo r = 19;

            Assert.IsTrue(l < r);
        }
        [Test]
        public void GreaterThan_21LT19_IsTrue()
        {
            Elo l = 21;
            Elo r = 19;

            Assert.IsTrue(l > r);
        }

        [Test]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            Elo l = 17;
            Elo r = 19;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            Elo l = 21;
            Elo r = 19;

            Assert.IsTrue(l >= r);
        }

        [Test]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            Elo l = 17;
            Elo r = 17;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            Elo l = 21;
            Elo r = 21;

            Assert.IsTrue(l >= r);
        }
        #endregion

        #region Casting tests
        
        [Test]
        public void Implicit_DoubleToElo_AreEqual()
        {
            Elo exp = Elo.Create(1600.1);
            Elo act = 1600.1;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_EloToDouble_AreEqual()
        {
            var exp = 1600.1;
            var act = (Double)Elo.Create(1600.1);

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Implicit_DecimalToElo_AreEqual()
        {
            Elo exp = Elo.Create(1600.1);
            Elo act = 1600.1m;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_EloToDecimal_AreEqual()
        {
            var exp = 1600.1m;
            var act = (Decimal)Elo.Create(1600.1);

            Assert.AreEqual(exp, act);
        }


        [Test]
        public void Implicit_Int32ToElo_AreEqual()
        {
            Elo exp = Elo.Create(1600);
            Elo act = 1600;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_EloToInt32_AreEqual()
        {
            var exp = 1600;
            var act = (Int32)Elo.Create(1600);

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Properties
        #endregion

        #region Operators

        [Test]
        public void Add_1600Add100_1700()
        {
            Elo l = 1600;
            Elo r = 100;

            Elo act = l + r;
            Elo exp = 1700;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Subtract_1600Subtract100_1500()
        {
            Elo l = 1600;
            Elo r = 100;

            Elo act = l - r;
            Elo exp = 1500;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Divde_1600Subtract100_800()
        {
            Elo act = 1600m;

            act /= 2.0;

            Elo exp = 800;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Divde_1600Subtract100_3200()
        {
            Elo act = 1600m;

            act *= 2.0;

            Elo exp = 3200;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Increment_1600_1601()
        {
            Elo act = 1600;
            act++;

            Elo exp = 1601;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Decrement_1600_1599()
        {
            Elo act = 1600;
            act--;

            Elo exp = 1599;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Negate_Min1600_1600()
        {
            Elo act = -1600;

            act = -act;

            Elo exp = 1600;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Plus_1600_1600()
        {
            Elo act = 1600;

            act = +act;

            Elo exp = 1600;

            Assert.AreEqual(exp, act);
        }
        #endregion

        #region IsValid tests

        [Test]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(Elo.IsValid("Complex"), "Complex");
            Assert.IsFalse(Elo.IsValid((String)null), "(String)null");
            Assert.IsFalse(Elo.IsValid(string.Empty), "string.Empty");
        }
        [Test]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(Elo.IsValid("1754.8*"));
        }
        #endregion
    }

    [Serializable]
    public class EloSerializeObject
    {
        public int Id { get; set; }
        public Elo Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
