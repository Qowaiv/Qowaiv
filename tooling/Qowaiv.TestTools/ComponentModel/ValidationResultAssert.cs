using NUnit.Framework;
using Qowaiv.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Qowaiv.TestTools.ComponentModel
{
    /// <summary>Assertion helper class on <see cref="Result"/> and/or <see cref="Result{T}"/>.</summary>
    public static class ValidationResultAssert
    {
        private static readonly ValidationResultComparer Comparer = new ValidationResultComparer();

        /// <summary>Asserts that the result is valid, throws if not.</summary>
        [DebuggerStepThrough]
        public static void IsValid(Result result, params ValidationResult[] expectedMessages)
        {
            Assert.NotNull(result, "The result is null.");
            SameMessages(expectedMessages, result.Messages);
            Assert.IsTrue(result.IsValid, "The result is not valid");
        }

        /// <summary>Asserts that result contains expected messages. Throws if not.</summary>
        [DebuggerStepThrough]
        public static void WithErrors(Result result, params ValidationResult[] expectedMessages)
        {
            Assert.NotNull(result, "The result is null.");
            SameMessages(expectedMessages, result.Messages);
        }

        /// <summary>Asserts that two collections contain the same error messages, Throws if not.</summary>
        [DebuggerStepThrough]
        public static void SameMessages(IEnumerable<ValidationResult> expectedErrors, IEnumerable<ValidationResult> actualErrors)
        {
            Assert.NotNull(expectedErrors, "The expected messages are null.");
            Assert.NotNull(actualErrors, "The actual messages are null.");

            var actual = actualErrors.GetWithSeverity().ToArray();
            var expected = expectedErrors.GetWithSeverity().ToArray();

            if (!expected.Any() && actual.Any())
            {
                Assert.Fail($"Expected: no messages{Environment.NewLine}But had:{Environment.NewLine}{ToString(actual)}");
            }
            if (actual.Any() && !expected.Any())
            {
                Assert.Fail($"Expected:{Environment.NewLine}{ToString(expected)}But had: no messages");
            }

            var missing = expectedErrors.Except(actualErrors, Comparer).ToArray();
            var extra = actualErrors.Except(expectedErrors, Comparer).ToArray();

            var sb = new StringBuilder();

            if (missing.Any())
            {
                sb.AppendLine($"Missing message{(missing.Length == 1 ? "" :"s")}:").AppendLine(ToString(missing));
            }
            if (extra.Any())
            {
                sb.AppendLine($"Extra message{(extra.Length == 1 ? "" : "s")}:").AppendLine(ToString(extra));
            }

            if (sb.Length != 0)
            {
                Assert.Fail(sb.ToString());
            }
        }
        private static string ToString(IEnumerable<ValidationResult> messages)
        {
            var sb = new StringBuilder();
            foreach (var message in messages.OrderByDescending(m => m.GetSeverity()))
            {
                Append(sb, message);
            }
            return sb.ToString();
        }

        private static void Append(StringBuilder sb, ValidationResult result)
        {
            sb.AppendFormat("- {0,-7} ", result.GetSeverity().ToString().ToUpperInvariant())
                .Append(result.ErrorMessage);

            if (result.MemberNames.Count() == 1)
            {
                sb.Append(" Member: ").AppendLine(result.MemberNames.First());
            }
            else if (result.MemberNames.Any())
            {
                sb.Append("Members: ").AppendLine(string.Join(", ", result.MemberNames));
            }
        }
    }
}
