﻿namespace Qowaiv.UnitTests.Identifiers;

[EmptyTestClass]
public sealed class ForInt64 : Int64IdBehavior { }

/// <summary>Tests the identifier SVO.</summary>
public class IdForInt64Test
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Id<ForInt64> TestStruct = Id<ForInt64>.Parse("123456789");

    /// <summary>Id<ForInt64>.Empty should be equal to the default of identifier.</summary>
    [Test]
    public void Empty_None_EqualsDefault()
    {
        Assert.AreEqual(default(Id<ForInt64>), Id<ForInt64>.Empty);
    }

    /// <summary>Id<ForInt64>.IsEmpty() should be true for the default of identifier.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue()
    {
        Assert.IsTrue(default(Id<ForInt64>).IsEmpty());
    }

    /// <summary>Id<ForInt64>.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse()
    {
        Assert.IsFalse(TestStruct.IsEmpty());
    }

    [Test]
    public void FromBytes_Null_IsEmpty()
    {
        var fromBytes = Id<ForInt64>.FromBytes(null);
        Assert.AreEqual(Id<ForInt64>.Empty, fromBytes);
    }

    [Test]
    public void FromBytes_Bytes_IsTestStruct()
    {
        var fromBytes = Id<ForInt64>.FromBytes(new byte[] { 21, 205, 91, 7, 0, 0, 0, 0 });
        Assert.AreEqual(TestStruct, fromBytes);
    }

    [Test]
    public void ToByteArray_Empty_EmptyArray()
    {
        var bytes = Id<ForInt64>.Empty.ToByteArray();
        Assert.AreEqual(Array.Empty<byte>(), bytes);
    }

    [Test]
    public void ToByteArray_TestStruct_FilledArray()
    {
        var bytes = TestStruct.ToByteArray();
        var exepected = new byte[] { 21, 205, 91, 7, 0, 0, 0, 0 };
        Assert.AreEqual(exepected, bytes);
    }

    /// <summary>TryParse null should be valid.</summary>
    [Test]
    public void TryParse_Null_IsValid()
    {
        Assert.IsTrue(Id<ForInt64>.TryParse(null, out var val));
        Assert.AreEqual(default(Id<ForInt64>), val);
    }

    /// <summary>TryParse string.Empty should be valid.</summary>
    [Test]
    public void TryParse_StringEmpty_IsValid()
    {
        Assert.IsTrue(Id<ForInt64>.TryParse(string.Empty, out var val));
        Assert.AreEqual(default(Id<ForInt64>), val);
    }

    /// <summary>TryParse with specified string value should be valid.</summary>
    [Test]
    public void TryParse_StringValue_IsValid()
    {
        string str = "123456789";
        Assert.IsTrue(Id<ForInt64>.TryParse(str, out var val));
        Assert.AreEqual(str, val.ToString());
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_StringValue_IsNotValid()
    {
        string str = "ABC";
        Assert.IsFalse(Id<ForInt64>.TryParse(str, out var val));
        Assert.AreEqual(default(Id<ForInt64>), val);
    }

    [Test]
    public void TryCreate_Int_Successful()
    {
        Assert.IsTrue(Id<ForInt64>.TryCreate(13L, out var id));
        Assert.AreEqual(Id<ForInt64>.Parse("13"), id);
    }

    [Test]
    public void Parse_InvalidInput_ThrowsFormatException()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Assert.Catch<FormatException>(() =>
            {
                Id<ForInt64>.Parse("InvalidInput");
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
            var act = Id<ForInt64>.TryParse(exp.ToString());
            Assert.AreEqual(exp, act);
        }
    }

    [Test]
    public void TryParse_InvalidInput_DefaultValue()
    {
        using (TestCultures.En_GB.Scoped())
        {
            var exp = default(Id<ForInt64>);
            var act = Id<ForInt64>.TryParse("InvalidInput");
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
        var info = new SerializationInfo(typeof(Id<ForInt64>), new FormatterConverter());
        obj.GetObjectData(info, default);
        Assert.AreEqual(123456789L, info.GetValue("Value", typeof(long)));
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
        var exp = "123456789";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void XmlDeserialize_XmlString_AreEqual()
    {
        var act =Deserialize.Xml<Id<ForInt64>>("123456789");
        Assert.AreEqual(TestStruct, act);
    }

    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_IdForInt64SerializeObject_AreEqual()
    {
        var input = new IdForInt64SerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14),
        }

        ;
        var exp = new IdForInt64SerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14),
        }

        ;
        var act = SerializeDeserialize.Binary(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

    [Test]
    public void XmlSerializeDeserialize_IdForInt64SerializeObject_AreEqual()
    {
        var input = new IdForInt64SerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14),
        }

        ;
        var exp = new IdForInt64SerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14),
        }

        ;
        var act = SerializeDeserialize.Xml(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

    [Test]
    public void DataContractSerializeDeserialize_IdForInt64SerializeObject_AreEqual()
    {
        var input = new IdForInt64SerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14),
        }

        ;
        var exp = new IdForInt64SerializeObject
        {
            Id = 17,
            Obj = TestStruct,
            Date = new DateTime(1970, 02, 14),
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
        var input = new IdForInt64SerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14),
        }

        ;
        var exp = new IdForInt64SerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14),
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
        var input = new IdForInt64SerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14),
        }

        ;
        var exp = new IdForInt64SerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14),
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
        Assert.Catch<FormatException>(() => JsonTester.Read<Id<ForInt64>>(json));
    }

    [Test]
    public void ToString_Empty_StringEmpty()
    {
        var act = Id<ForInt64>.Empty.ToString(CultureInfo.InvariantCulture);
        var exp = "";
        Assert.AreEqual(exp, act);
    }

    [Test]
    public void ToString_CustomFormatter_SupportsCustomFormatting()
    {
        var act = TestStruct.ToString("#,##0.0", FormatProvider.CustomFormatter);
        var exp = "Unit Test Formatter, value: '123,456,789.0', format: '#,##0.0'";
        Assert.AreEqual(exp, act);
    }

    /// <summary>GetHash should not fail for Id<ForInt64>.Empty.</summary>
    [Test]
    public void GetHash_Empty_Hash()
    {
        Assert.AreEqual(0, Id<ForInt64>.Empty.GetHashCode());
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
        Assert.IsTrue(Id<ForInt64>.Empty.Equals(Id<ForInt64>.Empty));
    }

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = Id<ForInt64>.Parse("123456");
        var r = Id<ForInt64>.Parse("+0000123456");
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
        Assert.IsFalse(TestStruct.Equals(Id<ForInt64>.Empty));
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        Assert.IsFalse(Id<ForInt64>.Empty.Equals(TestStruct));
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
        Assert.Throws<NotSupportedException>(() => Id<ForInt64>.Next());
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("ABC")]
    [TestCase("-1")]
    public void IsInvalid_String(string str)
    {
        Assert.IsFalse(Id<ForInt64>.IsValid(str));
    }

    [TestCase("0")]
    [TestCase("1234")]
    [TestCase("+123456")]
    public void IsValid_String(string str)
    {
        Assert.IsTrue(Id<ForInt64>.IsValid(str));
    }
}

[Serializable]
public class IdForInt64SerializeObject
{
    public int Id { get; set; }
    public Id<ForInt64> Obj { get; set; }
    public DateTime Date { get; set; }
}
