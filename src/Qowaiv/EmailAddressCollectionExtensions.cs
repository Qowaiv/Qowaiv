using System.Collections.Generic;

namespace Qowaiv
{
	/// <summary>Contains extensions for email address collection.</summary>
	public static class EmailAddressCollectionExtensions
	{
		/// <summary>Converts the enumeration into an email address collection.</summary>
		public static EmailAddressCollection ToCollection(this IEnumerable<EmailAddress> emailaddresses)
		{
			return new EmailAddressCollection(emailaddresses);
		}
	}
}
