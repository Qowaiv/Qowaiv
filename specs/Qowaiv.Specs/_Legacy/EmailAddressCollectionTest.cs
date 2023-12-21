﻿namespace Qowaiv.UnitTests;

[TestFixture]
public class EmailAddressCollectionTest
{
    private static EmailAddressCollection GetTestInstance()
        => EmailAddressCollection.Parse("info@qowaiv.org,test@qowaiv.org");

    #region (XML) (De)serialization tests

    [Test]
    public void DataContractSerializeDeserialize_TestStruct_AreEqual()
    {
        var input = GetTestInstance();
        var exp = GetTestInstance();
        var act = SerializeDeserialize.DataContract(input);
        act.Should().BeEquivalentTo(exp);
    }

    [Test]
    public void XmlSerialize_TestStruct_AreEqual()
    {
        var act = Serialize.Xml(GetTestInstance());
        var exp = "info@qowaiv.org,test@qowaiv.org";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<EmailAddressCollection>("info@qowaiv.org,test@qowaiv.org");
        act.Should().BeEquivalentTo(GetTestInstance());
    }

    [Test]
    public void DataContractSerializeDeserialize_EmailAddressSerializeObject_AreEqual()
    {
        var input = new EmailAddressCollectionSerializeObject
        {
            Id = 17,
            Obj = GetTestInstance(),
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new EmailAddressCollectionSerializeObject
        {
            Id = 17,
            Obj = GetTestInstance(),
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.DataContract(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        CollectionAssert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

    [Test]
    public void XmlSerializeDeserialize_Empty_AreEqual()
    {
        var input = new EmailAddressCollectionSerializeObject
        {
            Id = 17,
            Obj = [],
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new EmailAddressCollectionSerializeObject
        {
            Id = 17,
            Obj = [],
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Xml(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        CollectionAssert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

    [Test]
    public void GetSchema_None_IsNull()
    {
        IXmlSerializable obj = GetTestInstance();
        obj.GetSchema().Should().BeNull();
    }

    #endregion

    #region ToString

    [Test]
    public void ToString_EmptyCollection_StringEmpty()
    {
        var collection = new EmailAddressCollection();

        var exp = string.Empty;
        var act = collection.ToString();

        act.Should().Be(exp);
    }

    [Test]
    public void ToString_1Item_infoAtQowaivDotOrg()
    {
        var collection = new EmailAddressCollection(EmailAddress.Parse("info@qowaiv.org"));

        var exp = "info@qowaiv.org";
        var act = collection.ToString();

        act.Should().Be(exp);
    }

    [Test]
    public void ToString_2Items_infoAtQowaivDotOrgCommaTestAtQowaivDotOrg()
    {
        var collection = new EmailAddressCollection(EmailAddress.Parse("info@qowaiv.org"), EmailAddress.Parse("test@qowaiv.org"));

        var exp = "info@qowaiv.org,test@qowaiv.org";
        var act = collection.ToString();

        act.Should().Be(exp);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = GetTestInstance().ToString("U", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: 'INFO@QOWAIV.ORG,TEST@QOWAIV.ORG', format: 'U'";

        act.Should().Be(exp);
    }

    [Test]
    public void ToString_TestStructFormatMailToF_AreEqual()
    {
        var act = GetTestInstance().ToString(@"mai\lto:f");
        var exp = "mailto:info@qowaiv.org,mailto:test@qowaiv.org";
        act.Should().Be(exp);
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

        act.Should().BeEquivalentTo(exp);
    }

    [Test]
    public void Add_EmailAddressUnknownToEmptyCollection_DoesNotExtendTheCollection()
    {
        var exp = new EmailAddressCollection();
        var act = new EmailAddressCollection
        {
            EmailAddress.Unknown
        };

        act.Should().BeEquivalentTo(exp);
    }

    [Test]
    public void Add_EmailAddressEmptyToCollection_DoesNotExtendTheCollection()
    {
        var exp = GetTestInstance();
        var act = GetTestInstance();
        act.Add(EmailAddress.Empty);

        act.Should().BeEquivalentTo(exp);
    }

    [Test]
    public void Add_EmailAddressUnknownToCollection_DoesNotExtendTheCollection()
    {
        var exp = GetTestInstance();
        var act = GetTestInstance();
        act.Add(EmailAddress.Unknown);

        act.Should().BeEquivalentTo(exp);
    }

    #endregion

    #region (Try)parse

    [Test]
    public void TryParse_TestInstanceWithUnknownAdded_EqualsTestInstance()
    {
        var act = EmailAddressCollection.Parse("info@qowaiv.org,test@qowaiv.org,?");
        var exp = GetTestInstance();

        act.Should().BeEquivalentTo(exp);
    }

    [Test]
    public void TryParse_Null_EmptyCollection()
    {
        EmailAddressCollection exp = [];
        EmailAddressCollection.TryParse(null, out EmailAddressCollection act).Should().BeTrue();
        act.Should().BeEquivalentTo(exp);
    }

    [Test]
    public void TryParse_StringEmpty_EmptyCollection()
    {
        EmailAddressCollection exp = [];
        EmailAddressCollection.TryParse(string.Empty, out EmailAddressCollection act).Should().BeTrue();
        act.Should().BeEquivalentTo(exp);
    }

    [Test]
    public void TryParse_Invalid_EmptyCollection()
    {
        var act = EmailAddressCollection.TryParse("invalid");
        var exp = new EmailAddressCollection();

        act.Should().BeEquivalentTo(exp);
    }

    [Test]
    public void TryParse_SingleEmailAddress_CollectionWithOneItems()
    {
        var act = EmailAddressCollection.TryParse("svo@qowaiv.org");
        var exp = new EmailAddressCollection { EmailAddress.Parse("svo@qowaiv.org") };

        act.Should().BeEquivalentTo(exp);
    }

    #endregion

    #region Extensions

    [Test]
    public void ToCollection_enumeration_of_EmailAddresses_EmailAddressCollection()
    {
        var collection = EmailAddressCollection.Parse("mail@qowaiv.org,info@qowaiv.org,test@qowaiv.org").AsEnumerable();

        var act = collection.ToCollection();
        var exp = EmailAddressCollection.Parse("mail@qowaiv.org,info@qowaiv.org,test@qowaiv.org");

        Assert.AreEqual(typeof(EmailAddressCollection), act.GetType(), "The outcome of to collection should be an email address collection.");
        act.Should().BeEquivalentTo(exp);
    }

    #endregion
}

[Serializable]
public class EmailAddressCollectionSerializeObject
{
    public int Id { get; set; }
    public EmailAddressCollection? Obj { get; set; }
    public DateTime Date { get; set; }
}
