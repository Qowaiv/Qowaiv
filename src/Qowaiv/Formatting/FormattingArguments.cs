using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;

namespace Qowaiv.Formatting
{
	/// <summary>Represents formatting arguments.</summary>
	[DebuggerDisplay("{DebuggerDisplay}")]
	[Serializable]
	public struct FormattingArguments : ISerializable
	{
		/// <summary>Represents empty/not set formatting arguments.</summary>
		public static readonly FormattingArguments None = new FormattingArguments() { m_Format = default(String), m_FormatProvider = null };

		/// <summary>Initialzes a new instance of new formatformatting arguments.</summary>
		/// <param name="format">
		/// The format.
		/// </param>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public FormattingArguments(string format, IFormatProvider formatProvider)
		{
			m_Format = format;
			m_FormatProvider = formatProvider;
		}

		/// <summary>Initialzes a new instance of new formatformatting arguments.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>

		public FormattingArguments(IFormatProvider formatProvider) : this(null, formatProvider) { }

		/// <summary>Initialzes a new instance of new formatformatting arguments.</summary>
		/// <param name="format">
		/// The format.
		/// </param>
		public FormattingArguments(string format) : this(format, null) { }

		#region Properties

		private string m_Format;
		private IFormatProvider m_FormatProvider;

		/// <summary>Gets the format.</summary>
		public string Format { get { return m_Format; } }
		/// <summary>Gets the format provider.</summary>
		public IFormatProvider FormatProvider { get { return m_FormatProvider; } }

		#endregion

		#region Methods

		/// <summary>Formats the object using the formatting arguments.</summary>
		/// <param name="obj">
		/// The IFormattable object to get the formatted string representation from.
		/// </param>
		/// <returns>
		/// A formatted string representing the object.
		/// </returns>
		public string ToString(IFormattable obj)
		{
			if (obj == null) { return null; }
			return obj.ToString(this.Format, this.FormatProvider ?? CultureInfo.CurrentCulture);
		}

		/// <summary>Formats the object using the formatting arguments.</summary>
		/// <param name="obj">
		/// The object to get the formatted string representation from.
		/// </param>
		/// <returns>
		/// A formatted string representing the object.
		/// </returns>
		/// <remarks>
		/// If the object does not implement IFormattable, the ToString() will be used.
		/// </remarks>
		public string ToString(object obj)
		{
			if (obj == null) { return null; }
			var formattable = obj as IFormattable;
			if (formattable == null) { return obj.ToString(); }
			return ToString(formattable);
		}

		#endregion

		#region (De)serialization

		/// <summary>Initializes a new instance of formatting arguments based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private FormattingArguments(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			m_Format = info.GetString("Format");
			m_FormatProvider = (IFormatProvider)info.GetValue("FormatProvider", typeof(IFormatProvider));
		}

		/// <summary>Adds the underlying property of formatting arguments to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			info.AddValue("Format", m_Format);
			info.AddValue("FormatProvider", m_FormatProvider);
		}

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a System.String that represents the current formatting arguments for debug purposes.</summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
		private string DebuggerDisplay { get { return string.Format("Format: '{0}', Provider: {1}", this.Format, this.FormatProvider); } }

		#endregion

		#region IEquatable

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compare with.</param>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Returns the hash code for this formatting arguments.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode()
		{
			int hash = (m_Format == null) ? 0 : m_Format.GetHashCode();

			if (m_FormatProvider != null)
			{
				hash ^= m_FormatProvider.GetHashCode();
			}
			return hash;
		}

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(FormattingArguments left, FormattingArguments right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(FormattingArguments left, FormattingArguments right)
		{
			return !(left == right);
		}

		#endregion
	}
}