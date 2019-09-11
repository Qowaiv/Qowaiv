using NUnit.Framework;
using Qowaiv.TestTools;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests
{
    [TestFixture]
    public class EmailAddressCollectionTest
    {
        public EmailAddressCollection GetTestInstance()
        {
            return EmailAddressCollection.Parse("info@qowaiv.org,test@qowaiv.org");
        }

        #region Constructor

        [Test]
        public void Ctor_NullArray_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException(() =>
            {
                new EmailAddressCollection((EmailAddress[])null);
            },
            "emails");
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
                ISerializable obj = GetTestInstance();
                obj.GetObjectData(null, default);
            },
            "info");
        }

        [Test]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = GetTestInstance();
            var info = new SerializationInfo(typeof(EmailAddressCollection), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual("info@qowaiv.org,test@qowaiv.org", info.GetString("Value"));
        }

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = GetTestInstance();
            var exp = GetTestInstance();
            var act = SerializationTest.SerializeDeserialize(input);
            CollectionAssert.AreEqual(exp, act);
        }
        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = GetTestInstance();
            var exp = GetTestInstance();
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            CollectionAssert.AreEqual(exp, act);
        }
        [Test]
        public void XmlSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = GetTestInstance();
            var exp = GetTestInstance();
            var act = SerializationTest.XmlSerializeDeserialize(input);
            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void SerializeDeserialize_EmailAddressSerializeObject_AreEqual()
        {
            var input = new EmailAddressCollectionSerializeObject()
            {
                Id = 17,
                Obj = GetTestInstance(),
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EmailAddressCollectionSerializeObject()
            {
                Id = 17,
                Obj = GetTestInstance(),
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            CollectionAssert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_EmailAddressSerializeObject_AreEqual()
        {
            var input = new EmailAddressCollectionSerializeObject()
            {
                Id = 17,
                Obj = GetTestInstance(),
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EmailAddressCollectionSerializeObject()
            {
                Id = 17,
                Obj = GetTestInstance(),
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            CollectionAssert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void DataContractSerializeDeserialize_EmailAddressSerializeObject_AreEqual()
        {
            var input = new EmailAddressCollectionSerializeObject()
            {
                Id = 17,
                Obj = GetTestInstance(),
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EmailAddressCollectionSerializeObject()
            {
                Id = 17,
                Obj = GetTestInstance(),
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            CollectionAssert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void SerializeDeserialize_Empty_AreEqual()
        {
            var input = new EmailAddressCollectionSerializeObject()
            {
                Id = 17,
                Obj = new EmailAddressCollection(),
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EmailAddressCollectionSerializeObject()
            {
                Id = 17,
                Obj = new EmailAddressCollection(),
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            CollectionAssert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void XmlSerializeDeserialize_Empty_AreEqual()
        {
            var input = new EmailAddressCollectionSerializeObject()
            {
                Id = 17,
                Obj = new EmailAddressCollection(),
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new EmailAddressCollectionSerializeObject()
            {
                Id = 17,
                Obj = new EmailAddressCollection(),
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            CollectionAssert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [Test]
        public void GetSchema_None_IsNull()
        {
            IXmlSerializable obj = GetTestInstance();
            Assert.IsNull(obj.GetSchema());
        }

        #endregion

        #region JSON (De)serialization tests

        [Test]
        public void FromJson_None_EmptyValue()
        {
            var act = JsonTester.Read<EmailAddressCollection>();
            Assert.IsNull(act);
        }

        [Test]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            Assert.Catch<FormatException>(() =>
            {
                JsonTester.Read<EmailAddressCollection>("InvalidStringValue");
            },
            "Not valid email addresses");
        }
        [Test]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<EmailAddressCollection>("info@qowaiv.org,test@qowaiv.org");
            var exp = EmailAddressCollection.Parse("info@qowaiv.org, test@qowaiv.org");

            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void FromJson_Int64Value_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<EmailAddressCollection>(123456L);
            },
            "JSON deserialization from an integer is not supported.");
        }

        [Test]
        public void FromJson_DoubleValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<EmailAddressCollection>(1234.56);
            },
            "JSON deserialization from a number is not supported.");
        }

        [Test]
        public void FromJson_DateTimeValue_AssertNotSupportedException()
        {
            Assert.Catch<NotSupportedException>(() =>
            {
                JsonTester.Read<EmailAddressCollection>(new DateTime(1972, 02, 14));
            },
            "JSON deserialization from a date is not supported.");
        }

        [Test]
        public void ToJson_DefaultValue_IsNull()
        {
            object act = JsonTester.Write(new EmailAddressCollection());
            Assert.IsNull(act);
        }
        [Test]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(EmailAddressCollection.Parse("info@qowaiv.org, test@qowaiv.org"));
            var exp = "info@qowaiv.org,test@qowaiv.org";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region ToString

        [Test]
        public void ToString_EmptyCollection_StringEmpty()
        {
            var collection = new EmailAddressCollection();

            var exp = string.Empty;
            var act = collection.ToString();

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_1Item_infoAtQowaivDotOrg()
        {
            var collection = new EmailAddressCollection(EmailAddress.Parse("info@qowaiv.org"));

            var exp = "info@qowaiv.org";
            var act = collection.ToString();

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_2Items_infoAtQowaivDotOrgCommaTestAtQowaivDotOrg()
        {
            var collection = new EmailAddressCollection(EmailAddress.Parse("info@qowaiv.org"), EmailAddress.Parse("test@qowaiv.org"));

            var exp = "info@qowaiv.org,test@qowaiv.org";
            var act = collection.ToString();

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = GetTestInstance().ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: 'info@qowaiv.org,test@qowaiv.org', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void ToString_TestStructFormatMailtoF_AreEqual()
        {
            var act = GetTestInstance().ToString(@"mai\lto:f");
            var exp = "mailto:info@qowaiv.org,mailto:test@qowaiv.org";
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Methods

        [Test]
        public void Add_EmailAddressEmptyToEmptyCollection_DoesNotExtendTheCollection()
        {
            var exp = new EmailAddressCollection();
            var act = new EmailAddressCollection
            {
                EmailAddress.Empty
            };

            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void Add_EmailAddressUnknownToEmptyCollection_DoesNotExtendTheCollection()
        {
            var exp = new EmailAddressCollection();
            var act = new EmailAddressCollection
            {
                EmailAddress.Unknown
            };

            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void Add_EmailAddressEmptyToCollection_DoesNotExtendTheCollection()
        {
            var exp = GetTestInstance();
            var act = GetTestInstance();
            act.Add(EmailAddress.Empty);

            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void Add_EmailAddressUnknownToCollection_DoesNotExtendTheCollection()
        {
            var exp = GetTestInstance();
            var act = GetTestInstance();
            act.Add(EmailAddress.Unknown);

            CollectionAssert.AreEqual(exp, act);
        }

        #endregion

        #region (Try)parse

        [Test]
        public void TryParse_TestInstanceWithUnknownAdded_EqualsTestInstance()
        {
            var act = EmailAddressCollection.Parse("info@qowaiv.org,test@qowaiv.org,?");
            var exp = GetTestInstance();

            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void TryParse_Null_EmptyCollection()
        {
            EmailAddressCollection exp = new EmailAddressCollection();
            Assert.IsTrue(EmailAddressCollection.TryParse(null, out EmailAddressCollection act));
            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void TryParse_StringEmpty_EmptyCollection()
        {
            EmailAddressCollection exp = new EmailAddressCollection();
            Assert.IsTrue(EmailAddressCollection.TryParse(string.Empty, out EmailAddressCollection act));
            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void TryParse_Invalid_EmptyCollection()
        {
            var act = EmailAddressCollection.TryParse("invalid");
            var exp = new EmailAddressCollection();

            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void TryParse_SingleEmailAddress_CollectionWithOneItems()
        {
            var act = EmailAddressCollection.TryParse("svo@qowaiv.org");
            var exp = new EmailAddressCollection() { EmailAddress.Parse("svo@qowaiv.org") };

            CollectionAssert.AreEqual(exp, act);
        }

        #endregion

        #region Extensions

        [Test]
        public void ToCollection_EnumerartionOfEmailAddresses_EmailAddressCollection()
        {
            var collection = EmailAddressCollection.Parse("mail@qowaiv.org,info@qowaiv.org,test@qowaiv.org,test@unit.com")
                .Where(email => email.Domain.EndsWith(".org"));

            var act = collection.ToCollection();
            var exp = EmailAddressCollection.Parse("mail@qowaiv.org,info@qowaiv.org,test@qowaiv.org");

            Assert.AreEqual(typeof(EmailAddressCollection), act.GetType(), "The outcome of to collection should be an email address collection.");
            CollectionAssert.AreEqual(exp, act);
        }

        #endregion
    }

    [Serializable]
    public class EmailAddressCollectionSerializeObject
    {
        public int Id { get; set; }
        public EmailAddressCollection Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
