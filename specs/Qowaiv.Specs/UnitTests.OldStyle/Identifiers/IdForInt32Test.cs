namespace Qowaiv.UnitTests.Identifiers;

[EmptyTestClass]
public sealed class ForInt32 : Int32IdBehavior { }

/// <summary>Tests the identifier SVO.</summary>
public class IdForInt32Test
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Id<ForInt32> TestStruct = Id<ForInt32>.Parse("123456789");

    /// <summary>Id<ForInt32>.Empty should be equal to the default of identifier.</summary>
    [Test]
    public void Empty_None_EqualsDefault()
    {
        Id<ForInt32>.Empty.Should().Be(default(Id<ForInt32>));
    }

    /// <summary>Id<ForInt32>.IsEmpty() should be true for the default of identifier.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        default(Id<ForInt32>).IsEmpty().Should().BeTrue();
    }

    /// <summary>Id<ForInt32>.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse()
    {
        TestStruct.IsEmpty().Should().BeFalse();
    }

    [Test]
    public void FromBytes_Null_IsEmpty()
    {
        var fromBytes = Id<ForInt32>.FromBytes(null);
        fromBytes.Should().Be(Id<ForInt32>.Empty);
    }

    [Test]
    public void FromBytes_Bytes_IsTestStruct()
    {
        var fromBytes = Id<ForInt32>.FromBytes([21, 205, 91, 7]);
        fromBytes.Should().Be(TestStruct);
    }

    [Test]
    public void ToByteArray_Empty_EmptyArray()
    {
        var bytes = Id<ForInt32>.Empty.ToByteArray();
        bytes.Should().BeEmpty();
    }

    [Test]
    public void ToByteArray_TestStruct_FilledArray()
    {
        var bytes = TestStruct.ToByteArray();
        var exepected = new byte[] { 21, 205, 91, 7 };
        bytes.Should().BeEquivalentTo(exepected);
    }

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsValid()
    {
        Id<ForInt32>.TryParse(null, out var val).Should().BeTrue();
        val.Should().Be(default(Id<ForInt32>));
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        Id<ForInt32>.TryParse(string.Empty, out var val).Should().BeTrue();
        val.Should().Be(default(Id<ForInt32>));
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "123456789";
        Id<ForInt32>.TryParse(str, out var val).Should().BeTrue();
        val.ToString().Should().Be(str);
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "ABC";
        Id<ForInt32>.TryParse(str, out var val).Should().BeFalse();
        val.Should().Be(default(Id<ForInt32>));
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Assert.Catch<FormatException>(() =>
            {
                Id<ForInt32>.Parse("InvalidInput");
            }

            , "Not a valid identifier");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = TestStruct;
            var act = Id<ForInt32>.TryParse(exp.ToString());
            act.Should().Be(exp);
        }
    }

    [Test]
    public void TryParse_InvalidInput_DefaultValue()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = default(Id<ForInt32>);
            var act = Id<ForInt32>.TryParse("InvalidInput");
            act.Should().Be(exp);
        }
    }

    [Test]
    public void TryCreate_Int_Successful()
    {
        Id<ForInt32>.TryCreate(13, out var id).Should().BeTrue();
        id.Should().Be(Id<ForInt32>.Parse("13"));
    }

    [Test]
    public void DataContractSerializeDeserialize_TestStruct_AreEqual()
    {
        var input = TestStruct;
        var exp = TestStruct;
        var act = SerializeDeserialize.DataContract(input);
        act.Should().Be(exp);
    }

    [Test]
    public void XmlSerialize_TestStruct_AreEqual()
    {
        var act = Serialize.Xml(TestStruct);
        var exp = "123456789";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<Id<ForInt32>>("123456789");
        act.Should().Be(TestStruct);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_IdForInt32SerializeObject_AreEqual()
    {
        var input = new IdForInt32SerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForInt32SerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
#endif

    [Test]
    public void XmlSerializeDeserialize_IdForInt32SerializeObject_AreEqual()
    {
        var input = new IdForInt32SerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForInt32SerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var act = SerializeDeserialize.Xml(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

    [Test]
    public void DataContractSerializeDeserialize_IdForInt32SerializeObject_AreEqual()
    {
        var input = new IdForInt32SerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForInt32SerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var act = SerializeDeserialize.DataContract(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_Default_AreEqual()
    {
        var input = new IdForInt32SerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForInt32SerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
#endif

    [Test]
    public void XmlSerializeDeserialize_Default_AreEqual()
    {
        var input = new IdForInt32SerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForInt32SerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var act = SerializeDeserialize.Xml(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

    [Test]
    public void GetSchema_None_IsNull()
    {
        IXmlSerializable obj = TestStruct;
        obj.GetSchema().Should().BeNull();
    }

    [Test]
    public void ToString_Empty_StringEmpty()
    {
        var act = Id<ForInt32>.Empty.ToString(CultureInfo.InvariantCulture);
        var exp = "";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("#,##0.0", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: '123,456,789.0', format: '#,##0.0'";
        act.Should().Be(exp);
    }

    /// <summary>GetHash should not fail for Id<ForInt32>.Empty.</summary>
    [Test]
    public void GetHash_Empty_Hash()
    {
        Id<ForInt32>.Empty.GetHashCode().Should().Be(0);
    }

    /// <summary>GetHash should not fail for the test struct.</summary>
    [Test]
    public void GetHash_TestStruct_Hash()
    {
        TestStruct.GetHashCode().Should().NotBe(0);
    }

    [Test]
    public void Equals_EmptyEmpty_IsTrue()
    {
        Id<ForInt32>.Empty.Equals(Id<ForInt32>.Empty).Should().BeTrue();
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = Id<ForInt32>.Parse("123456");
        var r = Id<ForInt32>.Parse("+0000123456");
        l.Equals(r).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue()
    {
        TestStruct.Equals(TestStruct).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructEmpty_IsFalse()
    {
        TestStruct.Equals(Id<ForInt32>.Empty).Should().BeFalse();
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        Id<ForInt32>.Empty.Equals(TestStruct).Should().BeFalse();
    }

    [Test]
    public void Equals_TestStructObjectTestStruct_IsTrue()
    {
        TestStruct.Equals((object)TestStruct).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructNull_IsFalse()
    {
        TestStruct.Equals(null).Should().BeFalse();
    }

    [Test]
    public void Equals_TestStructObject_IsFalse()
    {
        TestStruct.Equals(new object()).Should().BeFalse();
    }

    [Test]
    public void OperatorIs_TestStructTestStruct_IsTrue()
    {
        var l = TestStruct;
        var r = TestStruct;
        (l == r).Should().BeTrue();
    }

    [Test]
    public void OperatorIsNot_TestStructTestStruct_IsFalse()
    {
        var l = TestStruct;
        var r = TestStruct;
        (l != r).Should().BeFalse();
    }

    [Test]
    public void Next_NotSupported()
    {
        Assert.Throws<NotSupportedException>(() => Id<ForInt32>.Next());
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("ABC")]
    [TestCase("-1")]
    public void IsInvalid_String(string str)
    {
        Id<ForInt32>.IsValid(str).Should().BeFalse();
    }

    [TestCase("0")]
    [TestCase("1234")]
    [TestCase("+123456")]
    public void IsValid_String(string str)
    {
        Id<ForInt32>.IsValid(str).Should().BeTrue();
    }
}

[Serializable]
public class IdForInt32SerializeObject
{
    public int Id { get; set; }
    public Id<ForInt32> Obj { get; set; }
    public DateTime Date { get; set; }
}
