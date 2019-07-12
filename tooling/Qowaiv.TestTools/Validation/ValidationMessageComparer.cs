using Qowaiv.Validation.Abstractions;
using System.Collections.Generic;

namespace Qowaiv.TestTools.Validiation
{
    /// <summary>Compares two instances of <see cref="IValidationMessage"/>.</summary>
    public class ValidationMessageComparer : IEqualityComparer<IValidationMessage>
    {
        /// <inheritdoc />
        public bool Equals(IValidationMessage x, IValidationMessage y)
        {
            if (x is null || y is null)
            {
                return ReferenceEquals(x, y);
            }
            return x.Message == y.Message
                && x.Severity == y.Severity
                && x.MemberName == y.MemberName;;
        }

        /// <inheritdoc />
        public int GetHashCode(IValidationMessage obj)
        {
            if (obj is null) { return 0; }

            return (obj.Message ?? "").GetHashCode()
                ^ (obj.MemberName ?? "").GetHashCode()
                ^ (int)obj.Severity;
        }
    }
}
