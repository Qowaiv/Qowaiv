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
            var value = Uuid.NewUuid();

            var actual = converter.ConvertTo((object)value, typeof(Guid?));

            Assert.IsInstanceOf<Guid>(actual);
            Assert.AreEqual((Guid)value, actual);
        }
    }
}
