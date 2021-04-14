using NUnit.Framework;
using Qowaiv;
using System.Globalization;

namespace Unknown_specs
{
    public class Is_unknown
    {
        [Test]
        public void question_mark_with_culture()
        {
            Assert.IsTrue(Unknown.IsUnknown("?", new CultureInfo("nl-NL")));
        }

        [Test]
        public void question_mark_without_culture()
        {
            Assert.IsTrue(Unknown.IsUnknown("?", null));
        }

        [Test]
        public void language_specific_unknown_for_specific_culture()
        {
            Assert.IsTrue(Unknown.IsUnknown("Não SABe", new CultureInfo("pt-PT")));
        }
    }

    public class Is_not_unknown
    {
        [Test]
        public void Null()
        {
            Assert.IsFalse(Unknown.IsUnknown(null));
        }

        [Test]
        public void string_empty()
        {
            Assert.IsFalse(Unknown.IsUnknown(string.Empty));
        }

        [Test]
        public void string_not_representing_unknown_for_specified_culture()
        {
            Assert.IsFalse(Unknown.IsUnknown("onbekend", CultureInfo.InvariantCulture));
        }
    }
   
    public class Can_be_resolved_for
    {
        [Test]
        public void value_type_with_unknown_value()
        {
            Assert.AreEqual(PostalCode.Unknown, Unknown.Value(typeof(PostalCode)));
        }
        [Test]
        public void reference_type_with_unknown_value()
        {
            Assert.AreEqual(ClassWithUnknownValue.Unknown, Unknown.Value(typeof(ClassWithUnknownValue)));
        }
        
        private class ClassWithUnknownValue
        {
            public static ClassWithUnknownValue Unknown { get; } = new ClassWithUnknownValue();
        }
    }
    public class Can_not_be_resolved_for
    {
        [Test]
        public void null_type()
            => Assert.IsNull(Unknown.Value(null));

        [Test]
        public void value_type_without_unknown_value()
            => Assert.IsNull(Unknown.Value(typeof(Uuid)));

        [Test]
        public void value_type_with_unknown_value_of_wrong_type()
            => Assert.IsNull(Unknown.Value(typeof(ClassWithUnknownValueOfWrongType)));

        [Test]
        public void reference_type_without_unknown_value()
            => Assert.IsNull(Unknown.Value(typeof(object)));

        private class ClassWithUnknownValueOfWrongType
        {
            private ClassWithUnknownValueOfWrongType() { }
            public static object Unknown { get; } = new object();
        }
    }
}
