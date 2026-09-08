namespace Qowaiv.UnitTests.IO;

/// <summary>Tests the stream size SVO.</summary>
public class StreamSizeTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly StreamSize TestStruct = 123456789;

    #region stream size const tests

    /// <summary>StreamSize.Empty should be equal to the default of stream size.</summary>
    [Test]
    public void Empty_None_EqualsDefault() => StreamSize.Zero.Should().Be(default);

    #endregion

    #region From byte factory methods

    [Test]
    public void FromKilobytes_2_2000()
    {
        var size = StreamSize.FromKilobytes(2);
        var act = (long)size;
        var exp = 2000L;
        act.Should().Be(exp);
    }
    [Test]
    public void FromMegabytes_3Dot5_3500000()
    {
        var size = StreamSize.FromMegabytes(3.5);
        var act = (long)size;
        var exp = 3500000L;
        act.Should().Be(exp);
    }
    [Test]
    public void FromGigabytes_0Dot8_800000000()
    {
        var size = StreamSize.FromGigabytes(0.8);
        var act = (long)size;
        var exp = 800000000L;
        act.Should().Be(exp);
    }
    [Test]
    public void FromTerabytes_10_10000000000000()
    {
        var size = StreamSize.FromTerabytes(10);
        var act = (long)size;
        var exp = 10000000000000L;
        act.Should().Be(exp);
    }

    [Test]
    public void FromKibibytes_2_2048()
    {
        var size = StreamSize.FromKibibytes(2);
        var act = (long)size;
        var exp = 2048L;
        act.Should().Be(exp);
    }
    [Test]
    public void FromMebibytes_3Dot5_3670016()
    {
        var size = StreamSize.FromMebibytes(3.5);
        var act = (long)size;
        var exp = 3670016L;
        act.Should().Be(exp);
    }
    [Test]
    public void FromGibibytes_0Dot8_858993459()
    {
        var size = StreamSize.FromGibibytes(0.8);
        var act = (long)size;
        var exp = 858993459L;
        act.Should().Be(exp);
    }
    [Test]
    public void FromTebibytes_10_10995116277760()
    {
        var size = StreamSize.FromTebibytes(10);
        var act = (long)size;
        var exp = 10995116277760L;
        act.Should().Be(exp);
    }

    #endregion

    [TestCase(-1, "-23KB")]
    [TestCase(0, "0KB")]
    [TestCase(+1, "16KB")]
    public void Sign(int expected, StreamSize size)
    {
        var actual = size.Sign();
        actual.Should().Be(expected);
    }

    [TestCase(1234, -1234)]
    [TestCase(1234, +1234)]
    public void Abs(StreamSize expected, StreamSize value)
    {
        var abs = value.Abs();
        abs.Should().Be(expected);
    }

    [Test]
    public void Increment_21_22()
    {
        StreamSize act = 21;
        StreamSize exp = 22;
        act++;

        act.Should().Be(exp);
    }
    [Test]
    public void Decrement_21_20()
    {
        StreamSize act = 21;
        StreamSize exp = 20;
        act--;

        act.Should().Be(exp);
    }

    [Test]
    public void Plus_21_21()
    {
        StreamSize act = +((StreamSize)21);
        StreamSize exp = 21;

        act.Should().Be(exp);
    }
    [Test]
    public void Negate_21_Minus21()
    {
        StreamSize act = -((StreamSize)21);
        StreamSize exp = -21;

        act.Should().Be(exp);
    }

    [Test]
    public void Addition_17Percentage10_18()
    {
        StreamSize act = 17;
        StreamSize exp = 18;
        act += Percentage.Create(0.1);

        act.Should().Be(exp);
    }
    [Test]
    public void Addition_17And5_24()
    {
        StreamSize act = 17;
        StreamSize exp = 24;
        act += (StreamSize)7;

        act.Should().Be(exp);
    }

    [Test]
    public void Subtraction_17Percentage10_16()
    {
        StreamSize act = 17;
        StreamSize exp = 16;
        act -= Percentage.Create(0.1);

        act.Should().Be(exp);
    }
    [Test]
    public void Subtraction_17And5_12()
    {
        StreamSize act = 17;
        StreamSize exp = 12;
        act -= (StreamSize)5;

        act.Should().Be(exp);
    }

    [Test]
    public void Division_81And2Int16_40()
    {
        StreamSize act = 81;
        StreamSize exp = 40;
        act /= (short)2;

        act.Should().Be(exp);
    }
    [Test]
    public void Division_81And2Int32_40()
    {
        StreamSize act = 81;
        StreamSize exp = 40;
        act /= 2;

        act.Should().Be(exp);
    }

    [Test]
    public void Division_81And2Int64_40()
    {
        StreamSize act = 81;
        StreamSize exp = 40;
        act /= (long)2;

        act.Should().Be(exp);
    }
    [Test]
    public void Division_81And2UInt16_40()
    {
        StreamSize act = 81;
        StreamSize exp = 40;
        act /= (ushort)2;

        act.Should().Be(exp);
    }
    [Test]
    public void Division_81And2UInt32_40()
    {
        StreamSize act = 81;
        StreamSize exp = 40;
        act /= (uint)2;

        act.Should().Be(exp);
    }
    [Test]
    public void Division_81And2UInt64_40()
    {
        StreamSize act = 81;
        StreamSize exp = 40;
        act /= (ulong)2;

        act.Should().Be(exp);
    }
    [Test]
    public void Division_81And150Percentage_54()
    {
        StreamSize act = 81;
        StreamSize exp = 54;
        act /= (Percentage)1.50;

        act.Should().Be(exp);
    }
    [Test]
    public void Division_81And1Point5Single_54()
    {
        StreamSize act = 81;
        StreamSize exp = 54;
        act /= (float)1.5;

        act.Should().Be(exp);
    }
    [Test]
    public void Division_81And1Point5Double_54()
    {
        StreamSize act = 81;
        StreamSize exp = 54;
        act /= 1.5;

        act.Should().Be(exp);
    }
    [Test]
    public void Division_81And1Point5Decimal_54()
    {
        StreamSize act = 81;
        StreamSize exp = 54;
        act /= 1.5d;

        act.Should().Be(exp);
    }

    [Test]
    public void Multiply_42And3Int16_126()
    {
        StreamSize act = 42;
        StreamSize exp = 126;
        act *= (short)3;

        act.Should().Be(exp);
    }
    [Test]
    public void Multiply_42And3Int32_126()
    {
        StreamSize act = 42;
        StreamSize exp = 126;
        act *= 3;

        act.Should().Be(exp);
    }
    [Test]
    public void Multiply_42And3Int64_126()
    {
        StreamSize act = 42;
        StreamSize exp = 126;
        act *= (long)3;

        act.Should().Be(exp);
    }
    [Test]
    public void Multiply_42And3UInt16_126()
    {
        StreamSize act = 42;
        StreamSize exp = 126;
        act *= (ushort)3;

        act.Should().Be(exp);
    }
    [Test]
    public void Multiply_42And3UInt32_126()
    {
        StreamSize act = 42;
        StreamSize exp = 126;
        act *= (uint)3;

        act.Should().Be(exp);
    }
    [Test]
    public void Multiply_42And3UInt64_126()
    {
        StreamSize act = 42;
        StreamSize exp = 126;
        act *= (ulong)3;

        act.Should().Be(exp);
    }
    [Test]
    public void Multiply_42And50Percentage_21()
    {
        StreamSize act = 42;
        StreamSize exp = 21;
        act *= 50.Percent();

        act.Should().Be(exp);
    }
    [Test]
    public void Multiply_42AndHalfSingle_21()
    {
        StreamSize act = 42;
        StreamSize exp = 21;
        act *= (float)0.5;

        act.Should().Be(exp);
    }
    [Test]
    public void Multiply_42AndHalfDouble_21()
    {
        StreamSize act = 42;
        StreamSize exp = 21;
        act *= 0.5;

        act.Should().Be(exp);
    }
    [Test]
    public void Multiply_42AndHalfDecimal_21()
    {
        StreamSize act = 42;
        StreamSize exp = 21;
        act *= 0.5d;

        act.Should().Be(exp);
    }

    #region Extension tests

    [Test]
    public void GetStreamSize_Stream_17Byte()
    {
        using var stream = new MemoryStream([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17]);

        StreamSize act = stream.GetStreamSize();
        StreamSize exp = 17;

        act.Should().Be(exp);
    }

    [Test]
    public void GetStreamSize_FileInfo_9Byte()
    {
        using var dir = new TemporaryDirectory();

        FileInfo file = dir.CreateFile("GetStreamSize_FileInfo_9.test");
        using (var writer = new StreamWriter(file.FullName, false))
        {
            writer.Write("Unit Test");
        }

        StreamSize act = file.GetStreamSize();
        StreamSize exp = 9;

        act.Should().Be(exp);
    }

    [Test]
    public void Average_ArrayOfStreamSizes_5Byte()
    {
        var arr = new StreamSize[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        StreamSize act = arr.Average();
        StreamSize exp = 5;

        act.Should().Be(exp);
    }
    [Test]
    public void Sum_ArrayOfStreamSizes_45Byte()
    {
        var arr = new StreamSize[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        StreamSize act = arr.Sum();
        StreamSize exp = 45;

        act.Should().Be(exp);
    }

    #endregion
}
