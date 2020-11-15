using System;
using System.Diagnostics.CodeAnalysis;

namespace Qowaiv.UnitTests.TestTools
{
    public static class XmlStructure
    {
        public static XmlStructure<TSvo> New<TSvo>(TSvo svo) where TSvo : struct
            => new XmlStructure<TSvo> { Svo = svo };
    }

    [Serializable]
    public sealed class XmlStructure<TSvo> 
        : IEquatable<XmlStructure<TSvo>>
        where TSvo: struct
    {
        public int Id { get; set; } = 17;
        public TSvo Svo { get; set; }
        public DateTime Date { get; set; } = new DateTime(2017, 06, 10);

        public override bool Equals(object obj) => obj is XmlStructure<TSvo> other && Equals(other);

        public bool Equals([AllowNull] XmlStructure<TSvo> other)
            => other != null
            && Id.Equals(other.Id)
            && Svo.Equals(other.Svo)
            && Date.Equals(other.Date);

        public override int GetHashCode()
            => Id.GetHashCode() ^ Svo.GetHashCode() ^ Date.GetHashCode();

        public override string ToString() => $"ID: {Id}, SVO: {Svo}, Date: {Date}";
    }
}
