using System;

namespace Qowaiv.Json.UnitTests.Models
{
    public class SvoClass : IJsonSerializable
    {
        public SvoClass() { }
        public SvoClass(object value) => Value = value;

        public object Value { get; private set; }

        public void FromJson() => Value = null;
        public void FromJson(string jsonString) => Value = jsonString;
        public void FromJson(long jsonInteger) => Value = jsonInteger;
        public void FromJson(double jsonNumber) => Value = jsonNumber;
        public void FromJson(DateTime jsonDate) => Value = jsonDate;
        public void FromJson(object json) => Value = json;
        public object ToJson() => Value;

        public override string ToString() => $"SVO, Value: {Value?.ToString() ?? "null"}";
    }
}
