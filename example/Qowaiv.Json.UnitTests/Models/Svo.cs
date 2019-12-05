using System;
using System.Diagnostics.CodeAnalysis;

namespace Qowaiv.Json.UnitTests.Models
{
    public struct Svo : IJsonSerializable, IEquatable<Svo>
    {
        public Svo(object value) => Value = value;

        public object Value { get; private set; }
        
        public void FromJson() => Value = null;
        public void FromJson(string jsonString) => Value = jsonString;
        public void FromJson(long jsonInteger) => Value = jsonInteger;
        public void FromJson(double jsonNumber) => Value = jsonNumber;
        public void FromJson(DateTime jsonDate) => Value = jsonDate;
        public void FromJson(object json) => Value = json;
        public object ToJson() => Value;

        public override string ToString() => $"SVO, Value: {Value?.ToString() ?? "null"}";

        public override bool Equals(object obj) => obj is Svo svo && Equals(svo);
        public bool Equals([AllowNull] Svo other) => other.Value == Value;
        public override int GetHashCode() => Value is null ? 0: Value.GetHashCode();

    }
}
