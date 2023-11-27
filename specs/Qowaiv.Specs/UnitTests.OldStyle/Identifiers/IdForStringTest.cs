namespace Qowaiv.UnitTests.Identifiers;

[EmptyTestClass]
public class ForString : StringIdBehavior { }

/// <summary>Tests the identifier SVO.</summary>
public class IdForStringTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Id<ForString> TestStruct = Id<ForString>.Parse("Qowaiv-ID");

    /// <summary>Id<ForString>.Empty should be equal to the default of identifier.</summary>
    [Test]
    public void Empty_None_EqualsDefault()
    {
        Id<ForString>.Empty.Should().Be(default(Id<ForString>));
    }

    /// <summary>Id<ForString>.IsEmpty() should be true for the default of identifier.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        default(Id<ForString>).IsEmpty().Should().BeTrue();
    }

    /// <summary>Id<ForString>.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse()
    {
        TestStruct.IsEmpty().Should().BeFalse();
    }

    [Test]
    public void FromBytes_Null_IsEmpty()
    {
        var fromBytes = Id<ForString>.FromBytes(null);
        fromBytes.Should().Be(Id<ForString>.Empty);
    }

    [Test]
    public void FromBytes_Bytes_IsTestStruct()
    {
        var fromBytes = Id<ForString>.FromBytes([81, 111, 119, 97, 105, 118, 45, 73, 68]);
        fromBytes.Should().Be(TestStruct);
    }

    [Test]
    public void ToByteArray_Empty_EmptyArray()
    {
        var bytes = Id<ForString>.Empty.ToByteArray();
        bytes.Should().BeEmpty();
    }

    [Test]
    public void ToByteArray_TestStruct_FilledArray()
    {
        var bytes = TestStruct.ToByteArray();
        var exepected = "Qowaiv-ID"u8.ToArray();
        bytes.Should().BeEquivalentTo(exepected);
    }

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsValid()
    {
        Id<ForString>.TryParse(null, out var val).Should().BeTrue();
        val.Should().Be(default(Id<ForString>));
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        Id<ForString>.TryParse(string.Empty, out var val).Should().BeTrue();
        val.Should().Be(default(Id<ForString>));
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "0f5ab5ab-12cb-4629-878d-b18b88b9a504";
        Id<ForString>.TryParse(str, out var val).Should().BeTrue();
        val.ToString().Should().Be(str);
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = TestStruct;
            var act = Id<ForString>.TryParse(exp.ToString());
            act.Should().Be(exp);
        }
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
        var exp = "Qowaiv-ID";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<Id<ForString>>("Qowaiv-ID");
        act.Should().Be(TestStruct);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_IdForStringSerializeObject_AreEqual()
    {
        var input = new IdForStringSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForStringSerializeObject
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
    public void XmlSerializeDeserialize_IdForStringSerializeObject_AreEqual()
    {
        var input = new IdForStringSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForStringSerializeObject
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
    public void DataContractSerializeDeserialize_IdForStringSerializeObject_AreEqual()
    {
        var input = new IdForStringSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForStringSerializeObject
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
        var input = new IdForStringSerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForStringSerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };

        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }
#endif

    [Test]
    public void XmlSerializeDeserialize_Default_AreEqual()
    {
        var input = new IdForStringSerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForStringSerializeObject
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

    [TestCase("0F5AB5AB-12CB-4629-878D-B18B88B9A504", "0F5AB5AB-12CB-4629-878D-B18B88B9A504")]
    [TestCase("", "")]
    [TestCase("123456789", 123456789L)]
    public void FromJson(Id<ForString> expected, object json)
    {
        var actual = JsonTester.Read<Id<ForString>>(json);
        actual.Should().Be(expected);
    }

    [Test]
    public void ToString_Empty_StringEmpty()
    {
        var act = Id<ForString>.Empty.ToString();
        var exp = "";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("S", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: 'Qowaiv-ID', format: 'S'";
        act.Should().Be(exp);
    }

    /// <summary>GetHash should not fail for Id<ForString>.Empty.</summary>
    [Test]
    public void GetHash_Empty_Hash()
    {
        Id<ForString>.Empty.GetHashCode().Should().Be(0);
    }

    /// <summary>GetHash should not fail for the test struct.</summary>
    [Test]
    public void GetHash_TestStruct_Hash()
    {
        Assert.AreNotEqual(0, TestStruct.GetHashCode());
    }

    [Test]
    public void Equals_EmptyEmpty_IsTrue()
    {
        Id<ForString>.Empty.Equals(Id<ForString>.Empty).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue()
    {
        TestStruct.Equals(TestStruct).Should().BeTrue();
    }

    [Test]
    public void Equals_TestStructEmpty_IsFalse()
    {
        TestStruct.Equals(Id<ForString>.Empty).Should().BeFalse();
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        Id<ForString>.Empty.Equals(TestStruct).Should().BeFalse();
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
        Assert.Throws<NotSupportedException>(() => Id<ForString>.Next());
    }

    [TestCase(null)]
    [TestCase("")]
    public void IsInvalid_String(string str)
    {
        Id<ForString>.IsValid(str).Should().BeFalse();
    }

    [TestCase("0F5AB5AB-12CB-4629-878D-B18B88B9A504")]
    [TestCase("Qowaiv_SVOLibrary_GUIA")]
    public void IsValid_String(string str)
    {
        Id<ForString>.IsValid(str).Should().BeTrue();
    }
}

[Serializable]
public class IdForStringSerializeObject
{
    public int Id { get; set; }
    public Id<ForString> Obj { get; set; }
    public DateTime Date { get; set; }
}
