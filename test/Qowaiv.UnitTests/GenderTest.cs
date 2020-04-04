using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests
{
    public class GenderTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Gender TestStruct = Gender.Male;

        #region Gender const tests

        /// <summary>Gender.Empty should be equal to the default of Gender.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(Gender), Gender.Empty);
        }

        #endregion

        #region Gender IsEmpty tests

        /// <summary>Gender.IsEmpty() should true for the default of Gender.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(Gender).IsEmpty());
        }

        /// <summary>Gender.IsEmpty() should false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_Default_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        #endregion

        #region TryParse tests


        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (new CultureInfoScope(TestCultures.En_GB))
            {
                Assert.Catch<FormatException>
                (() =>
                {
                    Gender.Parse("InvalidInput");
                },
                "Not a valid gender");
            }
        }

        [Test]
        public void Parse_NotKnown_ThrowsFormatException()
        {
            using (new CultureInfoScope(TestCultures.En_GB))
            {
                var exp = Gender.Unknown;
                var act = Gender.Parse("Not known");
                Assert.AreEqual(exp, act);
            }
        }

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TyrParse_Null_IsValid()
        {
            string str = null;
            Assert.IsTrue(Gender.TryParse(str, out Gender val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            string str = string.Empty;
            Assert.IsTrue(Gender.TryParse(str, out Gender val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            using (new CultureInfoScope("it-IT"))
            {
                string str = "Maschio";
                Assert.IsTrue(Gender.TryParse(str, out Gender val), "Valid");
                Assert.AreEqual(str, val.ToString(), "Value");
            }
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_M_IsValid()
        {
            string str = "M";
            Assert.IsTrue(Gender.TryParse(str, null, out Gender val), "Valid");
            Assert.AreEqual(TestStruct, val, "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            string str = "string";
            Assert.IsFalse(Gender.TryParse(str, out Gender val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope(TestCultures.En_GB))
            {
                var exp = TestStruct;
                var act = Gender.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope(TestCultures.En_GB))
            {
                var exp = default(Gender);
                var act = Gender.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        #endregion

        #region TryCreate tests

        [Test]
        public void Create_Nullable17_ThrowsArgumentOutOfRangeException()
        {
            using (new CultureInfoScope(TestCultures.En_GB))
            {
                ExceptionAssert.CatchArgumentOutOfRangeException(() =>
                {
                    Gender.Create((Int32?)17);
                },
                "val",
                "Not a valid gender");
            }
        }
        [Test]
        public void Create_17_ThrowsArgumentOutOfRangeException()
        {
            using (new CultureInfoScope(TestCultures.En_GB))
            {
                ExceptionAssert.CatchArgumentOutOfRangeException(() =>
                {
                    Gender.Create(17);
                },
                "val",
                "Not a valid gender");
            }
        }

        [Test]
        public void TryCreate_Null_IsEmpty()
        {
            Gender exp = Gender.Empty;
            Assert.IsTrue(Gender.TryCreate(null, out Gender act));
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void TryCreate_Int32MinValue_IsEmpty()
        {
            Gender exp = Gender.Empty;
            Assert.IsFalse(Gender.TryCreate(int.MinValue, out Gender act));
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void TryCreate_Int32MinValue_AreEqual()
        {
            var exp = Gender.Empty;
            var act = Gender.TryCreate(Int32.MinValue);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void TryCreate_0_AreEqual()
        {
            var exp = Gender.Unknown;
            var act = Gender.TryCreate(0);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void TryCreate_1_AreEqual()
        {
            var exp = TestStruct;
            var act = Gender.TryCreate(1);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Create_Int32MinValue_ThrowsArgumentOutOfRangeException()
        {
            ExceptionAssert.CatchArgumentOutOfRangeException
            (() =>
            {
                Gender.Create(Int32.MinValue);
            },
            "val",
            "Not a valid gender");
        }

        #endregion

        #region (XML) (De)serialization tests

        [Test]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<Gender>(null, default);
            },
            "info");
        }
        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(Gender), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<Gender>(info, default);
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
            var info = new SerializationInfo(typeof(Gender), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual((Byte)2, info.GetByte("Value"));
        }

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = GenderTest.TestStruct;
            var exp = GenderTest.TestStruct;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = GenderTest.TestStruct;
            var exp = GenderTest.TestStruct;
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlSerialize_TestStruct_AreEqual()
        {
            var act = SerializationTest.XmlSerialize(TestStruct);
            var exp = "Male";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<Gender>("Male");
            Assert.AreEqual(TestStruct, act);
        }

        [Test]
        public void SerializeDeserialize_GenderSerializeObject_AreEqual()
        {
            var input = new GenderSerializeObject
            {
                Id = 17,
                Obj = GenderTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new GenderSerializeObject
            {
                Id = 17,
                Obj = GenderTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_GenderSerializeObject_AreEqual()
        {
            var input = new GenderSerializeObject
            {
                Id = 17,
                Obj = GenderTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new GenderSerializeObject
            {
                Id = 17,
                Obj = GenderTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void DataContractSerializeDeserialize_GenderSerializeObject_AreEqual()
        {
            var input = new GenderSerializeObject
            {
                Id = 17,
                Obj = GenderTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new GenderSerializeObject
            {
                Id = 17,
                Obj = GenderTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void SerializeDeserialize_Empty_AreEqual()
        {
            var input = new GenderSerializeObject
            {
                Id = 17,
                Obj = Gender.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new GenderSerializeObject
            {
                Id = 17,
                Obj = Gender.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_Empty_AreEqual()
        {
            var input = new GenderSerializeObject
            {
                Id = 17,
                Obj = Gender.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new GenderSerializeObject
            {
                Id = 17,
                Obj = Gender.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
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
                JsonTester.Read<Gender>("InvalidStringValue");
            },
            "Not a valid gender");
        }

        [TestCase("male")]
        [TestCase(1L)]
        [TestCase(1d)]
        public void FromJson_AreEqual(object json)
        {
            var act = JsonTester.Read<Gender>(json);
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToJson_DefaultValue_IsNull()
        {
            object act = JsonTester.Write(default(Gender));
            Assert.IsNull(act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = "Male";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void ToString_Empty_IsStringEmpty()
        {
            var act = Gender.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {

            var act = TestStruct.ToString("i => s", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: '1 => ♂', format: 'i => s'";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_NullFormatProvider_FormattedString()
        {
            using (new CultureInfoScope("fr-FR"))
            {
                var act = TestStruct.ToString("f", null);
                var exp = "Masculin";

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToStringEmptyFormat_Empty_IsStringEmpty()
        {
            var act = Gender.Empty.ToString("");
            var exp = "";
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToStringEmptyFormat_TestStruct_IsStringEmpty()
        {
            using (new CultureInfoScope("nl-NL"))
            {
                var act = TestStruct.ToString("");
                var exp = "Mannelijk";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void ToString_FormatValueEnglishGreatBritain_AreEqual()
        {
            using (new CultureInfoScope(TestCultures.En_GB))
            {
                var act = Gender.Male.ToString(@"\x f \format \\ random");
                var exp = @"\x Male format \ random";
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(Gender));
        }

        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("{empty}", default(Gender));
        }

        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("Male", TestStruct);
        }

        [Test]
        public void ToString_InvariantCulture_AreEqual()
        {
            using (new CultureInfoScope(CultureInfo.InvariantCulture))
            {
                ToStringAssert
                (
                    "Not known", "Male", "Female", "Not applicable",
                    "?", "M", "F", "X",
                    "Mr/Mrs", "Mr", "Mrs", ""
                );
            }
        }

        [Test]
        public void ToString_deCH_AreEqual()
        {
            using (new CultureInfoScope("de-CH"))
            {
                ToStringAssert
                (
                    "Unbekannt", "Männlich", "Weiblich", "Nicht anwendbar",
                    "?", "M", "W", "X",
                    "Herr/Frau", "Herr", "Frau", ""
                );
            }
        }
        [Test]
        public void ToString_enGB_AreEqual()
        {
            using (new CultureInfoScope(TestCultures.En_GB))
            {
                ToStringAssert
                (
                    "Not known", "Male", "Female", "Not applicable",
                    "?", "M", "F", "X",
                    "Mr./Mrs.", "Mr.", "Mrs.", ""
                );
            }
        }
        [Test]
        public void ToString_esEC_AreEqual()
        {
            using (new CultureInfoScope("es-EC"))
            {
                ToStringAssert
                (
                    "Desconocido", "Hombre", "Mujer", "No aplicable",
                    "?", "H", "M", "X",
                    "Sr./Sra.", "Sr.", "Sra.", ""
                );
            }
        }
        [Test]
        public void ToString_frCH_AreEqual()
        {
            using (new CultureInfoScope("fr-CH"))
            {
                ToStringAssert
                (
                    "Inconnu", "Masculin", "Féminin", "Sans objet",
                    "?", "M", "F", "X",
                    "M/Mme", "M", "Mme", ""
                );
            }
        }
        [Test]
        public void ToString_itIT_AreEqual()
        {
            using (new CultureInfoScope("it-IT"))
            {
                ToStringAssert
                (
                    "Sconosciuto", "Maschio", "Femmina", "Non specificato",
                    "?", "M", "F", "X",
                    "Mr/Mrs", "Mr", "Mrs", ""
                );
            }
        }
        [Test]
        public void ToString_nlBE_AreEqual()
        {
            using (new CultureInfoScope("nl-BE"))
            {
                ToStringAssert
                (
                    "Onbekend", "Mannelijk", "Vrouwelijk", "Niet van toepassing",
                    "?", "M", "V", "X",
                    "Dhr./Mevr.", "Dhr.", "Mevr.", ""
                );
            }
        }

        private void ToStringAssert(
            string gender0, string gender1, string gender2, string gender9,
            string char0, string char1, string char2, string char9,
            string honorific0, string honorific1, string honorific2, string honorific9)
        {
            Assert.AreEqual(gender0, Gender.Unknown.ToString(""), "Gender.NotKnown.ToString(\"\")");
            Assert.AreEqual(gender1, Gender.Male.ToString(""), "Gender.Male.ToString(\"\")");
            Assert.AreEqual(gender2, Gender.Female.ToString(""), "Gender.Female.ToString(\"\")");
            Assert.AreEqual(gender9, Gender.NotApplicable.ToString(""), "Gender.NotApplicable.ToString(\"\")");

            Assert.AreEqual(char0, Gender.Unknown.ToString("c"), "Gender.NotKnown.ToString(\"c\")");
            Assert.AreEqual(char1, Gender.Male.ToString("c"), "Gender.Male.ToString(\"c\")");
            Assert.AreEqual(char2, Gender.Female.ToString("c"), "Gender.Female.ToString(\"c\")");
            Assert.AreEqual(char9, Gender.NotApplicable.ToString("c"), "Gender.NotApplicable.ToString(\"c\")");

            Assert.AreEqual(honorific0, Gender.Unknown.ToString("h"), "Gender.NotKnown.ToString(\"h\")");
            Assert.AreEqual(honorific1, Gender.Male.ToString("h"), "Gender.Male.ToString(\"h\")");
            Assert.AreEqual(honorific2, Gender.Female.ToString("h"), "Gender.Female.ToString(\"h\")");
            Assert.AreEqual(honorific9, Gender.NotApplicable.ToString("h"), "Gender.NotApplicable.ToString(\"h\")");

            Assert.AreEqual("0", Gender.Unknown.ToString("i"), "Gender.NotKnown.ToString(\"i\")");
            Assert.AreEqual("1", Gender.Male.ToString("i"), "Gender.Male.ToString(\"i\")");
            Assert.AreEqual("2", Gender.Female.ToString("i"), "Gender.Female.ToString(\"i\")");
            Assert.AreEqual("9", Gender.NotApplicable.ToString("i"), "Gender.NotApplicable.ToString(\"i\")");

            Assert.AreEqual("?", Gender.Unknown.ToString("s"), "Gender.NotKnown.ToString(\"s\")");
            Assert.AreEqual("♂", Gender.Male.ToString("s"), "Gender.Male.ToString(\"s\")");
            Assert.AreEqual("♀", Gender.Female.ToString("s"), "Gender.Female.ToString(\"s\")");
            Assert.AreEqual("X", Gender.NotApplicable.ToString("s"), "Gender.NotApplicable.ToString(\"s\")");
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for Gender.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, Gender.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(2, GenderTest.TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(Gender.Empty.Equals(Gender.Empty));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = Gender.Parse("Male", CultureInfo.InvariantCulture);
            var r = Gender.Parse("M", CultureInfo.InvariantCulture);

            Assert.IsTrue(l.Equals(r));
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(GenderTest.TestStruct.Equals(GenderTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(GenderTest.TestStruct.Equals(Gender.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(Gender.Empty.Equals(GenderTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(GenderTest.TestStruct.Equals((object)GenderTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(GenderTest.TestStruct.Equals(null));
        }

        [Test]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(GenderTest.TestStruct.Equals(new object()));
        }

        [Test]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = GenderTest.TestStruct;
            var r = GenderTest.TestStruct;
            Assert.IsTrue(l == r);
        }

        [Test]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = GenderTest.TestStruct;
            var r = GenderTest.TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of Genders ascending.</summary>
        [Test]
        public void OrderBy_Gender_AreEqual()
        {
            var item0 = Gender.Unknown;
            var item1 = Gender.Male;
            var item2 = Gender.Female;
            var item3 = Gender.NotApplicable;

            var inp = new List<Gender> { Gender.Empty, item3, item2, item0, item1, Gender.Empty };
            var exp = new List<Gender> { Gender.Empty, Gender.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of Genders descending.</summary>
        [Test]
        public void OrderByDescending_Gender_AreEqual()
        {
            var item0 = Gender.Unknown;
            var item1 = Gender.Male;
            var item2 = Gender.Female;
            var item3 = Gender.NotApplicable;

            var inp = new List<Gender> { Gender.Empty, item3, item2, item0, item1, Gender.Empty };
            var exp = new List<Gender> { item3, item2, item1, item0, Gender.Empty, Gender.Empty };
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
                "Argument must be Gender."
            );
        }

        [Test]
        public void LessThan_17LT19_IsTrue()
        {
            Gender l = Gender.Unknown;
            Gender r = Gender.Male;

            Assert.IsTrue(l < r);
        }
        [Test]
        public void GreaterThan_21LT19_IsTrue()
        {
            Gender l = Gender.Female;
            Gender r = Gender.Male;

            Assert.IsTrue(l > r);
        }

        [Test]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            Gender l = Gender.Male;
            Gender r = Gender.Female;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            Gender l = Gender.Female;
            Gender r = Gender.Male;

            Assert.IsTrue(l >= r);
        }

        [Test]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            Gender l = Gender.NotApplicable;
            Gender r = Gender.NotApplicable;

            Assert.IsTrue(l <= r);
        }
        [Test]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            Gender l = Gender.NotApplicable;
            Gender r = Gender.NotApplicable;

            Assert.IsTrue(l >= r);
        }
        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToGender_AreEqual()
        {
            var exp = TestStruct;
            var act = (Gender)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_GenderToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }


        [Test]
        public void Explicit_Int32ToGender_AreEqual()
        {
            var exp = TestStruct;
            var act = (Gender)1;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_GenderToInt32_AreEqual()
        {
            var exp = 1;
            var act = (Int32)TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Explicit_GenderToNullableInt32_AreEqual()
        {
            Int32? exp = 1;
            Int32? act = (Int32?)TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Explicit_Int32NotKnown_AreEqual()
        {
            var exp = Gender.Unknown;
            var act = (Gender)0;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Explicit_NullableInt32NotKnown_AreEqual()
        {
            var exp = Gender.Unknown;
            var act = (Gender)((Int32?)0);

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Explicit_NotKnownToInt32_AreEqual()
        {
            Int32 exp = 0;
            Int32 act = (Int32)Gender.Unknown;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Explicit_EmptyToNullableInt32_AreEqual()
        {
            Int32? exp = null;
            Int32? act = (Int32?)Gender.Empty;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Properties

        [Test]
        public void DisplayName_None_AreEqual()
        {
            using (new CultureInfoScope("fr-FR"))
            {
                var exp = "Masculin";
                var act = TestStruct.DisplayName;
                Assert.AreEqual(exp, act);
            }
        }

        #endregion

        #region Methods

        [Test]
        public void IsEmptyOrNotKnown_Empty_IsTrue()
        {
            Assert.IsTrue(Gender.Empty.IsEmptyOrUnknown());
        }

        [Test]
        public void IsEmptyOrNotKnown_NotKnown_IsTrue()
        {
            Assert.IsTrue(Gender.Unknown.IsEmptyOrUnknown());
        }

        [Test]
        public void IsEmptyOrNotKnown_Male_IsFalse()
        {
            Assert.IsFalse(Gender.Male.IsEmptyOrUnknown());
        }

        [Test]
        public void IsMaleOrFemale_Male_IsTrue()
        {
            Assert.IsTrue(Gender.Male.IsMaleOrFemale());
        }
        [Test]
        public void IsMaleOrFemale_Female_IsTrue()
        {
            Assert.IsTrue(Gender.Female.IsMaleOrFemale());
        }
        [Test]
        public void IsMaleOrFemale_NotApplicable_IsFalse()
        {
            Assert.IsFalse(Gender.NotApplicable.IsMaleOrFemale());
        }
        [Test]
        public void IsMaleOrFemale_NotKnown_IsFalse()
        {
            Assert.IsFalse(Gender.Unknown.IsMaleOrFemale());
        }

        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_Gender_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(Gender));
        }

        [Test]
        public void CanNotConvertFromInt32_Gender_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(Gender), typeof(Int32));
        }
        [Test]
        public void CanNotConvertToInt32_Gender_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(Gender), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_Gender_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(Gender));
        }

        [Test]
        public void CanConvertToString_Gender_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(Gender));
        }

        [Test]
        public void ConvertFrom_StringNull_GenderEmpty()
        {
            using (new CultureInfoScope(TestCultures.En_GB))
            {
                TypeConverterAssert.ConvertFromEquals(Gender.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFromString_StringEmpty_GenderEmpty()
        {
            using (new CultureInfoScope(TestCultures.En_GB))
            {
                TypeConverterAssert.ConvertFromEquals(Gender.Empty, string.Empty);
            }
        }

        [Test]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (new CultureInfoScope(TestCultures.En_GB))
            {
                TypeConverterAssert.ConvertFromEquals(TestStruct, "Male");
            }
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (new CultureInfoScope(TestCultures.En_GB))
            {
                TypeConverterAssert.ConvertToStringEquals("Male", TestStruct);
            }
        }

        #endregion

        #region IsValid tests

        [Test]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(Gender.IsValid("Mannetje"), "Complex");
            Assert.IsFalse(Gender.IsValid((String)null), "(String)null");
            Assert.IsFalse(Gender.IsValid(string.Empty), "string.Empty");
        }
        [Test]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(Gender.IsValid("Not known"));
        }
        [Test]
        public void IsValid_Null_IsFalse()
        {
            Assert.IsFalse(Gender.IsValid((String)null));
            Assert.IsFalse(Gender.IsValid((Int32?)null));
        }
        [Test]
        public void IsValid_StringEmpty_IsFalse()
        {
            Assert.IsFalse(Gender.IsValid(string.Empty));
        }

        [Test]
        public void IsValid_Min1_IsFalse()
        {
            Assert.IsFalse(Gender.IsValid(-1));
        }
        [Test]
        public void IsValid_512_IsFalse()
        {
            Assert.IsFalse(Gender.IsValid(512));
        }
        [Test]
        public void IsValid_9_IsTrue()
        {
            Assert.IsTrue(Gender.IsValid(9));
        }

        [Test]
        public void IsValid_MaleNull_IsTrue()
        {
            Assert.IsTrue(Gender.IsValid("Male", null));
        }

        #endregion
    }

    [Serializable]
    public class GenderSerializeObject
    {
        public int Id { get; set; }
        public Gender Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
