using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Text;
using Qowaiv.TestTools;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Qowaiv.UnitTests.Text
{
    public class WildcardPatternTest
    {
        public static readonly WildcardPattern TestPattern = new WildcardPattern("t?st*", WildcardPatternOptions.SingleOrTrailing, StringComparison.Ordinal);

        [Test]
        public void Ctor_InvalidPattern_ThrowsArgumentException()
        {
            ExceptionAssert.CatchArgumentException(() =>
            {
                new WildcardPattern("**");
            },
            "pattern",
            "The wildcard pattern is invalid.");
        }
        [Test]
        public void Ctor_InvalidPatternSql_ThrowsArgumentException()
        {
            ExceptionAssert.CatchArgumentException(() =>
            {
                new WildcardPattern("%%", WildcardPatternOptions.SqlWildcards, StringComparison.CurrentCulture);
            },
            "pattern",
            "The wildcard pattern is invalid.");
        }

        [Test]
        public void IsMatch_DefaultSettings_Matches()
        {
            IsMatch("转*字", "转注字", true);
            IsMatch("转?字", "转注字", true);
            IsMatch("g*ks", "geeks", true);
            IsMatch("g*éks", "geéks", true);
            IsMatch("G*ks", "Geeks", true);
            IsMatch("ge?ks*", "geeksforgeeks", true);

            // Don't match on trailing
            IsMatch("gee?ks", "geeks", false);

            // Don't match because of capitalization.
            IsMatch("g*ks", "Geeks", false);
            IsMatch("g*Ks", "geeks", false);

            // No because 'k' is not in second
            IsMatch("g*k", "gee", false);

            // No because 't' is not in first
            IsMatch("*pqrs", "pqrst", false);
            IsMatch("abc*bcd", "abcdhghgbcd", true);

            // No because second must have 2 instances of 'c'
            IsMatch("abc*c?d", "abcd", false);
            IsMatch("*c*d", "abcd", true);
            IsMatch("*?c*d", "abcd", true);

            // SQL wildcards.
            IsMatch("*_c%d", "abcd", false);
            IsMatch("g%ks", "geeks", false);
        }
        [Test]
        public void IsMatch_SqlWildCards_Matches()
        {
            var options = WildcardPatternOptions.SqlWildcards;

            IsMatch("转%字", "转注字", true, options);
            IsMatch("转_字", "转注字", true, options);
            IsMatch("g%ks", "geeks", true, options);
            IsMatch("g%éks", "geéks", true, options);
            IsMatch("G%ks", "Geeks", true, options);
            IsMatch("ge_ks%", "geeksforgeeks", true, options);

            // Don't match on trailing
            IsMatch("gee_ks", "geeks", false);

            // Don't match because of capitalization.
            IsMatch("g%ks", "Geeks", false);
            IsMatch("g%Ks", "geeks", false);

            // No because 'k' is not in second
            IsMatch("g%k", "gee", false, options);

            // No because 't' is not in first
            IsMatch("%pqrs", "pqrst", false, options);
            IsMatch("abc%bcd", "abcdhghgbcd", true, options);

            // No because second must have 2 instances of 'c'
            IsMatch("abc%c_d", "abcd", false, options);
            IsMatch("%c%d", "abcd", true, options);
            IsMatch("%_c%d", "abcd", true, options);

            // Default wildcards.
            IsMatch("*?c*d", "abcd", false, options);
            IsMatch("g*ks", "geeks", false, options);
        }
        [Test]
        public void IsMatch_SingleOrTrailing_Matches()
        {
            var options = WildcardPatternOptions.SingleOrTrailing;

            IsMatch("gee?ks", "geeks", true, options);
            IsMatch("gee_ks", "geeks", true, options | WildcardPatternOptions.SqlWildcards);

            IsMatch("ge?ks", "geeks", true, options);
            IsMatch("ge_ks", "geeks", true, options | WildcardPatternOptions.SqlWildcards);
        }

        [Test]
        public void IsMatch_InvariantCultureIgnoreCase_Matches()
        {
            using (new CultureInfoScope("tr-TR"))
            {
                var options = WildcardPatternOptions.None;
                var compare = StringComparison.InvariantCultureIgnoreCase;

                IsMatch("g*ks", "Geeks", true, options, compare);
                IsMatch("g*Ks", "geeks", true, options, compare);

                IsMatch("Ig*ks", "iGeeks", true, options, compare);
                IsMatch("ig*ks", "IGeeks", true, options, compare);

                IsMatch("Ig*ks", "ıGeeks", false, options, compare);
                IsMatch("ıg*ks", "IGeeks", false, options, compare);
            }
        }
        [Test]
        public void IsMatch_CurrentCultureIgnoreCase_Matches()
        {
            using (new CultureInfoScope("tr-TR"))
            {
                var options = WildcardPatternOptions.None;
                var compare = StringComparison.CurrentCultureIgnoreCase;

                IsMatch("g*ks", "Geeks", true, options, compare);
                IsMatch("g*Ks", "geeks", true, options, compare);

                IsMatch("Ig*ks", "iGeeks", false, options, compare);
                IsMatch("ig*ks", "IGeeks", false, options, compare);

                IsMatch("Ig*ks", "ıGeeks", true, options, compare);
                IsMatch("ıg*ks", "IGeeks", true, options, compare);
            }
        }
        [Test]
        public void IsMatch_OrdinalIgnoreCase_Matches()
        {
            using (new CultureInfoScope("tr-TR"))
            {
                var options = WildcardPatternOptions.None;
                var compare = StringComparison.OrdinalIgnoreCase;

                IsMatch("g*ks", "Geeks", true, options, compare);
                IsMatch("g*Ks", "geeks", true, options, compare);

                IsMatch("Ig*ks", "iGeeks", false, options, compare);
                IsMatch("ig*ks", "IGeeks", false, options, compare);

                IsMatch("Ig*ks", "ıGeeks", true, options, compare);
                IsMatch("ıg*ks", "IGeeks", true, options, compare);
            }
        }

        [Test]
        public void IsMatch_Static_Matches()
        {
            Assert.IsTrue(WildcardPattern.IsMatch("*", "matches"));
        }

        #region (XML) (De)serialization tests

        [Test]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<WildcardPattern>(null, default);
            },
            "info");
        }

        [Test]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            Assert.Catch<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(WildcardPattern), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<WildcardPattern>(info, default);
            });
        }

        [Test]
        public void GetObjectData_Null_ThrowsArgumentNullException()
        {
            ExceptionAssert.CatchArgumentNullException
            (() =>
            {
                ISerializable obj = new WildcardPattern("*");
                obj.GetObjectData(null, default);
            },
            "info");
        }

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
            var act = SerializationTest.SerializeDeserialize(TestPattern);
            DebuggerDisplayAssert.HasResult("{t?st*}, SingleOrTrailing, Ordinal", act);
        }
        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var act = SerializationTest.DataContractSerializeDeserialize(TestPattern);
            DebuggerDisplayAssert.HasResult("{t?st*}, SingleOrTrailing, Ordinal", act);
        }

        #endregion

        [Test]
        public void DebuggerDisplay_Simple_DebuggerString()
        {
            DebuggerDisplayAssert.HasResult("{t?st*}", new WildcardPattern("t?st*"));
        }
        [Test]
        public void DebuggerDisplay_TestPattern_DebuggerString()
        {
            DebuggerDisplayAssert.HasResult("{t?st*}, SingleOrTrailing, Ordinal", TestPattern);
        }

        [DebuggerStepThrough]
        private void IsMatch(string pattern, string input, bool isMatch, WildcardPatternOptions options = WildcardPatternOptions.None, StringComparison comparsionType = StringComparison.CurrentCulture)
        {
            Assert.AreEqual(isMatch, WildcardPattern.IsMatch(pattern, input, options, comparsionType),
                "'{0}' should {2} match '{1}', with {3} and {4}.", pattern, input, isMatch ? "" : "not ", options, comparsionType);
        }
    }
}
