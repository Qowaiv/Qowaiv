namespace Qowaiv.UnitTests.Identifiers;

public class IdCastTest
{
    [Test]
    public void CreateString_Null_IsEmpty()
    {
        var fromNull = Id<ForString>.Create(null);
        Assert.IsTrue(fromNull.IsEmpty());
    }

    [Test]
    public void CreateGuid_Null_IsEmpty()
    {
        var fromNull = Id<ForGuid>.Create(null);
        Assert.IsTrue(fromNull.IsEmpty());
    }

    [Test]
    public void CreateInt64_Null_IsEmpty()
    {
        var fromNull = Id<ForInt64>.Create(null);
        Assert.IsTrue(fromNull.IsEmpty());
    }

    [Test]
    public void Create_StringForInt64_Throws()
    {
        var x = Assert.Catch<InvalidCastException>(() => Id<ForInt64>.Create("NaN"));
        Assert.AreEqual("Cast from string to Qowaiv.Identifiers.Id<Qowaiv.UnitTests.Identifiers.ForInt64> is not valid.", x.Message);
    }

    [Test]
    public void Create_Int64ForGuid_Throws()
    {
        var x = Assert.Catch<InvalidCastException>(() => Id<ForGuid>.Create(13245L));
        Assert.AreEqual("Cast from long to Qowaiv.Identifiers.Id<Qowaiv.UnitTests.Identifiers.ForGuid> is not valid.", x.Message);
    }

    [Test]
    public void FromJson_NegativeValue_Throws()
    {
        var x = Assert.Catch<InvalidCastException>(() => Id<ForInt64>.FromJson(-1));
        Assert.AreEqual("Cast from long to Qowaiv.Identifiers.Id<Qowaiv.UnitTests.Identifiers.ForInt64> is not valid.", x.Message);
    }

    [Test]
    public void Cast_FromGuidToGuid_Successful()
    {
        var guid = Guid.Parse("AD38ECD4-020F-475C-9318-DFF2067DA1D4");
        var casted = (Id<ForGuid>)guid;
        var expected = Id<ForGuid>.Parse("AD38ECD4-020F-475C-9318-DFF2067DA1D4");
        Assert.AreEqual(expected, casted);
    }

    [Test]
    public void Cast_FromGuidToString_Successful()
    {
        var guid = Guid.Parse("AD38ECD4-020F-475C-9318-DFF2067DA1D4");
        var casted = (Id<ForString>)guid;
        var expected = Id<ForString>.Parse("ad38ecd4-020f-475c-9318-dff2067da1d4");
        Assert.AreEqual(expected, casted);
    }

    [Test]
    public void Cast_FromInt64ToInt64_Successful()
    {
        var number = 12345L;
        var casted = (Id<ForInt64>)number;
        var expected = Id<ForInt64>.Create(12345L);
        Assert.AreEqual(expected, casted);
    }

    [Test]
    public void Cast_FromStringToInt64_Successful()
    {
        var number = "12345";
        var casted = (Id<ForInt64>)number;
        var expected = Id<ForInt64>.Create(12345L);
        Assert.AreEqual(expected, casted);
    }

    [Test]
    public void Cast_FromInt64ToString_Successful()
    {
        var number = 12345L;
        var casted = (Id<ForString>)number;
        var expected = Id<ForString>.Parse("12345");
        Assert.AreEqual(expected, casted);
    }


    [Test]
    public void Cast_FromStringToInt64_Throws()
    {
        var str = "ABC";
        Assert.Throws<InvalidCastException>(() => Void((Id<ForInt64>)str));
    }

    [Test]
    public void Cast_FromInt64ToInt64_Throws()
    {
        var guid = Guid.NewGuid();
        Assert.Throws<InvalidCastException>(() => Void((Id<ForInt64>)guid));
    }

    [Test]
    public void Cast_FromStringToGuid_Throws()
    {
        var str = "12345";
        Assert.Throws<InvalidCastException>(() => Void((Id<ForGuid>)str));
    }

    [Test]
    public void Cast_FromInt64ToGuid_Throws()
    {
        var number = 12345L;
        Assert.Throws<InvalidCastException>(() => Void((Id<ForGuid>)number));
    }

    private static void Void(object obj) { /* Do nothing */}
}
