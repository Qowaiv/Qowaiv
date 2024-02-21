namespace Qowaiv.TestTools;

/// <summary>A test structure to test <see cref="IXmlSerializable"/> behavior of SVO's.</summary>
[Serializable]
public sealed class XmlStructure<TSvo>
    : IEquatable<XmlStructure<TSvo>>
    where TSvo : struct
{
    /// <summary>Gets and sets int property.</summary>
    public int Id { get; init; } = 17;

    /// <summary>Gets and sets SVO property.</summary>
    public TSvo Svo { get; init; }

    /// <summary>Gets and sets a date (time) property.</summary>
    public DateTime Date { get; init; } = new DateTime(2017, 06, 11, 00, 00, 000, DateTimeKind.Utc);

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is XmlStructure<TSvo> other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(XmlStructure<TSvo>? other)
        => other is { }
        && Id.Equals(other.Id)
        && Svo.Equals(other.Svo)
        && Date.Equals(other.Date);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode()
        => Id.GetHashCode() ^ Svo.GetHashCode() ^ Date.GetHashCode();

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"ID: {Id}, SVO: {Svo}, Date: {Date:O}";
}
