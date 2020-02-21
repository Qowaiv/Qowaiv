using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests
{
    public class PercentageTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Percentage TestStruct = 0.1751m;

        public static NumberFormatInfo GetCustomNumberFormatInfo()
        {
            var info = new NumberFormatInfo
            {
                PercentSymbol = "!",
                PerMilleSymbol = "#",
                PercentDecimalSeparator = "*",
            };
            return info;
        }

        #region Percentage const tests

        /// <summary>Percentage.Zero should be equal to the default of Percentage.</summary>
        [Test]
        public void Zero_None_EqualsDefault()
        {
            Assert.AreEqual(default(Percentage), Percentage.Zero);
        }

        [Test]
        public void One_None_0Dot01()
        {
            Assert.AreEqual((Percentage)0.01m, Percentage.One);
        }

        [Test]
        public void Hundred_None_1()
        {
            Assert.AreEqual((Percentage)1m, Percentage.Hundred);
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TyrParse_Null_IsInvalid()
        {
            string str = null;
            Assert.IsFalse(Percentage.TryParse(str, out Percentage val), "Valid");
            Assert.AreEqual(Percentage.Zero, val, "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsInvalid()
        {
            string str = string.Empty;
            Assert.IsFalse(Percentage.TryParse(str, out Percentage val), "Valid");
            Assert.AreEqual(Percentage.Zero, val, "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            using (new CultureInfoScope("nl-NL"))
            {
                string str = "17,51%";
                Assert.IsTrue(Percentage.TryParse(str, out Percentage val), "Valid");
                Assert.AreEqual(str, val.ToString(), "Value");
            }
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "string";
            Assert.IsFalse(Percentage.TryParse(str, out Percentage val), "Valid");
            Assert.AreEqual(Percentage.Zero, val, "Value");
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (new CultureInfoScope("en-GB"))
            {
                Assert.Catch<FormatException>
                (() =>
                {
                    Percentage.Parse("InvalidInput");
                },
                "Not a valid percentage");
            }
        }

        [Test]
        public void ParseFrench_Percentage17Comma51_AreEqual()
        {
            using (new CultureInfoScope("fr-FR"))
            {
                var act = Percentage.Parse("%17,51");
                Assert.AreEqual(TestStruct, act);
            }
        }

        [Test]
        public void TryParse_PercentageMarkInvalidPosition_AreEqual()
        {
            Assert.IsFalse(Percentage.TryParse("2%2", out Percentage act));
            Assert.AreEqual(Percentage.Zero, act);
        }
        [Test]
        public void TryParse_PerMilleMarkInvalidPosition_AreEqual()
        {
            Assert.IsFalse(Percentage.TryParse("2‰2", out Percentage act));
            Assert.AreEqual(Percentage.Zero, act);
        }
        [Test]
        public void TryParse_PerTenThousendMarkInvalidPosition_AreEqual()
        {
            Assert.IsFalse(Percentage.TryParse("2‱2", out Percentage act));
            Assert.AreEqual(Percentage.Zero, act);
        }

        [Test]
        public void Parse_CustomPercentageMarker_IsValid()
        {
            Percentage act = Percentage.Parse("44*45!", GetCustomNumberFormatInfo());
            Percentage exp = 0.4445;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Parse_CustomPerMilleMarker_IsValid()
        {
            Percentage act = Percentage.Parse("44#", GetCustomNumberFormatInfo());
            Percentage exp = 0.044;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Parse_WithSpaceBetweenNumberAndMarker_IsValid()
        {
            Percentage act = Percentage.Parse("44.05 %", CultureInfo.InvariantCulture);
            Percentage exp = 0.4405;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Create tests

        [Test]
        public void Create_DecimalValue_AreEqual()
        {
            var exp = TestStruct;
            var act = Percentage.Create(0.1751m);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Create_DoubleValue_AreEqual()
        {
            var exp = TestStruct;
            var act = Percentage.Create(0.1751);
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region (XML) (De)serialization tests

        [Test]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<Percentage>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(Percentage), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<Percentage>(info, default);
            });
        }

        [Test]
        public void GetObjectData_Null_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                ISerializable obj = TestStruct;
                obj.GetObjectData(null, default);
            },
            "info");
        }

        [Test]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = TestStruct;
            var info = new SerializationInfo(typeof(Percentage), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual(0.1751m, info.GetDecimal("Value"));
        }

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = TestStruct;
            var exp = TestStruct;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = TestStruct;
            var exp = TestStruct;
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlSerialize_TestStruct_AreEqual()
        {
            var act = SerializationTest.XmlSerialize(TestStruct);
            var exp = "17.51%";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<Percentage>("17.51%");
            Assert.AreEqual(TestStruct, act);
        }

        [Test]
        public void SerializeDeserialize_PercentageSerializeObject_AreEqual()
        {
            var input = new PercentageSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new PercentageSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_PercentageSerializeObject_AreEqual()
        {
            var input = new PercentageSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new PercentageSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void DataContractSerializeDeserialize_PercentageSerializeObject_AreEqual()
        {
            var input = new PercentageSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new PercentageSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void SerializeDeserialize_Zero_AreEqual()
        {
            var input = new PercentageSerializeObject
            {
                Id = 17,
                Obj = Percentage.Zero,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new PercentageSerializeObject
            {
                Id = 17,
                Obj = Percentage.Zero,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
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

        [Test]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            Assert.Catch<FormatException>(() =>
            {
                JsonTester.Read<Percentage>("InvalidStringValue");
            },
            "Not a valid percentage");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<Percentage>("17.51%");
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_DoubleValue_AreEqual()
        {
            var act = JsonTester.Read<Percentage>((double)TestStruct);
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }


        [Test]
        public void ToJson_DefaultValue_IsZero()
        {
            object act = JsonTester.Write(default(Percentage));
            object exp = "0%";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = "17.51%";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(Percentage));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("17.51%", TestStruct);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            using (CultureInfoScope.NewInvariant())
            {
                var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
                var exp = "Unit Test Formatter, value: '17.51%', format: 'Unit Test Format'";

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_IFormatProviderNull_FormattedString()
        {
            using (new CultureInfoScope("nl-BE"))
            {
                var act = TestStruct.ToString((IFormatProvider)null);
                var exp = "17,51%";

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_Zero_0()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = Percentage.Zero.ToString();
                var exp = "0%";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_ZeroNullFormat_0()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = Percentage.Zero.ToString((String)null);
                var exp = "0";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToPercentageString_TestStruct_0()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = TestStruct.ToString();
                var exp = "17.51%";
                Assert.AreEqual(exp, act);
            }
        }
        [Test]
        public void ToPerMilleString_TestStruct_0()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = TestStruct.ToPerMilleString();
                var exp = "175.1‰";
                Assert.AreEqual(exp, act);
            }
        }
        [Test]
        public void ToPerTenThousandMarkString_TestStruct_0()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = TestStruct.ToPerTenThousandMarkString();
                var exp = "1751‱";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_ValueDutchBelgium_AreEqual()
        {
            using (new CultureInfoScope("nl-BE"))
            {
                var act = Percentage.Parse("1600,1").ToString();
                var exp = "1600,1%";
                Assert.AreEqual(exp, act);
            }
        }
        [Test]
        public void ToString_ValueDutchNetherlands_AreEqual()
        {
            using (new CultureInfoScope("nl-NL"))
            {
                var act = Percentage.Parse("1600,1").ToString("");
                var exp = "1600,1";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_ValueEnglishGreatBritain_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = Percentage.Parse("1600.1").ToString();
                var exp = "1600.1%";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueDutchBelgium_AreEqual()
        {
            using (new CultureInfoScope("nl-BE"))
            {
                var act = Percentage.Parse("800").ToString("0000");
                var exp = "0800";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueEnglishGreatBritain_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var act = Percentage.Parse("800").ToString("0000");
                var exp = "0800";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueSpanishEcuador_AreEqual()
        {
            var act = Percentage.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
            var exp = "01700,0";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_PercentageEnGB_FormattedString()
        {
            using (new CultureInfoScope("en-GB"))
            {
                Assert.AreEqual("31.415%", ((Percentage).31415).ToString());
            }
        }

        [Test]
        public void ToString_PercentageNlBE_FormattedString()
        {
            using (new CultureInfoScope("nl-BE"))
            {
                Assert.AreEqual("31,415%", ((Percentage).31415).ToString());
            }
        }

        [Test]
        public void ToString_PercentageFrFR_FormattedString()
        {
            using (new CultureInfoScope("fr-FR"))
            {
                Assert.AreEqual("%31,415", ((Percentage).31415).ToString());
            }
        }
        [Test]
        public void ToString_PercentageFaIR_FormattedString()
        {
            using (new CultureInfoScope("fa-IR"))
            {
                Assert.AreEqual("%31/415", ((Percentage).31415).ToString());
            }
        }

        [Test]
        public void ToString_InvalidPercentageMarkPosition_ThrowsException()
        {
            Percentage val = 0.33m;

            Assert.Catch<FormatException>
            (() =>
            {
                val.ToString("%#%");
            },
            "Input string was not in a correct format.");
        }

        [Test]
        public void ToStringWithFormat_PercentageEnGB_FormattedString()
        {
            using (new CultureInfoScope("en-GB"))
            {
                Assert.AreEqual("11.11", ((Percentage).1111).ToString("0.00"));
                Assert.AreEqual("022.22%", ((Percentage).2222).ToString("000.00%"));
                Assert.AreEqual("22.22 %", ((Percentage).2222).ToString("0.00 %"));
                Assert.AreEqual("%033.33", ((Percentage).3333).ToString("%000.00"));
                Assert.AreEqual("44.4%", ((Percentage).4444).ToString(@"0.#\%"));
                Assert.AreEqual(@"55.6\%", ((Percentage).5555).ToString(@"0.#\\%"));
                Assert.AreEqual(@"66.7\%", ((Percentage).6666).ToString(@"0.#\\\%"));
                Assert.AreEqual(@"777.77‰", ((Percentage).77777).ToString(@"0.0#‰"));
                Assert.AreEqual(@"‰777.78", ((Percentage).77778).ToString(@"‰0.0#"));
                Assert.AreEqual(@"8888.88‱", ((Percentage).888888).ToString(@"0.0#‱"));
                Assert.AreEqual(@"‱8888.89", ((Percentage).888889).ToString(@"‱0.0#"));
            }
        }

        [Test]
        public void ToString_CustomFormatter_UsesCustomPercentage()
        {
            Percentage val = 0.33m;

            var act = val.ToString(GetCustomNumberFormatInfo());
            var exp = "33!";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_UsesCustomPerMille()
        {
            Percentage val = 0.33m;

            var act = val.ToString("0‰", GetCustomNumberFormatInfo());
            var exp = "330#";

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_SameValue_SameHash()
        {
            var hash0 = TestStruct.GetHashCode();
            var hash1 = 17.51.Percent().GetHashCode();
            Assert.AreEqual(hash0, hash1);
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = Percentage.Parse("17", CultureInfo.InvariantCulture);
            var r = Percentage.Parse("17.0%", CultureInfo.InvariantCulture);

            Assert.IsTrue(l.Equals(r));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.Equals(TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals((Percentage)0.18m));
        }

        [Test]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(((Percentage)0.1751m).Equals((object)TestStruct));
        }

        [Test]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(null));
        }

        [Test]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(new object()));
        }

        [Test]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = TestStruct;
            var r = TestStruct;
            Assert.IsTrue(l == r);
        }

        [Test]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = TestStruct;
            var r = TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of Percentages ascending.</summary>
        [Test]
        public void OrderBy_Percentage_AreEqual()
        {
            Percentage item0 = 0.0185m;
            Percentage item1 = 0.1230m;
            Percentage item2 = 0.2083m;
            Percentage item3 = 0.3333m;

            var inp = new List<Percentage> { item3, item2, item0, item1 };
            var exp = new List<Percentage> { item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of Percentages descending.</summary>
        [Test]
        public void OrderByDescending_Percentage_AreEqual()
        {
            Percentage item0 = 0.0185m;
            Percentage item1 = 0.1230m;
            Percentage item2 = 0.2083m;
            Percentage item3 = 0.3333m;

            var inp = new List<Percentage> { item3, item2, item0, item1 };
            var exp = new List<Percentage> { item3, item2, item1, item0 };
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
            ExceptionAssert.CatchArgumentException
            (() =>
                {
                    object other = new object();
                    TestStruct.CompareTo(other);
                },
                "obj",
                "Argument must be Percentage."
            );
        }

        [Test]
        public void LessThan_17LT19_IsTrue()
        {
            Percentage l = 0.17m;
            Percentage r = 0.19m;

            Assert.IsTrue(l < r);
        }
        [Test]
        public void GreaterThan_21LT19_IsTrue()
        {
            Percentage l = 0.21m;
            Percentage r = 0.19m;

            Assert.IsTrue(l > r);
        }

        [Test]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            Percentage l = 0.17m;
            Percentage r = 0.19m;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            Percentage l = 0.21m;
            Percentage r = 0.19m;

            Assert.IsTrue(l >= r);
        }

        [Test]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            Percentage l = 0.17m;
            Percentage r = 0.17m;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            Percentage l = 21.0;
            Percentage r = 21.0;

            Assert.IsTrue(l >= r);
        }
        #endregion

        #region Percentage manipulation tests

        [TestCase(0.1234, -0.1234)]
        [TestCase(0.1234, +0.1234)]
        public void Abs(Percentage expected, Percentage value)
        {
            var abs = value.Abs();
            Assert.AreEqual(expected, abs);
        }

        [Test]
        public void UnaryNegation_Percentage17_Min17()
        {
            Percentage act = 0.17m;
            Percentage exp = -0.17m;

            act = -act;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void UnaryPlus_Percentage17_17()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.17m;

            act = +act;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Multiply_Percentage17Percentage42_741()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.0714m;
            Percentage mut = 0.42m;

            act *= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Devide_Percentage17Percentage50_34()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.34m;
            Percentage mut = 0.50m;

            act /= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Add_Percentage17Percentage42_59()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.59m;
            Percentage mut = 0.42m;

            act += mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtract_Percentage17Percentage13_59()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.04m;
            Percentage mut = 0.13m;

            act -= mut;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Multiply_Percentage17Decimal42_741()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.0714m;
            decimal mut = 0.42m;

            act *= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Percentage17Double42_741()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.0714m;
            double mut = 0.42;

            act *= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Percentage17Single42_741()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.0714m;
            float mut = 0.42F;

            act *= mut;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Multiply_Percentage17Int6442_34()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.34m;
            long mut = 2;

            act *= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Percentage17Int2342_34()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.34m;
            int mut = 2;

            act *= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Percentage17Int1642_34()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.34m;
            short mut = 2;

            act *= mut;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Multiply_Percentage17UInt6442_34()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.34m;
            ulong mut = 2;

            act *= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Percentage17UInt2342_34()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.34m;
            uint mut = 2;

            act *= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Percentage17UInt1642_34()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.34m;
            ushort mut = 2;

            act *= mut;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Division_Percentage17Decimal42_085()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.4047619047619047619047619048m;
            decimal mut = 0.42m;

            act /= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Percentage17Double42_085()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.4047619047619047619047619048m;
            double mut = 0.42;

            act /= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Percentage17Single42_085()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.4047619047619047619047619048m;
            float mut = 0.42F;

            act /= mut;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Division_Percentage17Int6442_085()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.085m;
            long mut = 2;

            act /= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Percentage17Int2342_085()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.085m;
            int mut = 2;

            act /= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Percentage17Int1642_085()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.085m;
            short mut = 2;

            act /= mut;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Division_Percentage17UInt6442_085()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.085m;
            ulong mut = 2;

            act /= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Percentage17UInt2342_085()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.085m;
            uint mut = 2;

            act /= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Percentage17UInt1642_085()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.085m;
            ushort mut = 2;

            act /= mut;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Number manipulation tests

        [Test]
        public void Increment_Percentage10_Percentage11()
        {
            Percentage act = 0.1m;
            Percentage exp = 0.11m;
            act++;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Decrement_Percentage10_Percentage09()
        {
            Percentage act = 0.1m;
            Percentage exp = 0.09m;
            act--;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Addition_Int1617Percentage10_18()
        {
            short act = 17;
            short exp = 18;
            act += 10.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Addition_Int3217Percentage10_18()
        {
            int act = 17;
            int exp = 18;
            act += 10.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Addition_Int6417Percentage10_18()
        {
            long act = 17;
            long exp = 18;
            act += 10.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Addition_UInt1617Percentage50_25()
        {
            ushort act = 17;
            ushort exp = 25;
            act += 50.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Addition_UInt3217Percentage50_25()
        {
            uint act = 17;
            uint exp = 25;
            act += 50.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Addition_UInt6417Percentage50_25()
        {
            ulong act = 17;
            ulong exp = 25;
            act += 50.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Addition_Decimal17Percentage17_19D89()
        {
            decimal act = 17;
            decimal exp = 19.89m;
            act += Percentage.Create(0.17);

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Addition_Double17Percentage17_19D89()
        {
            double act = 17;
            double exp = 19.89;
            act += Percentage.Create(0.17);

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Addition_Single17Percentage17_19D89()
        {
            float act = 17;
            float exp = 19.89F;
            act += Percentage.Create(0.17);

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Subtraction_Int1617Percentage10_16()
        {
            short act = 17;
            short exp = 16;
            act -= 10.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_Int3217Percentage10_16()
        {
            int act = 17;
            int exp = 16;
            act -= 10.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_Int6417Percentage10_16()
        {
            long act = 17;
            long exp = 16;
            act -= 10.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_UInt1617Percentage50_9()
        {
            ushort act = 17;
            ushort exp = 9;
            act -= 50.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_UInt3217Percentage50_9()
        {
            uint act = 17;
            uint exp = 9;
            act -= 50.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_UInt6417Percentage50_9()
        {
            ulong act = 17;
            ulong exp = 9;
            act -= 50.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_Decimal17Percentage17_11D39()
        {
            decimal act = 17;
            decimal exp = 11.39m;
            act -= 33.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_Double17Percentage17_11D39()
        {
            double act = 17;
            double exp = 11.39;
            act -= 33.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_Single17Percentage17_11D3899994()
        {
            float act = 17;
            float exp = 11.3899994F;
            act -= 33.Percent();

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Division_Int1617Percentage51_33()
        {
            short act = 17;
            short exp = 33;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Int3217Percentage51_33()
        {
            int act = 17;
            int exp = 33;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Int6417Percentage51_33()
        {
            long act = 17;
            long exp = 33;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_UInt1617Percentage51_33()
        {
            ushort act = 17;
            ushort exp = 33;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_UInt3217Percentage51_33()
        {
            uint act = 17;
            uint exp = 33;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_UInt6417Percentage51_33()
        {
            ulong act = 17;
            ulong exp = 33;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Decimal17Percentage51_33()
        {
            decimal act = 17;
            decimal exp = 100.0m / 3.0m;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Double17Percentage51_33()
        {
            double act = 17;
            double exp = 100.0 / 3.0;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Single17Percentage51_33()
        {
            float act = 17;
            float exp = 100.0F / 3.0F;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Multiply_Int1617Percentage51_8()
        {
            short act = 17;
            short exp = 8;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Int3217Percentage51_8()
        {
            int act = 17;
            int exp = 8;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Int6417Percentage51_8()
        {
            long act = 17;
            long exp = 8;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_UInt1617Percentage51_8()
        {
            ushort act = 17;
            ushort exp = 8;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_UInt3217Percentage51_8()
        {
            uint act = 17;
            uint exp = 8;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_UInt6417Percentage51_8()
        {
            ulong act = 17;
            ulong exp = 8;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Decimal17Percentage51_8D67()
        {
            decimal act = 17;
            decimal exp = 8.67m;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Double17Percentage51_8D67()
        {
            double act = 17;
            double exp = 8.67;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Single17Percentage51_8D67()
        {
            float act = 17;
            float exp = 8.67F;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Math-like methods tests

        [Test]
        public void Max_FirstLargest_First()
        {
            var actual = Percentage.Max(TestStruct, 0.007);
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void Max_SecondLargest_Second()
        {
            var actual = Percentage.Max(0.1, TestStruct);
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void Max_Values_Largest()
        {
            var actual = Percentage.Max(0.1, TestStruct, 0.02, 0.17);
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void Min_FirstSmallest_Frist()
        {
            var actual = Percentage.Min(TestStruct, 0.9);
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void Min_SecondSmallest_Second()
        {
            var actual = Percentage.Min(0.9, TestStruct);
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void Min_Values_Smallest()
        {
            var actual = Percentage.Min(0.9, TestStruct, 0.18, 0.74);
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void Round_Minus27Digits_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => TestStruct.Round(-27));
        }

        [Test]
        public void Round_27Digits_Throws()
        {
            var exception = Assert.Catch<ArgumentOutOfRangeException>(() => TestStruct.Round(27));
            StringAssert.StartsWith("Percentages can only round to between -26 and 26 digits of precision.", exception.Message);
        }

        [Test]
        public void Round_18Percent()
        {
            var actual = TestStruct.Round();
            Assert.AreEqual(18.Percent(), actual);
        }

        [Test]
        public void Round_1decimal_17d5Percent()
        {
            var actual = TestStruct.Round(1);
            Assert.AreEqual(17.5.Percent(), actual);
        }

        [Test]
        public void Round_AwayFromZero_17Percent()
        {
            var actual = 16.5.Percent().Round(0, DecimalRounding.AwayFromZero);
            Assert.AreEqual(17.Percent(), actual);
        }

        [Test]
        public void Round_ToEven_16Percent()
        {
            var actual = 16.5.Percent().Round(0, DecimalRounding.ToEven);
            Assert.AreEqual(16.Percent(), actual);
        }

        [Test]
        public void RoundToMultiple_3Percent_15Percent()
        {
            var actual = 16.4.Percent().RoundToMultiple(3.Percent());
            Assert.AreEqual(15.Percent(), actual);
        }


        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToPercentage_AreEqual()
        {
            var exp = TestStruct;
            var act = (Percentage)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_PercentageToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Percent()

        [Test]
        public void Percent_Int_IsPercentage()
        {
            var p = 3.Percent();
            Assert.AreEqual("3%", p.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void Percent_Double_IsPercentage()
        {
            var p = 3.14.Percent();
            Assert.AreEqual("3.14%", p.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void Percent_Decimal_IsPercentage()
        {
            var p = 3.14m.Percent();
            Assert.AreEqual("3.14%", p.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_Percentage_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(Percentage));
        }

        [Test]
        public void CanConvertFromInt32()
        {
            TypeConverterAssert.ConvertFromEquals((Percentage)2.0m, 2);
        }
        [Test]
        public void CanConvertToDecimal()
        {
            TypeConverterAssert.ConvertToEquals(0.1751m, TestStruct);
        }

        [Test]
        public void CanConvertFromString_Percentage_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(Percentage));
        }

        [Test]
        public void CanConvertToString_Percentage_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(Percentage));
        }

        [Test]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(TestStruct, TestStruct.ToString());
            }
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertToStringEquals(TestStruct.ToString(), TestStruct);
            }
        }

        #endregion

        #region IsValid tests

        [Test]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(Percentage.IsValid("Complex"), "Complex");
            Assert.IsFalse(Percentage.IsValid(null), "(String)null");
            Assert.IsFalse(Percentage.IsValid(string.Empty), "string.Empty");
        }
        [Test]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(Percentage.IsValid("%12.00"));
        }
        #endregion
    }

    [Serializable]
    public class PercentageSerializeObject
    {
        public int Id { get; set; }
        public Percentage Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
