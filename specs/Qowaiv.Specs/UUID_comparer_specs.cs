namespace UUID_comparer_specs;

public class Compares
{
    private readonly Guid guid = Guid.Parse("40E56044-F781-43BD-A4AE-AA08882B4E28");
    private readonly Uuid uuid = Uuid.Parse("BD411BB9-D8C9-4A4F-B739-57F2312E0BC5");

    /// <summary>Proves that the<paramref name="index0" /> has an higher priority than the paired
    /// <paramref name="index1" />.
    /// </summary>
    [TestCase(10, 11)]
    [TestCase(11, 12)]
    [TestCase(12, 13)]
    [TestCase(13, 14)]
    [TestCase(14, 15)]
    [TestCase(15, 08)]
    [TestCase(08, 09)]
    [TestCase(09, 06)]
    [TestCase(06, 07)]
    [TestCase(07, 04)]
    [TestCase(04, 05)]
    [TestCase(05, 00)]
    [TestCase(00, 01)]
    [TestCase(01, 02)]
    [TestCase(02, 03)]
    public void SQL_Server(int index0, int index1)
    {
        var l = Simple(index0, index1, 1, 2);
        var r = Simple(index0, index1, 2, 1);

        var compare = UuidComparer.SqlServer.Compare(l, r);
        compare.Should().Be(-1);
    }

    private static Uuid Simple(int i0, int i1, byte v0, byte v1)
    {
        var bytes = new byte[16];
        bytes[i0] = v0;
        bytes[i1] = v1;
        return new Guid(bytes);
    }

    [Test]
    public void Is_ordered_Comparer_Guid_Default()
    {
        var uuids = new List<Guid>();

        for (var i = 0; i < 1000; i++)
        {
            uuids.Add(Uuid.NewUuid());
        }
        uuids.Sort(UuidComparer.Default);
        uuids.Should().BeInAscendingOrder();
    }

    [Test]
    public void Null_with_null_as_zero()
        => UuidComparer.Default.Compare(null, null).Should().Be(0);

    [Test]
    public void Null_with_GUID_as_minus_1()
        => UuidComparer.Default.Compare(null, Guid.NewGuid()).Should().Be(-1);

    [Test]
    public void Null_with_UUID_as_minus_1()
    => UuidComparer.Default.Compare(null, Uuid.NewUuid()).Should().Be(-1);

    [Test]
    public void GUID_with_null_as_plus_1()
        => UuidComparer.Default.Compare(Guid.NewGuid(), null).Should().Be(+1);

    [Test]
    public void UUID_with_null_as_plus_1()
        => UuidComparer.Default.Compare(Uuid.NewUuid(), null).Should().Be(+1);

    [Test]
    public void GUID_with_UUID()
        => UuidComparer.Default.Compare((object)guid, (object)uuid).Should().Be(-1);

    [Test]
    public void UUID_with_GUIDF()
        => UuidComparer.Default.Compare((object)uuid, (object)guid).Should().Be(+1);

    [Test]
    public void Compare_UuidObject_Throws()
        => new object().Invoking(o => UuidComparer.Default.Compare((object)uuid, o))
            .Should().Throw<NotSupportedException>();
}
