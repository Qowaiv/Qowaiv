using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.Globalization
{
    public class CountryTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Country TestStruct = Country.VA;

        /// <summary>Country.Empty should be equal to the default of Country.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(Country), Country.Empty);
        }

        [Test]
        public void All_None_259Countries()
        {
            Assert.AreEqual(259, Country.All.Count);
        }

        [Test]
        public void GetExisting_Januari2016_249Countries()
        {
            Assert.AreEqual(249, Country.GetExisting(new Date(2016, 01, 01)).Count());
        }


        #region Current

        [Test]
        public void Current_CurrentCultureDeDE_Germany()
        {
            using (new CultureInfoScope("de-DE"))
            {
                var act = Country.Current;
                var exp = Country.DE;

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Current_CurrentCultureEsEC_Ecuador()
        {
            using (new CultureInfoScope("es-EC"))
            {
                var act = Country.Current;
                var exp = Country.EC;

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Current_CurrentCultureEn_Empty()
        {
            using (new CultureInfoScope("en"))
            {
                var act = Country.Current;
                var exp = Country.Empty;

                Assert.AreEqual(exp, act);
            }
        }

        #endregion

        #region Country IsEmpty tests

        /// <summary>Country.IsEmpty() should true for the default of Country.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(Country).IsEmpty());
        }

        /// <summary>Country.IsEmpty() should false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_Default_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TyrParse_Null_IsValid()
        {
            Country val;

            string str = null;

            Assert.IsTrue(Country.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TyrParse_StringEmpty_IsValid()
        {
            Country val;

            string str = string.Empty;

            Assert.IsTrue(Country.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse "?" should be valid and the result should be Country.Unknown.</summary>
        [Test]
        public void TyrParse_Questionmark_IsValid()
        {
            Country val;

            string str = "?";

            Assert.IsTrue(Country.TryParse(str, out val), "Valid");
            Assert.IsTrue(val.IsUnknown(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_NullCultureStringValue_IsValid()
        {
            Country val;

            string str = "VA";

            Assert.IsTrue(Country.TryParse(str, null, out val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TyrParse_StringValue_IsValid()
        {
            Country val;

            string str = "VA";

            Assert.IsTrue(Country.TryParse(str, out val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TyrParse_StringValue_IsNotValid()
        {
            Country val;

            string str = "string";

            Assert.IsFalse(Country.TryParse(str, out val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (new CultureInfoScope("en-GB"))
            {
                Assert.Catch<FormatException>
                (() =>
                {
                    Country.Parse("InvalidInput");
                },
                "Not a valid country");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = Country.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(Country);
                var act = Country.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        #endregion

        #region Create tests

        [Test]
        public void Create_RegionInfoNull_Empty()
        {
            var exp = Country.Empty;
            var act = Country.Create((RegionInfo)null);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Create_CultureInfoNull_Empty()
        {
            var exp = Country.Empty;
            var act = Country.Create((CultureInfo)null);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Create_CS_CSXX()
        {
            var cs = new RegionInfo("CS");
            var exp = Country.CSXX;
            var act = Country.Create(cs);

            // We want to be sure that this country is still avialable in .NET.
            Assert.AreEqual("Serbia and Montenegro (Former)", cs.DisplayName, "cs.DisplayName");
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Create_NL_NL()
        {
            var exp = Country.NL;
            var act = Country.Create(new RegionInfo("NL"));
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Create_CultureInfoInvariant_Empty()
        {
            var exp = Country.Empty;
            var act = Country.Create(CultureInfo.InvariantCulture);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Create_CultureInfoEs_Empty()
        {
            var exp = Country.Empty;
            var act = Country.Create(new CultureInfo("es"));
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Create_CultureInfoEsEC_Empty()
        {
            var exp = Country.EC;
            var act = Country.Create(new CultureInfo("es-EC"));
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
                SerializationTest.DeserializeUsingConstructor<Country>(null, default(StreamingContext));
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(Country), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<Country>(info, default(StreamingContext));
            });
        }

        [Test]
        public void GetObjectData_Null_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                ISerializable obj = TestStruct;
                obj.GetObjectData(null, default(StreamingContext));
            },
            "info");
        }

        [Test]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = TestStruct;
            var info = new SerializationInfo(typeof(Country), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default(StreamingContext));

            Assert.AreEqual("VA", info.GetString("Value"));
        }

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = CountryTest.TestStruct;
            var exp = CountryTest.TestStruct;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = CountryTest.TestStruct;
            var exp = CountryTest.TestStruct;
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void XmlSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = CountryTest.TestStruct;
            var exp = CountryTest.TestStruct;
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void SerializeDeserialize_CountrySerializeObject_AreEqual()
        {
            var input = new CountrySerializeObject()
            {
                Id = 17,
                Obj = CountryTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CountrySerializeObject()
            {
                Id = 17,
                Obj = CountryTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date"); ;
        }
        [Test]
        public void XmlSerializeDeserialize_CountrySerializeObject_AreEqual()
        {
            var input = new CountrySerializeObject()
            {
                Id = 17,
                Obj = CountryTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CountrySerializeObject()
            {
                Id = 17,
                Obj = CountryTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date"); ;
        }
        [Test]
        public void DataContractSerializeDeserialize_CountrySerializeObject_AreEqual()
        {
            var input = new CountrySerializeObject()
            {
                Id = 17,
                Obj = CountryTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CountrySerializeObject()
            {
                Id = 17,
                Obj = CountryTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date"); ;
        }

        [Test]
        public void SerializeDeserialize_Empty_AreEqual()
        {
            var input = new CountrySerializeObject()
            {
                Id = 17,
                Obj = Country.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CountrySerializeObject()
            {
                Id = 17,
                Obj = Country.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date"); ;
        }
        [Test]
        public void XmlSerializeDeserialize_Empty_AreEqual()
        {
            var input = new CountrySerializeObject()
            {
                Id = 17,
                Obj = Country.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new CountrySerializeObject()
            {
                Id = 17,
                Obj = Country.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date"); ;
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
        public void FromJson_None_EmptyValue()
        {
            var act = JsonTester.Read<Country>();
            var exp = Country.Empty;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            Assert.Catch<FormatException>(() =>
            {
                JsonTester.Read<Country>("not a country");
            },
            "Not a valid country");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<Country>(TestStruct.ToString(CultureInfo.InvariantCulture));
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_Int64Value_AreEqual()
        {
            var act = JsonTester.Read<Country>(TestStruct.IsoNumericCode);
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_DoubleValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<Country>(1234.56);
            },
            "JSON deserialization from a number is not supported.");
        }

        [Test]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<Country>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [Test]
        public void ToJson_DefaultValue_AreEqual()
        {
            object act = JsonTester.Write(default(Country));
            object exp = null;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = TestStruct.ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void DebuggerDisplay_DebugToString_HasAttribute()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(Country));
        }
        [Test]
        public void DebuggerDisplay_DefaultValue_String()
        {
            DebuggerDisplayAssert.HasResult("Country: (empty)", default(Country));
        }
        [Test]
        public void DebuggerDisplay_Unknown_String()
        {
            DebuggerDisplayAssert.HasResult("Country: (unknown)", Country.Unknown);
        }
        [Test]
        public void DebuggerDisplay_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("Country: Holy See (VA/VAT)", TestStruct);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'VA', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_Empty_IsStringEmpty()
        {
            var act = Country.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_Unknown_Questionmark()
        {
            var act = Country.Unknown.ToString();
            var exp = "?";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString2_NZ_AreEqual()
        {
            var exp = "MZ";
            var act = Country.MZ.ToString("2");
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString3_MZ_AreEqual()
        {
            var exp = "MOZ";
            var act = Country.MZ.ToString("3", new CultureInfo("ja-JP"));
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void ToString0_MZ_AreEqual()
        {
            var exp = "508";
            var act = Country.MZ.ToString("0", new CultureInfo("ja-JP"));
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToStringN_CSHH_AreEqual()
        {
            var exp = "CSHH";
            var act = Country.CSHH.ToString("n", new CultureInfo("ja-JP"));
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToStringE_MZ_AreEqual()
        {
            var exp = "Mozambique";
            var act = Country.MZ.ToString("e", new CultureInfo("ja-JP"));
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToStringF_MZ_AreEqual()
        {
            var exp = "モザンビーク";
            var act = Country.MZ.ToString("f", new CultureInfo("ja-JP"));
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for Country.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, Country.Empty.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [Test]
        public void GetHash_TestStruct_NotZero()
        {
            Assert.NotZero(TestStruct.GetHashCode());
        }

        [Test]
        public void Equals_EmptyEmpty_IsTrue()
        {
            Assert.IsTrue(Country.Empty.Equals(Country.Empty));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            using (new CultureInfoScope("nl-NL"))
            {
                var l = Country.Parse("België");
                var r = Country.Parse("belgie");

                Assert.IsTrue(l.Equals(r));
            }
        }

        [Test]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(CountryTest.TestStruct.Equals(CountryTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(CountryTest.TestStruct.Equals(Country.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(Country.Empty.Equals(CountryTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(CountryTest.TestStruct.Equals((object)CountryTest.TestStruct));
        }

        [Test]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(CountryTest.TestStruct.Equals(null));
        }

        [Test]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(CountryTest.TestStruct.Equals(new object()));
        }

        [Test]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = CountryTest.TestStruct;
            var r = CountryTest.TestStruct;
            Assert.IsTrue(l == r);
        }

        [Test]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = CountryTest.TestStruct;
            var r = CountryTest.TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of Countrys ascending.</summary>
        [Test]
        public void OrderBy_Country_AreEqual()
        {
            var item0 = Country.AE;
            var item1 = Country.BE;
            var item2 = Country.CU;
            var item3 = Country.DO;

            var inp = new List<Country>() { Country.Empty, item3, item2, item0, item1, Country.Empty };
            var exp = new List<Country>() { Country.Empty, Country.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of Countrys descending.</summary>
        [Test]
        public void OrderByDescending_Country_AreEqual()
        {
            var item0 = Country.AE;
            var item1 = Country.BE;
            var item2 = Country.CU;
            var item3 = Country.DO;

            var inp = new List<Country>() { Country.Empty, item3, item2, item0, item1, Country.Empty };
            var exp = new List<Country>() { item3, item2, item1, item0, Country.Empty, Country.Empty };
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

        /// <summary>Compare with null should throw an exception.</summary>
        [Test]
        public void CompareTo_null_ThrowsArgumentException()
        {
            ExceptionAssert.CatchArgumentException
            (() =>
                {
                    object other = null;
                    var act = TestStruct.CompareTo(other);
                },
                "obj",
                "Argument must be a Country"
            );
        }
        /// <summary>Compare with a random object should throw an exception.</summary>
        [Test]
        public void CompareTo_newObject_ThrowsArgumentException()
        {
            ExceptionAssert.CatchArgumentException
            (() =>
                {
                    object other = new object();
                    var act = TestStruct.CompareTo(other);
                },
                "obj",
                "Argument must be a Country"
            );
        }
        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToCountry_AreEqual()
        {
            var exp = TestStruct;
            var act = (Country)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_CountryToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Implicit_RegionInfoToCountry_AreEqual()
        {
            Country exp = Country.NL;
            Country act = new RegionInfo("NL");

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_CountryToRegionInfo_AreEqual()
        {
            var exp = new RegionInfo("NL");
            var act = (RegionInfo)Country.NL;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Properties

        [Test]
        public void CallingCode_Empty_AreEqual()
        {
            var exp = "";
            var act = Country.Empty.CallingCode;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void CallingCode_Unknown_AreEqual()
        {
            var exp = "";
            var act = Country.Unknown.CallingCode;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void CallingCode_TestStruct_AreEqual()
        {
            var exp = "+379";
            var act = TestStruct.CallingCode;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Name_Empty_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = "";
                var act = Country.Empty.Name;
                Assert.AreEqual(exp, act);
            }
        }
        [Test]
        public void Name_Unknown_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = "?";
                var act = Country.Unknown.Name;
                Assert.AreEqual(exp, act);
            }
        }
        [Test]
        public void Name_TestStruct_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = "VA";
                var act = TestStruct.Name;
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void DisplayName_Empty_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = "";
                var act = Country.Empty.DisplayName;
                Assert.AreEqual(exp, act);
            }
        }
        [Test]
        public void DisplayName_Unknown_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = "Unknown";
                var act = Country.Unknown.DisplayName;
                Assert.AreEqual(exp, act);
            }
        }
        [Test]
        public void DisplayName_TestStruct_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = "Holy See";
                var act = TestStruct.DisplayName;
                Assert.AreEqual(exp, act);
            }
        }
        [Test]
        public void GetDisplayName_TestStruct_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = "Holy See";
                var act = TestStruct.GetDisplayName(null);
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void IsoNumericCode_Empty_AreEqual()
        {
            var exp = 0;
            var act = Country.Empty.IsoNumericCode;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void IsoNumericCode_Unknown_AreEqual()
        {
            var exp = 999;
            var act = Country.Unknown.IsoNumericCode;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void IsoNumericCode_TestStruct_AreEqual()
        {
            var exp = 336;
            var act = TestStruct.IsoNumericCode;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void StartDate_Empty_AreEqual()
        {
            var exp = Date.MinValue;
            var act = Country.Empty.StartDate;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void StartDate_Unknown_AreEqual()
        {
            var exp = Date.MinValue;
            var act = Country.Unknown.StartDate;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void StartDate_TestStruct_AreEqual()
        {
            var exp = new Date(1974, 01, 01);
            var act = TestStruct.StartDate;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void StartDate_CZ_AreEqual()
        {
            var exp = new Date(1993, 01, 01);
            var act = Country.CZ.StartDate;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void EndDate_Empty_AreEqual()
        {
            DateTime? exp = null;
            var act = Country.Empty.EndDate;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void EndDate_Unknown_AreEqual()
        {
            DateTime? exp = null;
            var act = Country.Unknown.EndDate;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void EndDate_TestStruct_AreEqual()
        {
            DateTime? exp = null;
            var act = TestStruct.EndDate;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void EndDate_CZ_AreEqual()
        {
            DateTime? exp = null;
            var act = Country.CZ.EndDate;
            Assert.AreEqual(exp, act);
        }
        [Test]
        public void EndDate_CSHH_AreEqual()
        {
            var exp = new Date(1992, 12, 31);
            var act = Country.CSHH.EndDate;
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Methods

        [Test]
        public void IsEmptyOrNotKnown_Empty_IsTrue()
        {
            Assert.IsTrue(Country.Empty.IsEmptyOrUnknown());
        }

        [Test]
        public void IsEmptyOrNotKnown_NotKnown_IsTrue()
        {
            Assert.IsTrue(Country.Unknown.IsEmptyOrUnknown());
        }

        [Test]
        public void IsEmptyOrNotKnown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmptyOrUnknown());
        }

        [Test]
        public void ExistsOnDate_SerbiaAndMontenegro1992_IsFalse()
        {
            Assert.IsFalse(Country.CSXX.ExistsOnDate(new Date(1992, 12, 31)));
        }
        [Test]
        public void ExistsOnDate_SerbiaAndMontenegro1993_IsTrue()
        {
            Assert.IsTrue(Country.CSXX.ExistsOnDate(new Date(1993, 01, 01)));
        }
        [Test]
        public void ExistsOnDate_SerbiaAndMontenegro2012_IsFalse()
        {
            Assert.IsFalse(Country.CSXX.ExistsOnDate(new Date(2012, 01, 01)));
        }

        [Test]
        public void GetCurrency_NL2001_NLG()
        {
            var act = Country.NL.GetCurrency(new Date(2001, 12, 31));
            var exp = Currency.NLG;

            Assert.AreEqual(act, exp);
        }

        [Test]
        public void GetCurrency_NLToday_EUR()
        {
            var act = Country.NL.GetCurrency(Date.Today);
            var exp = Currency.EUR;

            Assert.AreEqual(act, exp);
        }

        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_Country_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(Country));
        }

        [Test]
        public void CanNotConvertFromInt32_Country_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(Country), typeof(Int32));
        }
        [Test]
        public void CanNotConvertToInt32_Country_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(Country), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_Country_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(Country));
        }

        [Test]
        public void CanConvertToString_Country_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(Country));
        }

        [Test]
        public void ConvertFrom_StringNull_CountryEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(Country.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_CountryEmpty()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(Country.Empty, string.Empty);
            }
        }

        [Test]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(CountryTest.TestStruct, CountryTest.TestStruct.ToString());
            }
        }

        [Test]
        public void ConvertFromInstanceDescriptor_Country_Successful()
        {
            TypeConverterAssert.ConvertFromInstanceDescriptor(typeof(Country));
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertToStringEquals(CountryTest.TestStruct.ToString(), CountryTest.TestStruct);
            }
        }

        #endregion

        #region IsValid tests

        [Test]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(Country.IsValid("Complex"), "Complex");
            Assert.IsFalse(Country.IsValid((String)null), "(String)null");
            Assert.IsFalse(Country.IsValid(string.Empty), "string.Empty");
        }
        [Test]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(Country.IsValid("China", null));
        }
        #endregion

        #region Collection tests

        [Test]
        public void GetCurrent_1973_0()
        {
            var exp = 0;

            // before the ISO standard was introduced.
            var act = Country.GetExisting(new Date(1973, 12, 31)).ToList();

            Assert.AreEqual(exp, act.Count);
        }

        [Test]
        public void GetCurrent_None_249()
        {
            var exp = 249;
            var act = Country.GetExisting().ToList();

            Assert.AreEqual(exp, act.Count);
        }

        #endregion
    }

    [Serializable]
    public class CountrySerializeObject
    {
        public int Id { get; set; }
        public Country Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
