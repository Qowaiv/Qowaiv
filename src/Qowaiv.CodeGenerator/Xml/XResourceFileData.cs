using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace Qowaiv.CodeGenerator.Xml
{
	/// <summary>Represents data of a resource file.</summary>
	[Serializable, XmlType("data"), DebuggerDisplay("{DebuggerDisplay}")]
	public class XResourceFileData
	{
		/// <summary>Initializes a new instance of resource file data.</summary>
		protected XResourceFileData() { }

		/// <summary>Initializes a new instance of resource file data.</summary>
		public XResourceFileData(string name, string val, string comment = null)
		{
			if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("name"); }
			this.Name = name;
			this.Value = val;
			// not add empty comments.
			this.Comment = String.IsNullOrEmpty(comment) ? null : comment;
		}

		/// <summary>Gets and set the name.</summary>
		[XmlAttribute("name")]
		public string Name { get; set; }

		/// <summary>Gets and set the value.</summary>
		[XmlElement("value")]
		public string Value { get; set; }

		/// <summary>Gets and set a comment.</summary>
		[XmlElement("comment")]
		public string Comment { get; set; }

		/// <summary>Represents the resource file data as debug string.</summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture,
					"Data, Name: {0}, Value: '{1}'{2}",
					Name,
					Value,
					string.IsNullOrEmpty(Comment) ? "" : ", Comment: '" + Comment + "'");
			}
		}
	}
}
