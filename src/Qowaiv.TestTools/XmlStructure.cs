namespace Qowaiv.TestTools;

/// <summary>A <see cref="XmlStructure{TSvo}"/> factory.</summary>
public static class XmlStructure
{
    /// <summary>Initializes a new instance of the <see cref="XmlStructure{TSvo}"/> class.</summary>
    [Pure]
    public static XmlStructure<TSvo> New<TSvo>(TSvo svo) where TSvo : struct
        => new() { Svo = svo };
}
