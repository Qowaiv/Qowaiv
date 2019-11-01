using System;

namespace Qowaiv.Json.UnitTests.Models
{
    public struct Svo : IJsonSerializable
    {
        public Svo(object value) => Value = value;

        public object Value { get; private set; }

        public void FromJson() => Value = null;

        public void FromJson(string jsonString) => Value = jsonString;

        public void FromJson(long jsonInteger) => Value = jsonInteger;

        public void FromJson(double jsonNumber) => Value = jsonNumber;

        public void FromJson(DateTime jsonDate) => Value = jsonDate;

        public object ToJson() => Value;

        public override string ToString() => $"SVO, Value: {Value?.ToString() ?? "null"}";
    }
}
