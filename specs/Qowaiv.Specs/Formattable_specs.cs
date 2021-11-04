using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Specs;
using Qowaiv.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Formattable_specs
{
    public class ToString_with_format_and_provider
    {
        internal static IEnumerable<IFormattable> Svos => Svo.All().OfType<IFormattable>();

        [TestCaseSource(nameof(Svos))]
        public void null_and_string_empty_are_equal(IFormattable svo)
        {
            var string_empty = svo.ToString(string.Empty, TestCultures.Nl_NL);
            var @null = svo.ToString(null, TestCultures.Nl_NL);
            string_empty.Should().Be(@null);
        }
    }
}
