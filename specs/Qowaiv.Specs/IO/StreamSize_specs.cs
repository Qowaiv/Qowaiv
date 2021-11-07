﻿using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Hashing;
using Qowaiv.IO;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.Globalization;

namespace IO.StreamSize_specs
{
    public class Can_be_parsed
    {
        [TestCase("en", "123456789")]
        [TestCase("en", "123456.789 kB")]
        [TestCase("en", "123456.789 kilobyte")]
        [TestCase("en", "123.456789 MB")]
        [TestCase("nl", "123,456789 MB")]
        [TestCase("nl", "0,123456789 GB")]
        public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
        {
            using (culture.Scoped())
            {
                StreamSize.Parse(input).Should().Be(Svo.StreamSize);
            }
        }

        [Test]
        public void from_valid_input_only_otherwise_throws_on_Parse()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Func<StreamSize> parse = () => StreamSize.Parse("invalid input");
                parse.Should().Throw<FormatException>().WithMessage("Not a valid stream size");
            }
        }

        [Test]
        public void from_valid_input_only_otherwise_return_false_on_TryParse()
            => (StreamSize.TryParse("invalid input", out _)).Should().BeFalse();

        [Test]
        public void from_invalid_as_null_with_TryParse()
            => StreamSize.TryParse("invalid input").Should().Be(StreamSize.Zero);

        [Test]
        public void with_TryParse_returns_SVO()
            => StreamSize.TryParse("123456789").Should().Be(Svo.StreamSize);
    }

    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
            => Svo.StreamSize.Equals(null).Should().BeFalse();

        [Test]
        public void not_equal_to_other_type()
            => Svo.StreamSize.Equals(new object()).Should().BeFalse();

        [Test]
        public void not_equal_to_different_value()
            => Svo.StreamSize.Equals(StreamSize.MinValue).Should().BeFalse();

        [Test]
        public void equal_to_same_value()
            => Svo.StreamSize.Equals(StreamSize.Byte * 123456789).Should().BeTrue();

        [Test]
        public void equal_operator_returns_true_for_same_values()
            => (StreamSize.Byte * 123456789 == Svo.StreamSize).Should().BeTrue();

        [Test]
        public void equal_operator_returns_false_for_different_values()
            => (StreamSize.Byte * 123456789 == StreamSize.MinValue).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
            => (StreamSize.Byte * 123456789 != Svo.StreamSize).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
            => (StreamSize.Byte * 123456789 != StreamSize.MinValue).Should().BeTrue();

        [TestCase("0 byte", 0)]
        [TestCase("123456789 byte", 553089222)]
        public void hash_code_is_value_based(StreamSize svo, int hash)
        {
            using (Hash.WithoutRandomizer())
            {
                svo.GetHashCode().Should().Be(hash);
            }
        }
    }

    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
            => typeof(StreamSize).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.From<string>(null).To<StreamSize>().Should().Be(StreamSize.Zero);
            }
        }

        [TestCase("123456789")]
        [TestCase("123456.789 kB")]
        public void from_string(string str)
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.From(str).To<StreamSize>().Should().Be(Svo.StreamSize);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.ToString().From(Svo.StreamSize).Should().Be("123456789 byte");
            }
        }

        [Test]
        public void from_long()
            => Converting.From(123456789L).To<StreamSize>().Should().Be(Svo.StreamSize);

        [Test]
        public void to_long()
            => Converting.To<long>().From(Svo.StreamSize).Should().Be(123456789);
    }
}
