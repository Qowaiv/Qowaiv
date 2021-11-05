using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Hashing;
using Qowaiv.Security;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using System;

namespace Security.Secret_specs
{
    public class With_domain_logic
    {
        [TestCase(false, "Ken sent me!")]
        [TestCase(true, "")]
        public void IsEmpty_returns(bool result, Secret svo)
            => svo.IsEmpty().Should().Be(result);
    }
    public class Has_constant
    {
        [Test]
        public void Empty_represent_default_value()
            => Secret.Empty.Should().Be(default(Secret));
    }

    public class equality_is_limited_to_empty
    {
        [Test]
        public void not_equal_to_null()
            => Svo.Secret.Equals(null).Should().BeFalse();
        
        [Test]
        public void not_equal_to_other_type() 
            => Svo.Secret.Equals(new object()).Should().BeFalse();
        
        [Test]
        public void not_equal_to_different_value()
            => Svo.Secret.Equals(Secret.Parse("different")).Should().BeFalse();
        
        [Test]
        public void not_equal_to_same_value()
            => Svo.Secret.Equals(Secret.Parse("Ken sent me!")).Should().BeFalse();

        [Test]
        public void equal_to_for_two_empties() 
            => Secret.Empty.Equals(Secret.Empty).Should().BeTrue();

        [Test]
        public void hash_code_is_value_based()
        {
            Func<int> hash =() => Svo.Secret.GetHashCode();
            hash.Should().Throw<HashingNotSupported>();
        }
    }

    public class Can_be_parsed
    {
        [Test]
        public void from_null_string_represents_Empty()
        {
            Assert.AreEqual(Secret.Empty, Secret.Parse(null));
        }

        [Test]
        public void from_empty_string_represents_Empty()
        {
            Assert.AreEqual(Secret.Empty, Secret.Parse(string.Empty));
        }
    }

    public class Supports_type_conversion_from
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute() 
            => typeof(Secret).Should().HaveTypeConverterDefined();
        
        [Test]
        public void null_string()
            => Converting.From<string>(null).To<Secret>().Should().Be(Secret.Empty);

        [Test]
        public void empty_string()
            => Converting.From(string.Empty).To<Secret>().Should().Be(Secret.Empty);

        [Test]
        public void @string()
            => Converting.From("Ken sent me!").To<Secret>().Value()
            .Should().Be("Ken sent me!");
    }

    public class Does_not_support_type_converstion_to
    {
        [Test]
        public void convertered_is_null()
            => Converting.ToString().From(Svo.Secret).Should().BeNull();
    }

    public class Supports_JSON_deserialization
    {
        [Test]
        public void convention_based_deserialization()
            => JsonTester.Read<Secret>("Ken sent me!").Value().Should().Be(Svo.Secret.Value());

    }
    public class Does_not_supports_JSON_serialization
    {
        [Test]
        public void serializes_to_null()
            => JsonTester.Write(Svo.Secret).Should().BeNull();
    }

    public class Debugger
    {
        [TestCase("{empty}", "")]
        [TestCase("Ken sent me!", "Ken sent me!")]
        public void has_custom_display(object display, Secret svo) => svo.Should().HaveDebuggerDisplay(display);
    }
}
