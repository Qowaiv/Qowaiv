namespace Qowaiv.UnitTests.Identifiers;

[EmptyTestClass]
public sealed class ForGuid : GuidBehavior { }

/// <summary>Tests the identifier SVO.</summary>
public class IdForGuidTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Id<ForGuid> TestStruct = Id<ForGuid>.Parse("0F5AB5AB-12CB-4629-878D-B18B88B9A504");

    /// <summary>Id<ForGuid>.Empty should be equal to the default of identifier.</summary>
    [Test]
    public void Empty_None_EqualsDefault()
    {
        Assert.AreEqual(default(Id<ForGuid>), Id<ForGuid>.Empty);
    }

    /// <summary>Id<ForGuid>.IsEmpty() should be true for the default of identifier.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        Assert.IsTrue(default(Id<ForGuid>).IsEmpty());
    }

    /// <summary>Id<ForGuid>.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse()
    {
        Assert.IsFalse(TestStruct.IsEmpty());
    }

    [Test]
    public void FromBytes_Null_IsEmpty()
    {
        var fromBytes = Id<ForGuid>.FromBytes(null);
        Assert.AreEqual(Id<ForGuid>.Empty, fromBytes);
    }

    [Test]
    public void FromBytes_Bytes_IsTestStruct()
    {
        var fromBytes = Id<ForGuid>.FromBytes(new byte[] { 171, 181, 90, 15, 203, 18, 41, 70, 135, 141, 177, 139, 136, 185, 165, 4 });
        Assert.AreEqual(TestStruct, fromBytes);
    }

    [Test]
    public void ToByteArray_Empty_EmptyArray()
    {
        var bytes = Id<ForGuid>.Empty.ToByteArray();
        Assert.AreEqual(Array.Empty<byte>(), bytes);
    }

    [Test]
    public void ToByteArray_TestStruct_FilledArray()
    {
        var bytes = TestStruct.ToByteArray();
        var exepected = new byte[] { 171, 181, 90, 15, 203, 18, 41, 70, 135, 141, 177, 139, 136, 185, 165, 4 };
        Assert.AreEqual(exepected, bytes);
    }

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsValid()
    {
        Assert.IsTrue(Id<ForGuid>.TryParse(null, out var val));
        Assert.AreEqual(default(Id<ForGuid>), val);
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        Assert.IsTrue(Id<ForGuid>.TryParse(string.Empty, out var val));
        Assert.AreEqual(default(Id<ForGuid>), val);
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "0f5ab5ab-12cb-4629-878d-b18b88b9a504";
        Assert.IsTrue(Id<ForGuid>.TryParse(str, out var val));
        Assert.AreEqual(str, val.ToString());
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "0F5AB5AB-12CB-4629-878D";
        Assert.IsFalse(Id<ForGuid>.TryParse(str, out var val));
        Assert.AreEqual(default(Id<ForGuid>), val);
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (TestCultures.En_GB.Scoped())
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
        using (TestCultures.En_GB.Scoped())
        {
            var exp = TestStruct;
            var act = Id<ForGuid>.TryParse(exp.ToString());
            Assert.AreEqual(exp, act);
        }
    }

    [Test]
    public void TryParse_InvalidInput_DefaultValue()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = default(Id<ForGuid>);
            var act = Id<ForGuid>.TryParse("InvalidInput");
            Assert.AreEqual(exp, act);
        }
    }

    [Test]
    public void TryCreate_Int_NotSuccessful()
    {
        Assert.IsFalse(Id<ForGuid>.TryCreate(17L, out _));
    }

    [Test]
    public void TryCreate_Guid_Successful()
    {
        Assert.IsTrue(Id<ForGuid>.TryCreate(Guid.Parse("0F5AB5AB-12CB-4629-878D-B18B88B9A504"), out var id));
        Assert.AreEqual(Id<ForGuid>.Parse("0F5AB5AB-12CB-4629-878D-B18B88B9A504"), id);
    }

    [Test]
    public void DataContractSerializeDeserialize_TestStruct_AreEqual()
    {
        var input = TestStruct;
        var exp = TestStruct;
        var act = SerializeDeserialize.DataContract(input);
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void XmlSerialize_TestStruct_AreEqual()
    {
        var act = Serialize.Xml(TestStruct);
        var exp = "0f5ab5ab-12cb-4629-878d-b18b88b9a504";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act =Deserialize.Xml<Id<ForGuid>>("0F5AB5AB-12CB-4629-878D-B18B88B9A504");
        Assert.AreEqual(TestStruct, act);
    }

    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_IdForGuidSerializeObject_AreEqual()
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
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

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
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
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
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

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
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

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
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToJson_TestStruct_StringValue()
    {
        var json = TestStruct.ToJson();
        Assert.AreEqual("0f5ab5ab-12cb-4629-878d-b18b88b9a504", json);
    }

    [Test]
    public void ToString_Empty_StringEmpty()
    {
        var act = Id<ForGuid>.Empty.ToString();
        var exp = "";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("S", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: 'q7VaD8sSKUaHjbGLiLmlBA', format: 'S'";
        Assert.AreEqual(exp, act);
    }

    /// <summary>GetHash should not fail for Id<ForGuid>.Empty.</summary>
    [Test]
    public void GetHash_Empty_Hash()
    {
        Assert.AreEqual(0, Id<ForGuid>.Empty.GetHashCode());
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
        Assert.IsTrue(Id<ForGuid>.Empty.Equals(Id<ForGuid>.Empty));
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = Id<ForGuid>.Parse("Qowaiv_SVOLibrary_GUIA");
        var r = Id<ForGuid>.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");
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
        Assert.IsFalse(TestStruct.Equals(Id<ForGuid>.Empty));
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        Assert.IsFalse(Id<ForGuid>.Empty.Equals(TestStruct));
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

    [Test]
    public void Next_100Items_AllUnique()
    {
        var nexts = Enumerable.Range(0, 100).Select(i => Id<ForGuid>.Next()).ToArray();
        CollectionAssert.AllItemsAreUnique(nexts);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("Complex")]
    public void IsInvalid_String(string str)
    {
        Assert.IsFalse(Id<ForGuid>.IsValid(str));
    }

    [TestCase("0F5AB5AB-12CB-4629-878D-B18B88B9A504")]
    [TestCase("Qowaiv_SVOLibrary_GUIA")]
    public void IsValid_String(string str)
    {
        Assert.IsTrue(Id<ForGuid>.IsValid(str));
    }
}

[Serializable]
public class IdForGuidSerializeObject
{
    public int Id { get; set; }
    public Id<ForGuid> Obj { get; set; }
    public DateTime Date { get; set; }
}
