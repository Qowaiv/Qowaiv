using NUnit.Framework;
using Qowaiv.Conversion;
using System;

namespace Qowaiv.UnitTests
{
    public class SvoTypeConverterTest
    {
        [Test]
        public void ConvertTo_Nullable()
        {
            var converter = new UuidTypeConverter();
            object value = Uuid.NewUuid();

            var actual = converter.ConvertTo(value, typeof(Guid?));

            Assert.AreEqual((Guid)value, actual);
        }
    }
}
