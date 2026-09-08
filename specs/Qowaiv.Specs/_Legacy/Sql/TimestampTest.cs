namespace Qowaiv.UnitTests.Sql;

/// <summary>Tests the timestamp SVO.</summary>
public class TimestampTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Timestamp TestStruct = 123456789L;

    #region Methods

    [Test]
    public void ToByteArray_TestStruct_()
    {
        var act = TestStruct.ToByteArray();
        var exp = new byte[] { 21, 205, 91, 7, 0, 0, 0, 0 };

        act.Should().BeEquivalentTo(exp);
    }

    #endregion
}
