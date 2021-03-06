﻿using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests
{
    /// <summary>Tests the email address SVO.</summary>
    public class EmailAddressTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly EmailAddress TestStruct = EmailAddress.Parse("svo@qowaiv.org");

        #region email address const tests

        /// <summary>EmailAddress.Empty should be equal to the default of email address.</summary>
        [Test]
        public void Empty_None_EqualsDefault()
        {
            Assert.AreEqual(default(EmailAddress), EmailAddress.Empty);
        }

        #endregion

        #region email address IsEmpty tests

        /// <summary>EmailAddress.IsEmpty() should be true for the default of email address.</summary>
        [Test]
        public void IsEmpty_Default_IsTrue()
        {
            Assert.IsTrue(default(EmailAddress).IsEmpty());
        }
        /// <summary>EmailAddress.IsEmpty() should be false for EmailAddress.Unknown.</summary>
        [Test]
        public void IsEmpty_Unknown_IsFalse()
        {
            Assert.IsFalse(EmailAddress.Unknown.IsEmpty());
        }
        /// <summary>EmailAddress.IsEmpty() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmpty_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmpty());
        }

        /// <summary>EmailAddress.IsUnknown() should be false for the default of email address.</summary>
        [Test]
        public void IsUnknown_Default_IsFalse()
        {
            Assert.IsFalse(default(EmailAddress).IsUnknown());
        }
        /// <summary>EmailAddress.IsUnknown() should be true for EmailAddress.Unknown.</summary>
        [Test]
        public void IsUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(EmailAddress.Unknown.IsUnknown());
        }
        /// <summary>EmailAddress.IsUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsUnknown());
        }

        /// <summary>EmailAddress.IsEmptyOrUnknown() should be true for the default of email address.</summary>
        [Test]
        public void IsEmptyOrUnknown_Default_IsFalse()
        {
            Assert.IsTrue(default(EmailAddress).IsEmptyOrUnknown());
        }
        /// <summary>EmailAddress.IsEmptyOrUnknown() should be true for EmailAddress.Unknown.</summary>
        [Test]
        public void IsEmptyOrUnknown_Unknown_IsTrue()
        {
            Assert.IsTrue(EmailAddress.Unknown.IsEmptyOrUnknown());
        }
        /// <summary>EmailAddress.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
        [Test]
        public void IsEmptyOrUnknown_TestStruct_IsFalse()
        {
            Assert.IsFalse(TestStruct.IsEmptyOrUnknown());
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [Test]
        public void TryParse_Null_IsValid()
        {
            string str = null;

            Assert.IsTrue(EmailAddress.TryParse(str, out EmailAddress val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [Test]
        public void TryParse_StringEmpty_IsValid()
        {
            string str = string.Empty;

            Assert.IsTrue(EmailAddress.TryParse(str, out EmailAddress val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TryParse_StringValue_IsValid()
        {
            string str = "svo@qowaiv.org";

            Assert.IsTrue(EmailAddress.TryParse(str, out EmailAddress val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [Test]
        public void TryParse_StringValue_IsNotValid()
        {
            string str = "string";

            Assert.IsFalse(EmailAddress.TryParse(str, out EmailAddress val), "Valid");
            Assert.AreEqual(string.Empty, val.ToString(), "Value");
        }

        [Test]
        public void Parse_DomainPart_ShouldBeLowerCased()
        {
            var email = EmailAddress.Parse("mail@UPPERCASE.com");
            Assert.AreEqual("mail@uppercase.com", email.ToString());
        }

        [Test]
        public void Parse_LocalPart_ShouldNotBeLowerCased()
        {
            var email = EmailAddress.Parse("MAIL@lowercase.com");
            Assert.AreEqual("MAIL@lowercase.com", email.ToString());
        }

        [Test]
        public void Parse_QuotedLocalPart_ShouldNotBeStripped()
        {
            var email = EmailAddress.Parse(@"""Joe Smith"" ""Literal (c)""@domain.com");
            Assert.AreEqual(@"""Literal (c)""@domain.com", email.ToString());
        }

        [Test]
        public void Parse_Unknown_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var act = EmailAddress.Parse("?");
                var exp = EmailAddress.Unknown;
                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void Parse_WithDisplayName_WithoutDisplayName()
        {
            var email = EmailAddress.Parse("Joe Smith <email@domain.com>");
            Assert.AreEqual("email@domain.com", email.ToString());
        }

        [Test]
        public void Parse_WithQuotedDisplayName_WithoutDisplayName()
        {
            var email = EmailAddress.Parse("\"Joe Smith\" email@domain.com");
            Assert.AreEqual("email@domain.com", email.ToString());
        }

        [Test]
        public void Parse_WithCommentAtTheEnd_WithoutComment()
        {
            var email = EmailAddress.Parse("email@domain.com (Joe Smith)");
            Assert.AreEqual("email@domain.com", email.ToString());
        }

        [Test]
        public void Parse_WithComment_WithoutComment()
        {
            var email = EmailAddress.Parse("email@(stupid but true)domain.com");
            Assert.AreEqual("email@domain.com", email.ToString());
        }

        [Test]
        public void Parse_WithoutBrackets_WithBrackets()
        {
            var email = EmailAddress.Parse("home@127.0.0.1");
            Assert.AreEqual("home@[127.0.0.1]", email.ToString());
        }

        [Test]
        public void Parse_WithBrackets_WithBrackets()
        {
            var email = EmailAddress.Parse("home@[127.0.0.1]");
            Assert.AreEqual("home@[127.0.0.1]", email.ToString());
        }

        [Test]
        public void Parse_WithoutIPv6Prefix_WithPrefix()
        {
            var email = EmailAddress.Parse("home@2607:f0d0:1002:51::4");
            Assert.AreEqual("home@[IPv6:2607:f0d0:1002:51::4]", email.ToString());
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.Catch<FormatException>
                (() =>
                {
                    EmailAddress.Parse("InvalidInput");
                },
                "Not a valid email address");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = TestStruct;
                var act = EmailAddress.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [Test]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = default(EmailAddress);
                var act = EmailAddress.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        #endregion

        #region (XML) (De)serialization tests

        [Test]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<EmailAddress>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(EmailAddress), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<EmailAddress>(info, default);
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
            var info = new SerializationInfo(typeof(EmailAddress), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual("svo@qowaiv.org", info.GetString("Value"));
        }

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = TestStruct;
            var exp = TestStruct;
            var act = SerializationTest.BinaryFormatterSerializeDeserialize(input);
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
            var exp = "svo@qowaiv.org";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = SerializationTest.XmlDeserialize<EmailAddress>("svo@qowaiv.org");
            Assert.AreEqual(TestStruct, act);
        }


        [Test]
        public void SerializeDeserialize_EmailAddressSerializeObject_AreEqual()
        {
            var input = new EmailAddressSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EmailAddressSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.BinaryFormatterSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_EmailAddressSerializeObject_AreEqual()
        {
            var input = new EmailAddressSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EmailAddressSerializeObject
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
        public void DataContractSerializeDeserialize_EmailAddressSerializeObject_AreEqual()
        {
            var input = new EmailAddressSerializeObject
            {
                Id = 17,
                Obj = TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EmailAddressSerializeObject
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
        public void SerializeDeserialize_Empty_AreEqual()
        {
            var input = new EmailAddressSerializeObject
            {
                Id = 17,
                Obj = EmailAddress.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EmailAddressSerializeObject
            {
                Id = 17,
                Obj = EmailAddress.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.BinaryFormatterSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_Empty_AreEqual()
        {
            var input = new EmailAddressSerializeObject
            {
                Id = 17,
                Obj = EmailAddress.Empty,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EmailAddressSerializeObject
            {
                Id = 17,
                Obj = EmailAddress.Empty,
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
                JsonTester.Read<EmailAddress>("InvalidStringValue");
            },
            "Not a valid email address");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<EmailAddress>("svo@qowaiv.org");
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToJson_DefaultValue_IsNull()
        {
            object act = JsonTester.Write(default(EmailAddress));
            Assert.IsNull(act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = "svo@qowaiv.org";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void ToString_Empty_IsStringEmpty()
        {
            var act = EmailAddress.Empty.ToString();
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("U", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'SVO@QOWAIV.ORG', format: 'U'";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_EmptyFormatF_AreEqual()
        {
            var act = EmailAddress.Empty.ToString(@"f");
            var exp = "";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_TestStructFormatMailtoF_AreEqual()
        {
            var act = TestStruct.ToString(@"mai\lto:f");
            var exp = "mailto:svo@qowaiv.org";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_TestStructFormatU_AreEqual()
        {
            var act = TestStruct.ToString("U");
            var exp = "SVO@QOWAIV.ORG";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_TestStructFormatLLowerAtd_ComplexPatternAreEqual()
        {
            var act = TestStruct.ToString("l[at]d");
            var exp = "svo[at]qowaiv.org";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_TestStructFormatUpperLAtD_ComplexPatternAreEqual()
        {
            var act = TestStruct.ToString("L[at]D");
            var exp = "SVO[at]QOWAIV.ORG";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for EmailAddress.Empty.</summary>
        [Test]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, EmailAddress.Empty.GetHashCode());
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
            Assert.IsTrue(EmailAddress.Empty.Equals(EmailAddress.Empty));
        }

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = EmailAddress.Parse("svo@qowaiv.org", CultureInfo.InvariantCulture);
            var r = EmailAddress.Parse("With display <svo@Qowaiv.org>", CultureInfo.InvariantCulture);

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
            Assert.IsFalse(TestStruct.Equals(EmailAddress.Empty));
        }

        [Test]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(EmailAddress.Empty.Equals(TestStruct));
        }

        [Test]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(TestStruct.Equals((object)TestStruct));
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

        /// <summary>Orders a list of email addresss ascending.</summary>
        [Test]
        public void OrderBy_EmailAddress_AreEqual()
        {
            var item0 = EmailAddress.Parse("a@qowaiv.org");
            var item1 = EmailAddress.Parse("b@qowaiv.org");
            var item2 = EmailAddress.Parse("c@qowaiv.org");
            var item3 = EmailAddress.Parse("d@qowaiv.org");

            var inp = new List<EmailAddress> { EmailAddress.Empty, item3, item2, item0, item1, EmailAddress.Empty };
            var exp = new List<EmailAddress> { EmailAddress.Empty, EmailAddress.Empty, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of email addresss descending.</summary>
        [Test]
        public void OrderByDescending_EmailAddress_AreEqual()
        {
            var item0 = EmailAddress.Parse("a@qowaiv.org");
            var item1 = EmailAddress.Parse("b@qowaiv.org");
            var item2 = EmailAddress.Parse("c@qowaiv.org");
            var item3 = EmailAddress.Parse("d@qowaiv.org");

            var inp = new List<EmailAddress> { EmailAddress.Empty, item3, item2, item0, item1, EmailAddress.Empty };
            var exp = new List<EmailAddress> { item3, item2, item1, item0, EmailAddress.Empty, EmailAddress.Empty };
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
                "Argument must be EmailAddress."
            );
        }
        #endregion

        #region Casting tests

        [Test]
        public void Explicit_StringToEmailAddress_AreEqual()
        {
            var exp = TestStruct;
            var act = (EmailAddress)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Explicit_EmailAddressToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Properties

        [Test]
        public void Length_DefaultValue_0()
        {
            var exp = 0;
            var act = EmailAddress.Empty.Length;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Length_Unknown_0()
        {
            var exp = 0;
            var act = EmailAddress.Unknown.Length;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Length_TestStruct_IntValue()
        {
            var exp = 14;
            var act = TestStruct.Length;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Local_Empty_StringEmpty()
        {
            var email = EmailAddress.Empty;
            Assert.AreEqual(string.Empty, email.Local);
        }

        [Test]
        public void Local_Unknown_StringEmpty()
        {
            var email = EmailAddress.Unknown;
            Assert.AreEqual(string.Empty, email.Local);
        }

        [Test]
        public void Local_TestStruct_Info()
        {
            var exp = "svo";
            var act = TestStruct.Local;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Domain_Empty_StringEmpty()
        {
            var email = EmailAddress.Empty;
            Assert.AreEqual(string.Empty, email.Domain);
        }

        [Test]
        public void Domain_Unknown_StringEmpty()
        {
            var email = EmailAddress.Unknown;
            Assert.AreEqual(string.Empty, email.Domain);
        }

        [Test]
        public void Domain_TestStruct_QowaivOrg()
        {
            var exp = "qowaiv.org";
            var act = TestStruct.Domain;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void IsIPBased_Empty_False()
        {
            Assert.IsFalse(EmailAddress.Empty.IsIPBased);
        }

        [Test]
        public void IPDomain_TestStruct_None()
        {
            var exp = IPAddress.None;
            var act = TestStruct.IPDomain;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void IPDomain_IPBasedEmail_SomeIPAddress()
        {
            var email = EmailAddress.Parse("qowaiv@172.16.254.1");
            var exp = IPAddress.Parse("172.16.254.1");
            var act = email.IPDomain;
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void IPDomain_IPv6BasedEmail_SomeIPV6ddress()
        {
            var email = EmailAddress.Parse("qowaiv@[IPv6:0::1]");
            var exp = IPAddress.Parse("0::1");
            var act = email.IPDomain;
            Assert.AreEqual(exp, act);
        }


        #endregion

        #region Methods

        [Test]
        public void WithDisplayName_UnknownEmailAddress_Throws()
        {
            var x = Assert.Catch<InvalidOperationException>(() => EmailAddress.Unknown.WithDisplayName("Jimi Hendrix"));
            Assert.AreEqual("An not set email address can not be shown with a display name.", x.Message);
        }

        [Test]
        public void WithDisplayName_EmptyString_EmailAddressOnly()
        {
            var str = TestStruct.WithDisplayName(string.Empty);
            Assert.AreEqual("svo@qowaiv.org", str);
        }

        [Test]
        public void WithDisplayName_JimiHendrix_WithDisplayName()
        {
            var str = TestStruct.WithDisplayName(" Jimi Hendrix  ");
            Assert.AreEqual("Jimi Hendrix <svo@qowaiv.org>", str);
        }

        #endregion

        #region Type converter tests

        [Test]
        public void ConverterExists_EmailAddress_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(EmailAddress));
        }

        [Test]
        public void CanNotConvertFromInt32_EmailAddress_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(EmailAddress), typeof(Int32));
        }
        [Test]
        public void CanNotConvertToInt32_EmailAddress_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(EmailAddress), typeof(Int32));
        }

        [Test]
        public void CanConvertFromString_EmailAddress_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(EmailAddress));
        }

        [Test]
        public void CanConvertToString_EmailAddress_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(EmailAddress));
        }

        [Test]
        public void ConvertFrom_StringNull_EmailAddressEmpty()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(EmailAddress.Empty, (string)null);
            }
        }

        [Test]
        public void ConvertFrom_StringEmpty_EmailAddressEmpty()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(EmailAddress.Empty, string.Empty);
            }
        }

        [Test]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(TestStruct, TestStruct.ToString());
            }
        }

        [Test]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertToStringEquals(TestStruct.ToString(), TestStruct);
            }
        }

        #endregion
    }

    [Serializable]
    public class EmailAddressSerializeObject
    {
        public int Id { get; set; }
        public EmailAddress Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
