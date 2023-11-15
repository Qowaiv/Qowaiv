namespace Qowaiv.UnitTests.Formatting;

/// <summary>Tests the formatting arguments SVO.</summary>
public class FormattingArgumentsTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly FormattingArguments TestStruct = new("0.000", new CultureInfo("fr-BE"));

    #region formatting arguments const tests

    /// <summary>FormattableArguments.None should be equal to the default of formatting arguments.</summary>
    [Test]
    public void None_None_EqualsDefault()
    {
        Assert.AreEqual(default(FormattingArguments), FormattingArguments.None);
    }

    #endregion

    #region (De)serialization tests

    [Test]
    public void GetObjectData_SerializationInfo_AreEqual()
    {
        ISerializable obj = TestStruct;
        var info = new SerializationInfo(typeof(FormattingArguments), new System.Runtime.Serialization.FormatterConverter());
        obj.GetObjectData(info, default);

        Assert.AreEqual("0.000", info.GetString("Format"));
        Assert.AreEqual(new CultureInfo("fr-BE"), info.GetValue("FormatProvider", typeof(IFormatProvider)));
    }

#if NET8_0_OR_GREATER
#else
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void SerializeDeserialize_Default_AreEqual()
    {
        var input = new FormattableArgumentsSerializeObject
        {
            Id = 17,
            Obj = default,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new FormattableArgumentsSerializeObject
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
    public void XmlSerializeDeserialize_Empty_AreEqual()
    {
        var input = new FormattableArgumentsSerializeObject
        {
            Id = 17,
            Obj = FormattingArguments.None,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var exp = new FormattableArgumentsSerializeObject
        {
            Id = 17,
            Obj = FormattingArguments.None,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var act = SerializeDeserialize.Xml(input);
        Assert.AreEqual(exp.Id, act.Id, "Id");
        Assert.AreEqual(exp.Obj, act.Obj, "Obj");
        Assert.AreEqual(exp.Date, act.Date, "Date");
    }

    #endregion

    #region IEquatable tests

    /// <summary>GetHash should not fail for FormattableArguments.Empty.</summary>
    [Test]
    public void GetHash_Empty_0()
    {
        Assert.AreEqual(0, FormattingArguments.None.GetHashCode());
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
        Assert.IsTrue(FormattingArguments.None.Equals(default));
    }

    [Test]
    public void Equals_TestStructTestStruct_IsTrue()
    {
        Assert.IsTrue(TestStruct.Equals(new FormattingArguments("0.000", new CultureInfo("fr-BE"))));
    }

    [Test]
    public void Equals_TestStructEmpty_IsFalse()
    {
        Assert.IsFalse(TestStruct.Equals(FormattingArguments.None));
    }

    [Test]
    public void Equals_EmptyTestStruct_IsFalse()
    {
        Assert.IsFalse(FormattingArguments.None.Equals(TestStruct));
    }

    [Test]
    public void Equals_TestStructObjectTestStruct_IsTrue()
    {
        object obj = TestStruct;
        Assert.IsTrue(TestStruct.Equals(obj));
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

    #region Properties

    [Test]
    public void Format_DefaultValue_StringNull()
    {
        var exp = Nil.String;
        var act = FormattingArguments.None.Format;
        Assert.AreEqual(exp, act);
    }
    [Test]
    public void Format_TestStruct_FormatString()
    {
        var exp = "0.000";
        var act = TestStruct.Format;
        Assert.AreEqual(exp, act);
    }
    #endregion
}

[Serializable]
public class FormattableArgumentsSerializeObject
{
    public int Id { get; set; }
    public FormattingArguments Obj { get; set; }
    public DateTime Date { get; set; }
}
