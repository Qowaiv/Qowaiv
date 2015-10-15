using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Text;

namespace Qowaiv.UnitTests.TestTools
{
	public static class DateTimeAssert
	{
		/// <summary>Verifies that two date times are equal. Two date times are
		/// considered equal if both are null, or have the same value.
		/// If they are not equal an NUnit.Framework.AssertionException is
		/// thrown.
		/// </summary>
		/// <param name="expected">
		/// The date time that is expected
		/// </param>
		/// <param name="actual">
		/// The actual date time
		/// </param>
		/// <param name="tolerance">
		/// The accepted tolerance between the actual and expected date time.
		/// </param>
		/// <param name="message">
		/// The message to display in case of failure.
		/// </param>
		/// <param name="args">
		/// Array of objects to be used in formatting the message
		/// </param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// If the specified tolerance is negative.
		/// </exception>
		/// <exception cref="NUnit.Framework.AssertionException">
		/// If the assertion fails.
		/// </exception>
		[DebuggerStepThrough]
		public static void AreEqual(DateTime? expected, DateTime? actual, string message, params object[] args)
		{
			AreEqual(expected, actual, TimeSpan.Zero, message, args);
		}

		/// <summary>Verifies that two date times are equal. Two date times are
		/// considered equal if both are null, or the difference between the
		/// date times is smaller then the specified tolerance.
		/// If they are not equal an NUnit.Framework.AssertionException is
		/// thrown.
		/// </summary>
		/// <param name="expected">
		/// The date time that is expected
		/// </param>
		/// <param name="actual">
		/// The actual date time
		/// </param>
		/// <param name="tolerance">
		/// The accepted tolerance between the actual and expected date time.
		/// </param>
		/// <param name="message">
		/// The message to display in case of failure.
		/// </param>
		/// <param name="args">
		/// Array of objects to be used in formatting the message
		/// </param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// If the specified tolerance is negative.
		/// </exception>
		/// <exception cref="NUnit.Framework.AssertionException">
		/// If the assertion fails.
		/// </exception>
		[DebuggerStepThrough]
		public static void AreEqual(DateTime? expected, DateTime? actual, TimeSpan tolerance, string message, params object[] args)
		{
			if (tolerance.Ticks < 0) { throw new ArgumentOutOfRangeException("tolerance", "Should be positive."); }

			if (actual.HasValue && expected.HasValue)
			{
				var difference = (actual.Value - expected.Value).Duration();

				if (difference > tolerance)
				{
					var sb = new StringBuilder();
					sb.AppendFormat("Expected:<{0:yyyy-MM-dd HH:mm:ss.FFFFFFF}>. ", actual);
					sb.AppendFormat("Actual:<{0:yyyy-MM-dd HH:mm:ss.FFFFFFF}>.", expected);
					if (tolerance > TimeSpan.Zero)
					{
						sb.AppendFormat(" Difference:<{0:#,##0.0######} seconds>.", difference.TotalSeconds);
					}
					if (!String.IsNullOrEmpty(message))
					{
						sb.Append(' ').AppendFormat(message, args);
					}
					Assert.Fail(sb.ToString());
				}
			}
			else
			{
				Assert.AreEqual(expected, actual, message, args);
			}
		}
	}
}
