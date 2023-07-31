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
        Assert.AreEqual(default(Id<ForString>), Id<ForString>.Empty);
    }

    /// <summary>Id<ForString>.IsEmpty() should be true for the default of identifier.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        Assert.IsTrue(default(Id<ForString>).IsEmpty());
    }

    /// <summary>Id<ForString>.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse()
    {
        Assert.IsFalse(TestStruct.IsEmpty());
    }

    [Test]
    public void FromBytes_Null_IsEmpty()
    {
        var fromBytes = Id<ForString>.FromBytes(null);
        Assert.AreEqual(Id<ForString>.Empty, fromBytes);
    }

    [Test]
    public void FromBytes_Bytes_IsTestStruct()
    {
        var fromBytes = Id<ForString>.FromBytes(new byte[] { 81, 111, 119, 97, 105, 118, 45, 73, 68 });
        Assert.AreEqual(TestStruct, fromBytes);
    }

    [Test]
    public void ToByteArray_Empty_EmptyArray()
    {
        var bytes = Id<ForString>.Empty.ToByteArray();
        Assert.AreEqual(Array.Empty<byte>(), bytes);
    }

    [Test]
    public void ToByteArray_TestStruct_FilledArray()
    {
        var bytes = TestStruct.ToByteArray();
        var exepected = new byte[] { 81, 111, 119, 97, 105, 118, 45, 73, 68 };
        Assert.AreEqual(exepected, bytes);
    }

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsValid()
    {
        Assert.IsTrue(Id<ForString>.TryParse(null, out var val));
        Assert.AreEqual(default(Id<ForString>), val);
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        Assert.IsTrue(Id<ForString>.TryParse(string.Empty, out var val));
        Assert.AreEqual(default(Id<ForString>), val);
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "0f5ab5ab-12cb-4629-878d-b18b88b9a504";
        Assert.IsTrue(Id<ForString>.TryParse(str, out var val));
        Assert.AreEqual(str, val.ToString());
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = TestStruct;
            var act = Id<ForString>.TryParse(exp.ToString());
            Assert.AreEqual(exp, act);
        }
    }

    [Test]
    public void GetObjectData_NulSerializationInfo_Throws()
    {
        ISerializable obj = TestStruct;
        Assert.Catch<ArgumentNullException>(() => obj.GetObjectData(null, default));
    }

    [Test]
    public void GetObjectData_SerializationInfo_AreEqual()
    {
        ISerializable obj = TestStruct;
        var info = new SerializationInfo(typeof(Id<ForString>), new FormatterConverter());
        obj.GetObjectData(info, default);
        Assert.AreEqual("Qowaiv-ID", info.GetValue("Value", typeof(string)));
    }

    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_TestStruct_AreEqual()
    {
        var input = TestStruct;
        var exp = TestStruct;
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp, act);
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
        var exp = "Qowaiv-ID";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act =Deserialize.Xml<Id<ForString>>("Qowaiv-ID");
        Assert.AreEqual(TestStruct, act);
    }

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
        Assert.IsNull(obj.GetSchema());
    }

    [TestCase("0F5AB5AB-12CB-4629-878D-B18B88B9A504", "0F5AB5AB-12CB-4629-878D-B18B88B9A504")]
    [TestCase("", "")]
    [TestCase("123456789", 123456789L)]
    public void FromJson(Id<ForString> expected, object json)
    {
        var actual = JsonTester.Read<Id<ForString>>(json);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ToString_Empty_StringEmpty()
    {
        var act = Id<ForString>.Empty.ToString();
        var exp = "";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("S", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: 'Qowaiv-ID', format: 'S'";
        Assert.AreEqual(exp, act);
    }

    /// <summary>GetHash should not fail for Id<ForString>.Empty.</summary>
    [Test]
    public void GetHash_Empty_Hash()
    {
        Assert.AreEqual(0, Id<ForString>.Empty.GetHashCode());
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
        Assert.IsTrue(Id<ForString>.Empty.Equals(Id<ForString>.Empty));
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue()
    {
        Assert.IsTrue(TestStruct.Equals(TestStruct));
    }

    [Test]
    public void Equals_TestStructEmpty_IsFalse()
    {
        Assert.IsFalse(TestStruct.Equals(Id<ForString>.Empty));
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        Assert.IsFalse(Id<ForString>.Empty.Equals(TestStruct));
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
    public void Next_NotSupported()
    {
        Assert.Throws<NotSupportedException>(()=> Id<ForString>.Next());
    }

    [TestCase(null)]
    [TestCase("")]
    public void IsInvalid_String(string str)
    {
        Assert.IsFalse(Id<ForString>.IsValid(str));
    }

    [TestCase("0F5AB5AB-12CB-4629-878D-B18B88B9A504")]
    [TestCase("Qowaiv_SVOLibrary_GUIA")]
    public void IsValid_String(string str)
    {
        Assert.IsTrue(Id<ForString>.IsValid(str));
    }
}

[Serializable]
public class IdForStringSerializeObject
{
    public int Id { get; set; }
    public Id<ForString> Obj { get; set; }
    public DateTime Date { get; set; }
}
