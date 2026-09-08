namespace Qowaiv.UnitTests.Formatting;

/// <summary>Tests the formatting arguments SVO.</summary>
public class FormattingArgumentsTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly FormattingArguments TestStruct = new("0.000", new CultureInfo("fr-BE"));

    #region formatting arguments const tests

    /// <summary>FormattableArguments.None should be equal to the default of formatting arguments.</summary>
    [Test]
    public void None_None_EqualsDefault() => FormattingArguments.None.Should().Be(default(FormattingArguments));

    #endregion

    #region (De)serialization tests

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
        Should.BeEqual(exp.Id, act.Id, "Id");
        Should.BeEqual(exp.Obj, act.Obj, "Obj");
        Should.BeEqual(exp.Date, act.Date, "Date");
    }

    #endregion

    #region Properties

    [Test]
    public void Format_DefaultValue_StringNull()
    {
        var exp = Nil.String;
        var act = FormattingArguments.None.Format;
        act.Should().Be(exp);
    }
    [Test]
    public void Format_TestStruct_FormatString()
    {
        var exp = "0.000";
        var act = TestStruct.Format;
        act.Should().Be(exp);
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
