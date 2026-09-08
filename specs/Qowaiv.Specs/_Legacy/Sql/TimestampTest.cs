namespace Qowaiv.UnitTests.Sql;

/// <summary>Tests the timestamp SVO.</summary>
public class TimestampTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Timestamp TestStruct = 123456789L;

    #region TryParse tests

    [Test]
    public void TryParse_0x00000000075BCD15_IsValid()
    {
        string str = "0x00000000075BCD15";

        Timestamp.TryParse(str, out Timestamp val).Should().BeTrue();
        Should.BeEqual(TestStruct, val, "Value");
    }

    [Test]
    public void TryParse_123456789_IsValid()
    {
        string str = "123456789";

        Timestamp.TryParse(str, out Timestamp val).Should().BeTrue();
        Should.BeEqual(TestStruct, val, "Value");
    }

    /// <summary>TryParse with specified string value should be invalid.</summary>
    [Test]
    public void TryParse_0xInvalidTimeStamp_IsNotValid()
    {
        string str = "0xInvalidTimeStamp";

        Timestamp.TryParse(str, out Timestamp val).Should().BeFalse();
        Should.BeEqual(Timestamp.MinValue, val, "Value");
    }

    [Test]
    public void TryParse_TestStructInput_AreEqual()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exp = TestStruct;
            var act = Timestamp.TryParse(exp.ToString());

            act.Should().Be(exp);
        }
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Timestamp.TryParse("invalid input").Should().BeNull();

    #endregion

    #region IEquatable tests

    [Test]
    public void Equals_FormattedAndUnformatted_IsTrue()
    {
        var l = Timestamp.Parse("0x75bcd15", CultureInfo.InvariantCulture);
        var r = Timestamp.Parse("0x00000000075BCD15", CultureInfo.InvariantCulture);

        l.Equals(r).Should().BeTrue();
    }

    #endregion

    #region Methods

    [Test]
    public void ToByteArray_TestStruct_()
    {
        var act = TestStruct.ToByteArray();
        var exp = new byte[] { 21, 205, 91, 7, 0, 0, 0, 0 };

        act.Should().BeEquivalentTo(exp);
    }

    #endregion

    #region Casting tests

    [Test]
    public void Explicit_ByteArrayToTimestamp_AreEqual()
    {
        var exp = TestStruct;
        var act = (Timestamp)new byte[] { 21, 205, 91, 7, 0, 0, 0, 0 };

        act.Should().Be(exp);
    }
    [Test]
    public void Explicit_TimestampToByteArray_AreEqual()
    {
        var exp = new byte[] { 21, 205, 91, 7, 0, 0, 0, 0 };
        var act = (byte[])TestStruct;

        act.Should().BeEquivalentTo(exp);
    }

    [Test]
    public void Explicit_Int64ToTimestamp_AreEqual()
    {
        var exp = TestStruct;
        var act = (Timestamp)123456789L;

        act.Should().Be(exp);
    }
    [Test]
    public void Explicit_TimestampToInt64_AreEqual()
    {
        var exp = 123456789L;
        var act = (long)TestStruct;

        act.Should().Be(exp);
    }

    [Test]
    public void Explicit_UInt64ToTimestamp_AreEqual()
    {
        var exp = TestStruct;
        var act = (Timestamp)123456789UL;

        act.Should().Be(exp);
    }
    [Test]
    public void Explicit_TimestampToUInt64_AreEqual()
    {
        var exp = 123456789UL;
        var act = (ulong)TestStruct;

        act.Should().Be(exp);
    }

    #endregion
}
