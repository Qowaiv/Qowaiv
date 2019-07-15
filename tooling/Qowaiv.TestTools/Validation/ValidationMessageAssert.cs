using Qowaiv.Validation.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Qowaiv.TestTools.Validiation
{
    /// <summary>Assertion helper class on <see cref="Result"/> and/or <see cref="Result{T}"/>.</summary>
    public static class ValidationMessageAssert
    {
        private static readonly ValidationMessageComparer Comparer = new ValidationMessageComparer();

        /// <summary>Asserts that the result is valid, throws if not.</summary>
        [DebuggerStepThrough]
        public static void IsValid(Result result, params IValidationMessage[] expectedMessages)
        {
            Assert.IsNotNull(result, "The result is null.");
            SameMessages(expectedMessages, result.Messages);
            Assert.IsTrue(result.IsValid, "The result is not valid");
        }

        /// <summary>Asserts that result contains expected messages. Throws if not.</summary>
        [DebuggerStepThrough]
        public static void WithErrors(Result result, params IValidationMessage[] expectedMessages)
        {
            Assert.IsNotNull(result, "The result is null.");
            SameMessages(expectedMessages, result.Messages);
        }

        /// <summary>Asserts that two collections contain the same error messages, Throws if not.</summary>
        [DebuggerStepThrough]
        public static void SameMessages(IEnumerable<IValidationMessage> expectedErrors, IEnumerable<IValidationMessage> actualErrors)
        {
            Assert.IsNotNull(expectedErrors, "The expected messages are null.");
            Assert.IsNotNull(actualErrors, "The actual messages are null.");

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
        private static string ToString(IEnumerable<IValidationMessage> messages)
        {
            var sb = new StringBuilder();
            foreach (var message in messages.OrderByDescending(m => m.Severity))
            {
                Append(sb, message);
            }
            return sb.ToString();
        }

        private static void Append(StringBuilder sb, IValidationMessage message)
        {
            sb.AppendFormat("- {0,-7} ", message.Severity.ToString().ToUpperInvariant())
                .Append(message.Message);

            if (!string.IsNullOrEmpty(message.PropertyName))
            {
                sb.Append(" Member: ").AppendLine(message.PropertyName);
            }
        }
    }
}
