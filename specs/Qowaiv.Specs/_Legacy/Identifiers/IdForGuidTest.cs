namespace Qowaiv.UnitTests.Identifiers;

[Obsolete("Will be dropped in Qowaiv 8.0")]
public sealed class ForGuid : GuidBehavior { }

/// <summary>Tests the identifier SVO.</summary>
[Obsolete("Will be dropped in Qowaiv 8.0")]
public class IdForGuidTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Id<ForGuid> TestStruct = Id<ForGuid>.Parse("0F5AB5AB-12CB-4629-878D-B18B88B9A504");

    /// <summary>Id<ForGuid>.Empty should be equal to the default of identifier.</summary>
    [Test]
    public void Empty_None_EqualsDefault()
    {
        Id<ForGuid>.Empty.Should().Be(default);
    }

    /// <summary>Id<ForGuid>.IsEmpty() should be true for the default of identifier.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        default(Id<ForGuid>).IsEmpty().Should().BeTrue();
    }

    /// <summary>Id<ForGuid>.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse()
    {
        TestStruct.IsEmpty().Should().BeFalse();
    }

    [Test]
    public void FromBytes_Null_IsEmpty()
    {
        var fromBytes = Id<ForGuid>.FromBytes(null);
        fromBytes.Should().Be(Id<ForGuid>.Empty);
    }

    [Test]
    public void FromBytes_Bytes_IsTestStruct()
    {
        var fromBytes = Id<ForGuid>.FromBytes([171, 181, 90, 15, 203, 18, 41, 70, 135, 141, 177, 139, 136, 185, 165, 4]);
        fromBytes.Should().Be(TestStruct);
    }

    [Test]
    public void ToByteArray_Empty_EmptyArray()
    {
        var bytes = Id<ForGuid>.Empty.ToByteArray();
        bytes.Should().BeEmpty();
    }

    [Test]
    public void ToByteArray_TestStruct_FilledArray()
    {
        var bytes = TestStruct.ToByteArray();
        var exepected = new byte[] { 171, 181, 90, 15, 203, 18, 41, 70, 135, 141, 177, 139, 136, 185, 165, 4 };
        bytes.Should().BeEquivalentTo(exepected);
    }

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsValid()
    {
        Id<ForGuid>.TryParse(null, out var val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        Id<ForGuid>.TryParse(string.Empty, out var val).Should().BeTrue();
        val.Should().Be(default);
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "0f5ab5ab-12cb-4629-878d-b18b88b9a504";
        Id<ForGuid>.TryParse(str, out var val).Should().BeTrue();
        val.ToString().Should().Be(str);
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "0F5AB5AB-12CB-4629-878D";
        Id<ForGuid>.TryParse(str, out var val).Should().BeFalse();
        val.Should().Be(default);
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Assert.Catch<FormatException>(() =>
            {
                Id<ForGuid>.Parse("InvalidInput");
            }

            , "Not a valid identifier");
        }
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = TestStruct;
            var act = Id<ForGuid>.TryParse(exp.ToString());
            act.Should().Be(exp);
        }
    }

    [Test]
    public void TryParse_InvalidInput_DefaultValue()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = default(Id<ForGuid>);
            var act = Id<ForGuid>.TryParse("InvalidInput");
            act.Should().Be(exp);
        }
    }

    [Test]
    public void TryCreate_Int_NotSuccessful()
    {
        Id<ForGuid>.TryCreate(17L, out _).Should().BeFalse();
    }

    [Test]
    public void TryCreate_Guid_Successful()
    {
        Id<ForGuid>.TryCreate(Guid.Parse("0F5AB5AB-12CB-4629-878D-B18B88B9A504"), out var id).Should().BeTrue();
        Should.BeEqual(Id<ForGuid>.Parse("0F5AB5AB-12CB-4629-878D-B18B88B9A504"), id);
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
        var exp = "0f5ab5ab-12cb-4629-878d-b18b88b9a504";
        act.Should().Be(exp);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act = Deserialize.Xml<Id<ForGuid>>("0F5AB5AB-12CB-4629-878D-B18B88B9A504");
        act.Should().Be(TestStruct);
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_IdForGuidSerializeObject_AreEqual()
    {
        var input = new IdForGuidSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new IdForGuidSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Binary(input);
        Should.BeEqual(exp.Id, act.Id, "Id");
        Should.BeEqual(exp.Obj, act.Obj, "Obj");
        Should.BeEqual(exp.Date, act.Date, "Date");
    }
#endif

    [Test]
    public void XmlSerializeDeserialize_IdForGuidSerializeObject_AreEqual()
    {
        var input = new IdForGuidSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForGuidSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var act = SerializeDeserialize.Xml(input);
        Should.BeEqual(exp.Id, act.Id, "Id");
        Should.BeEqual(exp.Obj, act.Obj, "Obj");
        Should.BeEqual(exp.Date, act.Date, "Date");
    }

    [Test]
    public void DataContractSerializeDeserialize_IdForGuidSerializeObject_AreEqual()
    {
        var input = new IdForGuidSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForGuidSerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var act = SerializeDeserialize.DataContract(input);
        Should.BeEqual(exp.Id, act.Id, "Id");
        Should.BeEqual(exp.Obj, act.Obj, "Obj");
        Should.BeEqual(exp.Date, act.Date, "Date");
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_Default_AreEqual()
    {
        var input = new IdForGuidSerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForGuidSerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var act = SerializeDeserialize.Binary(input);
        Should.BeEqual(exp.Id, act.Id, "Id");
        Should.BeEqual(exp.Obj, act.Obj, "Obj");
        Should.BeEqual(exp.Date, act.Date, "Date");
    }
#endif

    [Test]
    public void XmlSerializeDeserialize_Default_AreEqual()
    {
        var input = new IdForGuidSerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var exp = new IdForGuidSerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        }

        ;
        var act = SerializeDeserialize.Xml(input);
        Should.BeEqual(exp.Id, act.Id, "Id");
        Should.BeEqual(exp.Obj, act.Obj, "Obj");
        Should.BeEqual(exp.Date, act.Date, "Date");
    }

    [Test]
    public void GetSchema_None_IsNull()
    {
        IXmlSerializable obj = TestStruct;
        obj.GetSchema().Should().BeNull();
    }

    [TestCase("2017-06-11")]
    public void FromJson_Invalid_Throws(object json)
    {
        Assert.Catch<FormatException>(() => JsonTester.Read<Id<ForGuid>>(json));
    }

    [TestCase("0F5AB5AB-12CB-4629-878D-B18B88B9A504", "0F5AB5AB-12CB-4629-878D-B18B88B9A504")]
    [TestCase("", "")]
    public void FromJson(Id<ForGuid> expected, object json)
    {
        var actual = JsonTester.Read<Id<ForGuid>>(json);
        actual.Should().Be(expected);
    }

    [Test]
    public void ToJson_TestStruct_StringValue()
    {
        var json = TestStruct.ToJson();
        Should.BeEqual("0f5ab5ab-12cb-4629-878d-b18b88b9a504", json);
    }

    [Test]
    public void ToString_Empty_StringEmpty()
    {
        var act = Id<ForGuid>.Empty.ToString();
        var exp = "";
        act.Should().Be(exp);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("S", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: 'q7VaD8sSKUaHjbGLiLmlBA', format: 'S'";
        act.Should().Be(exp);
    }

    /// <summary>GetHash should not fail for Id<ForGuid>.Empty.</summary>
    [Test]
    public void GetHash_Empty_Hash()
    {
        Id<ForGuid>.Empty.GetHashCode().Should().Be(0);
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
        Id<ForGuid>.Empty.Equals(Id<ForGuid>.Empty).Should().BeTrue();
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = Id<ForGuid>.Parse("Qowaiv_SVOLibrary_GUIA");
        var r = Id<ForGuid>.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");
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
        TestStruct.Equals(Id<ForGuid>.Empty).Should().BeFalse();
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        Id<ForGuid>.Empty.Equals(TestStruct).Should().BeFalse();
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
    public void Next_100Items_AllUnique()
        => Enumerable.Range(0, 100).Select(i => Id<ForGuid>.Next())
        .ToHashSet().Should().HaveCount(100);
}

[Serializable]
public class IdForGuidSerializeObject
{
    public int Id { get; set; }
    public Id<ForGuid> Obj { get; set; }
    public DateTime Date { get; set; }
}
