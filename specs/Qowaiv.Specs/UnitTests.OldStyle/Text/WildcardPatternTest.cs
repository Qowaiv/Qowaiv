using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using Qowaiv.Text;
using System;
using System.Runtime.Serialization;

namespace Qowaiv.UnitTests.Text
{
    public class WildcardPatternTest
    {
        public static readonly WildcardPattern TestPattern = new WildcardPattern("t?st*", WildcardPatternOptions.SingleOrTrailing, StringComparison.Ordinal);

        [Test]
        public void Ctor_InvalidPattern_ThrowsArgumentException()
        {
            Action create = () => new WildcardPattern("**");
            create.Should()
                .Throw<ArgumentException>()
                .WithMessage("The wildcard pattern is invalid.");
        }
        [Test]
        public void Ctor_InvalidPatternSql_ThrowsArgumentException()
        {
            Action create = () => new WildcardPattern("%%", WildcardPatternOptions.SqlWildcards, StringComparison.CurrentCulture);
            create.Should()
                .Throw<ArgumentException>()
                .WithMessage("The wildcard pattern is invalid.");
        }

        [TestCase("*", "matches", true)]
        [TestCase("转*字", "转注字", true)]
        [TestCase("转?字", "转注字", true)]
        [TestCase("g*ks", "geeks", true)]
        [TestCase("g*éks", "geéks", true)]
        [TestCase("G*ks", "Geeks", true)]
        [TestCase("ge?ks*", "geeksforgeeks", true)]
        // Don't match on trailing
        [TestCase("gee?ks", "geeks", false)]
        // Don't match because of capitalization.
        [TestCase("g*ks", "Geeks", false)]
        [TestCase("g*Ks", "geeks", false)]
        // No because 'k' is not in second
        [TestCase("g*k", "gee", false)]
        // No because 't' is not in first
        [TestCase("*pqrs", "pqrst", false)]
        [TestCase("abc*bcd", "abcdhghgbcd", true)]
        // No because second must have 2 instances of 'c'
        [TestCase("abc*c?d", "abcd", false)]
        [TestCase("*c*d", "abcd", true)]
        [TestCase("*?c*d", "abcd", true)]
        // SQL wildcards.
        [TestCase("*_c%d", "abcd", false)]
        [TestCase("g%ks", "geeks", false)]
        [TestCase("转%字", "转注字", true, WildcardPatternOptions.SqlWildcards)]
        [TestCase("转_字", "转注字", true, WildcardPatternOptions.SqlWildcards)]
        [TestCase("g%ks", "geeks", true, WildcardPatternOptions.SqlWildcards)]
        [TestCase("g%éks", "geéks", true, WildcardPatternOptions.SqlWildcards)]
        [TestCase("G%ks", "Geeks", true, WildcardPatternOptions.SqlWildcards)]
        [TestCase("ge_ks%", "geeksforgeeks", true, WildcardPatternOptions.SqlWildcards)]
        // No because 'k' is not in second
        [TestCase("g%k", "gee", false, WildcardPatternOptions.SqlWildcards)]
        // No because 't' is not in first
        [TestCase("%pqrs", "pqrst", false, WildcardPatternOptions.SqlWildcards)]
        [TestCase("abc%bcd", "abcdhghgbcd", true, WildcardPatternOptions.SqlWildcards)]
        // No because second must have 2 instances of 'c'
        [TestCase("abc%c_d", "abcd", false, WildcardPatternOptions.SqlWildcards)]
        [TestCase("%c%d", "abcd", true, WildcardPatternOptions.SqlWildcards)]
        [TestCase("%_c%d", "abcd", true, WildcardPatternOptions.SqlWildcards)]
        // Default wildcards.
        [TestCase("*?c*d", "abcd", false, WildcardPatternOptions.SqlWildcards)]
        [TestCase("g*ks", "geeks", false, WildcardPatternOptions.SqlWildcards)]
        // SingleOrTrailing
        [TestCase("gee?ks", "geeks", true, WildcardPatternOptions.SingleOrTrailing)]
        [TestCase("ge?ks", "geeks", true, WildcardPatternOptions.SingleOrTrailing)]
        [TestCase("gee_ks", "geeks", true, WildcardPatternOptions.SingleOrTrailing | WildcardPatternOptions.SqlWildcards)]
        [TestCase("ge_ks", "geeks", true, WildcardPatternOptions.SingleOrTrailing | WildcardPatternOptions.SqlWildcards)]
        [TestCase("g*ks", "Geeks", true, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase,  "tr-TR")]
        [TestCase("g*Ks", "geeks", true, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase,  "tr-TR")]
        [TestCase("Ig*ks", "iGeeks", true, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase,  "tr-TR")]
        [TestCase("ig*ks", "IGeeks", true, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase,  "tr-TR")]
        [TestCase("Ig*ks", "ıGeeks", false, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase,  "tr-TR")]
        [TestCase("ıg*ks", "IGeeks", false, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase,  "tr-TR")]
        [TestCase("g*ks", "Geeks", true, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
        [TestCase("g*Ks", "geeks", true, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
        [TestCase("Ig*ks", "iGeeks", false, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
        [TestCase("ig*ks", "IGeeks", false, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
        [TestCase("Ig*ks", "ıGeeks", true, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
        [TestCase("ıg*ks", "IGeeks", true, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
        [TestCase("g*ks", "Geeks", true, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
        [TestCase("g*Ks", "geeks", true, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
        [TestCase("Ig*ks", "iGeeks", false, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
        [TestCase("ig*ks", "IGeeks", false, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
        [TestCase("Ig*ks", "ıGeeks", true, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
        [TestCase("ıg*ks", "IGeeks", true, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
        public void IsMatch(string pattern, string input, bool isMatch, WildcardPatternOptions options = default, StringComparison comparsionType = default, string culture = null)
        {
            using (culture is null ? CultureInfoScope.NewInvariant() : new CultureInfoScope(culture))
            {
                var actual = WildcardPattern.IsMatch(pattern, input, options, comparsionType);
                Assert.AreEqual(isMatch, actual, "'{0}' should {2} match '{1}', with {3} and {4}.", pattern, input, isMatch ? "" : "not ", options, comparsionType);
            }
        }
  
        #region (XML) (De)serialization tests

        [Test]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = TestPattern;
            var info = new SerializationInfo(typeof(Date), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

            Assert.AreEqual("t?st*", info.GetString("Pattern"));
            Assert.AreEqual((int)WildcardPatternOptions.SingleOrTrailing, info.GetInt32("Options"));
            Assert.AreEqual((int)StringComparison.Ordinal, info.GetInt32("ComparisonType"));
        }

        [Test]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var act = SerializeDeserialize.Binary(TestPattern);
            act.Should().HaveDebuggerDisplay("{t?st*}, SingleOrTrailing, Ordinal");
        }
        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var act = SerializeDeserialize.DataContract(TestPattern);
            act.Should().HaveDebuggerDisplay("{t?st*}, SingleOrTrailing, Ordinal");
        }

        #endregion
    }
}
