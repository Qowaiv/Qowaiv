namespace Qowaiv.UnitTests.Sql;

/// <summary>Tests the timestamp SVO.</summary>
public class TimestampTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Timestamp TestStruct = 123456789L;

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
