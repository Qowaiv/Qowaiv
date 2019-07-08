using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.TestTools.ComponentModel
{
    /// <summary>Compares two instances of <see cref="ValidationResult"/>.</summary>
    public class ValidationResultComparer : IEqualityComparer<ValidationResult>
    {
        /// <inheritdoc />
        public bool Equals(ValidationResult x, ValidationResult y)
        {
            if (x is null || y is null)
            {
                return ReferenceEquals(x, y);
            }
            return x.ErrorMessage == y.ErrorMessage
                && x.GetSeverity() == y.GetSeverity()
                && Enumerable.SequenceEqual(x.MemberNames, y.MemberNames);
        }

        /// <inheritdoc />
        public int GetHashCode(ValidationResult obj)
        {
            if (obj is null) { return 0; }

            var hash = (obj.ErrorMessage ?? "").GetHashCode() ^ (int)obj.GetSeverity();

            foreach (var memberName in obj.MemberNames)
            {
                hash ^= (memberName ?? "").GetHashCode();
            }
            return hash;
        }
    }
}
