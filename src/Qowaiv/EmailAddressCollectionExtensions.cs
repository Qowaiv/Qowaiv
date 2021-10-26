using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Qowaiv
{
    /// <summary>Contains extensions for email address collection.</summary>
    public static class EmailAddressCollectionExtensions
    {
        /// <summary>Converts the enumeration into an email address collection.</summary>
        [Pure]
        public static EmailAddressCollection ToCollection(this IEnumerable<EmailAddress> emailaddresses)
            => new(emailaddresses);
    }
}
